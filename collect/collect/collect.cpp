// collect.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include "CLog.h"
#include "AdoHelper.h"
#include "globle.h"
#include "ThreadManager.h"
#include "DataThread.h"
#include "DBFactory.h"
#include <my_global.h>
#include <mysql.h>

//////////////////////////////////////////////////////////////////////////
#include "CThread.h"
//////////////////////////////////////////////////////////////////////////
#pragma warning(disable:4996) 
int _tmain(int argc, _TCHAR* argv[])
{
	//应用程序只能启动一次
	HANDLE hexeMutex;
	hexeMutex = CreateMutex(NULL, true, "collect.exe");
	if (GetLastError() == ERROR_ALREADY_EXISTS)
	{
		return 0;
	}

	//初始化COM接口
	CoInitialize(NULL);

	//初始化多线程环境
	int rc = mysql_library_init(0, NULL, NULL);	

	printf("\t\t\t[游戏数据采集服务器]\n\n");

	CLog g_log;	

	try
	{
		while (true)
		{
			char strConn_info[255];
			char strQuery[512];
			char strKey[30];
			SYSTEMTIME CurrentTime;

			CAdoHelper adoHelper;

			//根据数据库信息生成需要的文件夹以及ctl文件
			if (!CreateCTL())
			{
				printf("创建文件失败，按确定键退出，并检查！\n");
			}

			//清理已经完成线程对象
			g_threadManager->clearFinalthread();

			//获取连接字符串
			MakeLocalConnectString(strConn_info);

			if (!adoHelper.connDB(strConn_info))
			{
				printf("连接数据库失败，请查看日志\n");
			}
			else
			{
				_RecordsetPtr pRset_game = NULL;

				//获取需要采集的游戏列表
//				sprintf(strQuery, "select * from import_express_temp_my where 0 = 1");
				sprintf(strQuery, "{call pd_gameinfo_pack.pd_sqlexpress_query_my()}");

				pRset_game = adoHelper.execQuery(strQuery);
				if(pRset_game)
				{
					int iDbType, taskid, gamedbid;
					while (!pRset_game->adoEOF)
					{
						taskid = atoi((char *)_bstr_t(pRset_game->GetCollect("EXPRESS_ID")));
						gamedbid = atoi((char *)_bstr_t(pRset_game->GetCollect("GAMEDB_ID")));
						sprintf(strKey, "%s_%d_%d", "taskid", taskid, gamedbid);

						iDbType = atoi((char *)_bstr_t(pRset_game->GetCollect("type_db")));

						CDataThread *newThread = CDBFactory::CreateDBThread(iDbType);
						newThread->m_iExpressId = atoi((char *)_bstr_t(pRset_game->GetCollect("EXPRESS_ID")));
						newThread->m_strServerIp = (char *)_bstr_t(pRset_game->GetCollect("SERVER_IP"));
						newThread->m_strDBName = (char *)_bstr_t(pRset_game->GetCollect("SERVER_DB"));
						newThread->m_strSrcTable = (char *)_bstr_t(pRset_game->GetCollect("SRCTABLE"));
						newThread->m_strDesTable = (char *)_bstr_t(pRset_game->GetCollect("DESTABLE"));
						newThread->m_strUserId = (char *)_bstr_t(pRset_game->GetCollect("USER_ID"));
						newThread->m_strPassWd = (char *)_bstr_t(pRset_game->GetCollect("USER_PWD"));
						newThread->m_strServerName = (char *)_bstr_t(pRset_game->GetCollect("SERVER_NAME"));
						newThread->m_iGameId = atoi((char *)_bstr_t(pRset_game->GetCollect("GAME_ID")));
						newThread->m_strGameName = (char *)_bstr_t(pRset_game->GetCollect("game_name"));
						newThread->m_iPort = atoi((char *)_bstr_t(pRset_game->GetCollect("PORT")));

						newThread->m_strBcpName = (char *)_bstr_t(pRset_game->GetCollect("BCP_NAME"));
						newThread->m_strBcpExpress = (char *)_bstr_t(pRset_game->GetCollect("BCP_SQL"));
						newThread->m_strCtlName = (char *)_bstr_t(pRset_game->GetCollect("CTLNAME"));

						newThread->m_strTns = (char *)_bstr_t(pRset_game->GetCollect("Ora_Tns"));
						newThread->m_strColUser = (char *)_bstr_t(pRset_game->GetCollect("collect_id"));
						newThread->m_strColPwd = (char *)_bstr_t(pRset_game->GetCollect("collect_pwd"));
						newThread->m_iDbType = iDbType;

						g_threadManager->addThread(strKey, newThread);

						pRset_game->MoveNext();
					}
					pRset_game->Close();
				}
				else
				{
					printf("THERE IS NO TASK TO DO!");
				}
			}

			//管理线程初始化各个线程对象
			g_threadManager->initialize();

			Sleep(120*1000);
			GetLocalTime(&CurrentTime);
			
			printf("等待新任务, 当前时间：%4d/%02d/%02d %02d:%02d:%02d ...\n", 
				CurrentTime.wYear,CurrentTime.wMonth,CurrentTime.wDay,CurrentTime.wHour,CurrentTime.wMinute,CurrentTime.wSecond);

		}


		/*
		while(TRUE)
		{
			Sleep(3600*1000);

			//清理已经完成线程对象
			g_threadManager->clearFinalthread();
			
			//连接数据库，取新的记录
			char strConn_info[255];
			char strQuery[512];
			char strKey[30];

			CAdoHelper adoHelper;

			//根据数据库信息生成需要的文件夹以及ctl文件
			if (!CreateCTL())
			{
				printf("创建文件失败，按确定键退出，并检查！");
			}

			//获取连接字符串
			MakeLocalConnectString(strConn_info);

			if (!adoHelper.connDB(strConn_info))
			{
				printf("连接数据库失败，请查看日志\n");
			}
			else
			{
				_RecordsetPtr pRset_game = NULL;

				//获取需要采集的游戏列表
//				sprintf(strQuery, "select * from Game_Info where flag = 1 and type_db in (%s)", strDBType);
				sprintf(strQuery, "{call pd_sqlexpress_query_my}");
				pRset_game = adoHelper.execQuery(strQuery);
				if(pRset_game)
				{
					int iDbType, taskid, gamedbid;
					while (!pRset_game->adoEOF)
					{
						taskid = atoi((char *)_bstr_t(pRset_game->GetCollect("EXPRESS_ID")));
						gamedbid = atoi((char *)_bstr_t(pRset_game->GetCollect("GAMEDB_ID")));
						sprintf(strKey, "%s_%d_%d", "taskid", taskid, gamedbid);

						iDbType = atoi((char *)_bstr_t(pRset_game->GetCollect("type_db")));

						CDataThread *newThread = CDBFactory::CreateDBThread(iDbType);
						newThread->m_iExpressId = atoi((char *)_bstr_t(pRset_game->GetCollect("EXPRESS_ID")));
						newThread->m_strServerIp = (char *)_bstr_t(pRset_game->GetCollect("SERVER_IP"));
						newThread->m_strDBName = (char *)_bstr_t(pRset_game->GetCollect("SERVER_DB"));
						newThread->m_strSrcTable = (char *)_bstr_t(pRset_game->GetCollect("SRCTABLE"));
						newThread->m_strDesTable = (char *)_bstr_t(pRset_game->GetCollect("DESTABLE"));
						newThread->m_strUserId = (char *)_bstr_t(pRset_game->GetCollect("USER_ID"));
						newThread->m_strPassWd = (char *)_bstr_t(pRset_game->GetCollect("USER_PWD"));
						newThread->m_strServerName = (char *)_bstr_t(pRset_game->GetCollect("SERVER_NAME"));
						newThread->m_iGameId = atoi((char *)_bstr_t(pRset_game->GetCollect("GAME_ID")));
						newThread->m_strGameName = (char *)_bstr_t(pRset_game->GetCollect("game_name"));

						newThread->m_strBcpName = (char *)_bstr_t(pRset_game->GetCollect("BCP_NAME"));
						newThread->m_strBcpExpress = (char *)_bstr_t(pRset_game->GetCollect("BCP_SQL"));
						newThread->m_strCtlName = (char *)_bstr_t(pRset_game->GetCollect("CTLNAME"));

						newThread->m_strTns = (char *)_bstr_t(pRset_game->GetCollect("Ora_Tns"));
						newThread->m_strColUser = (char *)_bstr_t(pRset_game->GetCollect("collect_id"));
						newThread->m_strColPwd = (char *)_bstr_t(pRset_game->GetCollect("collect_pwd"));
						newThread->m_iDbType = iDbType;

						g_threadManager->addThread(strKey, newThread);

						pRset_game->MoveNext();
					}
					pRset_game->Close();
				}
				else
				{
					printf("THERE IS NO NEW TASK TO DO");
				}
			}

			//管理线程初始化各个线程对象
			g_threadManager->initialize();*/

		mysql_library_end();		

		//关闭COM接口
		CoUninitialize();
	}
	catch(_com_error &e)
	{
		printf((char *)e.Description());
		g_log.WriteLog((char *)e.Description());
	}
	catch(...)
	{
		printf("Main Thread Unexpected Error");
		g_log.WriteLog("Main Thread Unexpected Error");
	}

	CloseHandle(hexeMutex);
	getchar();
	return 0;
}