#include "stdafx.h"

#include "MysqlThread.h"
#include "AdoHelper.h"
#include <process.h>
#include <my_global.h>
#include <my_sys.h>
#include <my_alloc.h>
#include <m_string.h>
#include <mysql.h>
#include "globle.h"

#pragma comment(lib,"libmysql.lib")

struct tagMag
{
	CMysqlThread * myThread;
	int iBegin;
	int iNumber;
	int threadNo;
};

CMysqlThread::CMysqlThread(void)
{
}

CMysqlThread::~CMysqlThread(void)
{
}

DWORD CMysqlThread::run()
{
	try
	{
		char path[256];
		char bcpPath[256];

		GetCurrentDirectory(256,path);//获取当前目录
		sprintf(bcpPath, "%s%s",path,"\\temp\\");

 		SYSTEMTIME sys;
		GetLocalTime(&sys);
		printf("开始时间：%02d:%02d:%02d:%0004d\n", sys.wHour, sys.wMinute, sys.wSecond, sys.wMilliseconds);

		if(QueryTotalRecord())
		{
			
			char strCmd[255];
			int iCmdRet = -1;

			if (!WriteRecord())
			{
				//将错误日志记入数据库日志
				WriteDBlog(2, "FAIL_BCP");
				m_FinishFlag = 1;
				return -1;
			}

			//执行sqlload导入工作
			ExecSQLDR();


			printf("%s-%s-%s:采集结束\n",m_strServerName.c_str(), m_strDBName.c_str(), m_strSrcTable.c_str());

			
			char textpath[255];
			sprintf(textpath, "%s\\%s\\%s", bcpPath, m_strGameName.c_str(), m_strBcpName.c_str());

			if(remove(textpath) == -1)
				printf("%s is not exists!", textpath);


		}
		else
		{
			printf("Query Error\n");
			WriteDBlog(2, "COLLECT_BCP");
			m_FinishFlag = 1;
			return -1;
		}

	}
	catch(...)
	{
		//记录线程运行错误日志
		printf("Thread Run Unknown Error\n");
		WriteCollectLog("Thread Run Unknown Error");
		m_FinishFlag = 1;
		return -1;
	}

	m_FinishFlag = 1;
	return 0;
}

bool CMysqlThread::WriteRecord()
{
	//	string strRecord;
	int iRecordNum = 0;
	int iFieldNum = 0;
	int rc;
	char strMsg[512];
	char path[256];
	char filepath[256];
//	char strQuery[512];
	string strQuery;
	int error = -1;

	GetCurrentDirectory(256,path);//获取当前目录
	sprintf(filepath,"%s\\%s\\%s\\%s",path,"temp",m_strGameName.c_str(), m_strBcpName.c_str());//获取scheme.ini文件路径

	string strSev;
	string tempSub;

	char replacestr[512];
	int iSevPos = m_strBcpExpress.find(",");
	iSevPos = m_strBcpExpress.find(",", iSevPos+1);
	int iBcpSize = m_strBcpExpress.size();
	tempSub = m_strBcpExpress.substr(0,iSevPos);

	sprintf(replacestr,tempSub.c_str(), m_strServerIp.c_str(),m_strServerName.c_str());//m_strDBName.c_str()

	strQuery = replacestr;
	tempSub = m_strBcpExpress.substr(iSevPos, iBcpSize-iSevPos);
	strQuery += tempSub;

	WriteCollectLog(const_cast<char *>(strQuery.c_str()));

	try
	{
		while (error)
		{
			my_init();
			mysql_thread_init();

			//初始化MYSQL句柄
			MYSQL *localMysql = NULL;
			localMysql = mysql_init(NULL);

			if (localMysql == NULL)
			{
				sprintf(strMsg, "init mysql error \n");
				WriteCollectLog(strMsg);

				Sleep(3000);
				continue;

			}

			unsigned int timeout = 1200;
			mysql_options(localMysql, MYSQL_OPT_CONNECT_TIMEOUT,&timeout);

			timeout = 3600;
			mysql_options(localMysql, MYSQL_OPT_READ_TIMEOUT,&timeout);

			//连接mysql数据库
			if (mysql_real_connect(localMysql, m_strServerIp.c_str(),m_strUserId.c_str(), m_strPassWd.c_str(),m_strDBName.c_str(), m_iPort, NULL, (unsigned long)NULL) == NULL)
			{
				sprintf(strMsg, "connect to mysql server %s error, errno=%d, errmsg=%s \n", m_strServerIp.c_str(), mysql_errno(localMysql), mysql_error(localMysql));
				WriteCollectLog(strMsg);
				mysql_close(localMysql);
				mysql_thread_end();

				Sleep(3000);
				continue;
			}

			Sleep(1000);

			//创建打开导入文本文件
			FILE *fHandle = NULL;
			fHandle = fopen(filepath, "wb");
			if (fHandle == NULL)
			{
				sprintf(strMsg, "open error");
				WriteCollectLog(strMsg);
				return FALSE;
			}

			//设置连接使用字符集
			mysql_set_character_set(localMysql, "GBK");
			rc = mysql_query(localMysql, strQuery.c_str());
			if (rc)
			{
				sprintf(strMsg, "run : sql '%s' error, errno=%d, errmsg=%s \n", strQuery.c_str(), mysql_errno(localMysql), mysql_error(localMysql));
				WriteCollectLog(strMsg);
				mysql_close(localMysql);
				mysql_thread_end();

				fclose(fHandle);

				Sleep(3000);
				continue;
			}

			//获取结果集句柄 
			//mysql_store_result模式：全部记录读到客户端缓冲区  
			//mysql_use_result模式：每次取记录时都去服务器端读取数据
			MYSQL_RES *localRes = mysql_use_result(localMysql);
			if (localRes == NULL)
			{
				sprintf(strMsg, "no result \n");
				mysql_close(localMysql);
				mysql_thread_end();

				fclose(fHandle);

				Sleep(3000);
				continue;
			}

			//获取列的数目
			iFieldNum = mysql_num_fields(localRes);

			//循环获取结果集数据
			MYSQL_ROW localRow = NULL;
			int realNum =0;
			int printNum =0;
			while ((localRow=mysql_fetch_row(localRes)) != NULL)
			{
				int m=0; 
				string strRecord;
				unsigned long *lengths;
				while (m<iFieldNum-1)
				{
//					strRecord += (localRow[m]==NULL?"null":localRow[m]);
					strRecord += ((int) lengths[m], localRow[m] ? localRow[m] : "NULL");
					strRecord += "\t";
					m++;
				}

				strRecord += ((int) lengths[m], localRow[m] ? localRow[m] : "NULL");
				strRecord += "\r\n";

				fprintf(fHandle,"%s", strRecord.c_str());

				printNum++;

				if(printNum%1000 == 0)
				{
					printf("1000条记录写入主文件, 写入记录总数: %d\n", printNum);
				}

				if (WaitForSingleObject(m_eventThreadStop, 0) != WAIT_TIMEOUT)
				{
					fclose(fHandle);
					mysql_free_result(localRes);
					mysql_close(localMysql);
					mysql_thread_end();
					return FALSE;
				}

			}

			if(printNum%1000)
			{
				printf("%d条记录写入主文件, 写入记录总数: %d\n", printNum%1000, printNum);
			}

			error = mysql_errno(localMysql);

			sprintf(strMsg, "%s, error = %d, errormsg = %s",  strQuery.c_str(), error, mysql_error(localMysql));
			WriteCollectLog(strMsg);

			realNum = mysql_num_rows(localRes);

			sprintf(strMsg, "%s, %d, %d",  strQuery.c_str(), realNum, m_iRecordNum);
			WriteCollectLog(strMsg);

			fclose(fHandle);

			//释放结果集资源
			mysql_free_result(localRes);

			//释放MYSQL句柄
			mysql_close(localMysql);

			mysql_thread_end();

			Sleep(3000);
		}
	}
	catch(...)
	{
		sprintf(strMsg, "生成文本：%s 发送未知错误", strQuery.c_str());
		WriteCollectLog(strMsg);
		return FALSE;
	}

	return TRUE;
}