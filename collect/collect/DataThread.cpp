#include "stdafx.h"
#include "DataThread.h"
#include "AdoHelper.h"
#include "globle.h"

#pragma warning(disable:4996)

extern void MakeLocalConnectString(char *strConnect);

CDataThread::CDataThread(void)
{
//	m_bCollect = FALSE;
	m_iRecordNum = 0;
	m_iInsertNum = 0;
	m_FinishFlag = 0;
}

CDataThread::~CDataThread(void)
{
}

int CDataThread::GetInsertNum(const int Line)
{
	FILE   *fHandle;
	char buf[4096], filePath[256], strPath[255];
	int iLine = 0;
	long lRecordNum = -1;
	char strNum[20];

	int pos = m_strCtlName.find('.');
	string strname = m_strCtlName.substr(0, pos);

	GetCurrentDirectory(256, filePath);
	sprintf(strPath, "%s\\LoadLog\\%s\\%s.log", filePath, m_strGameName.c_str(), strname.c_str());

	fHandle = fopen(strPath,"r");
	if(fHandle == NULL)
	{    
		return lRecordNum;  
	}

	while(!feof(fHandle)) 
	{  
		fgets(buf,sizeof(buf),fHandle);

		iLine++;

		if(iLine == Line)
		{
			int j=0;

			for(int i=0; i<strlen(buf)+1; i++)
			{
				if(buf[i]>='0' && buf[i]<='9')
				{
					strNum[j] = buf[i];
					j++;
				}
			}
			if (j != 0)
			{
				lRecordNum = atoi(strNum);
			}
		}

	}
	fclose(fHandle);
	return lRecordNum;
}

//��һ���ַ����в����ض����ַ������ҵ����� 1 �Ҳ������� 0
int CDataThread::FindLine(char *des, char *search)
{
	int idesLen = strlen(des),isearchLen = strlen(search);
	if (search ==NULL)
	{
		return 0;
	}
	
	int soupos = 0, searchpos = 0;

	while(soupos<idesLen && searchpos<isearchLen)
	{
		if(des[soupos] == search[searchpos])
		{
			++soupos;
			++searchpos;
		}
		else
		{
			soupos = soupos - searchpos + 1;
			searchpos = 0;
		}
	}
	if (searchpos == isearchLen)
	{
		return 1;
	}
	else
	{
		return 0;
	}
}

//�ҵ��ı��к����ض��ַ������У���ȡ�������е�����
int CDataThread::GetInsertNum(char * strchar)
{
	FILE   *fHandle;  
	char buf[4096], filePath[256], strPath[255];
	int iLine = 0;
	long lRecordNum = -1;
	char strNum[20];

	int pos = m_strCtlName.find('.');
	string strname = m_strCtlName.substr(0, pos);

	GetCurrentDirectory(256, filePath);
	sprintf(strPath, "%s\\LoadLog\\%s\\%s.log", filePath, m_strGameName.c_str(), strname.c_str());

	fHandle = fopen(strPath,"r");

	if(fHandle == NULL)
	{    
		return 0;  
	}

	while(!feof(fHandle))  
	{  
		fgets(buf,sizeof(buf),fHandle);

		iLine = FindLine(buf, strchar);	

		if(iLine == 1)
		{
			int j=0;

			for(int i=0; i<strlen(buf)+1; i++)
			{
				if(buf[i]>='0' && buf[i]<='9')
				{
					strNum[j] = buf[i];
					j++;
				}
			}

			if (j != 0)
			{
				lRecordNum = atoi(strNum);
			}

		}

	}
	fclose(fHandle);
	return lRecordNum;	
}

void CDataThread::WriteDBlog(int iResult, char *plog)
{
	char strDate[40];
	SYSTEMTIME sys;

	GetLocalTime(&sys);
	sprintf(strDate, "%4d/%02d/%02d %02d:%02d:%02d", sys.wYear, sys.wMonth, sys.wDay,sys.wHour,sys.wMinute,sys.wSecond);

	CAdoHelper myado;

	char strConn[255];
	char path[256];
	char filepath[256];

	GetCurrentDirectory(256,path);//��ȡ��ǰĿ¼
	sprintf(filepath,"%s%s",path,"\\config\\scheme.ini");//��ȡscheme.ini�ļ�·��

	char strLog[255];
	char tempLog[255];
	char sqlCommand[512];

	GetPrivateProfileString("MESSAGE", plog, "", strLog, 255, filepath);

	MakeLocalConnectString(strConn);

	if (iResult == 1)
	{
		sprintf(tempLog, strLog, m_strServerIp.c_str(), m_strGameName.c_str(), m_strServerName.c_str(), m_iRecordNum, m_iInsertNum);//m_strDBName.c_str()
		/*
		sprintf(sqlCommand, "insert into EXPRESS_LOG(id, LOG_DATE, LOG_GAME, LOG_SERVER, LOG_DB, LOG_TABLE, LOG_TYPE, LOG_SOURCE, LOG_RESULT, LOG_DETAIL) values(Err_ID.NEXTVAL,'%s', %d, '%s', '%s', '%s', %d, %d, %d, '%s')",
		//sprintf(sqlCommand, "{call pd_game_admin.pd_gamedbinfo_update('%s', %d, '%s', '%s', '%s', %d, %d, %d, '%s')}",
			strDate,
			m_iGameId,
			m_strServerIp.c_str(),
			m_strDBName.c_str(),
			"",
			iResult,
			m_iRecordNum,
			m_iInsertNum,
			tempLog
			);*/

	}
	else
	{
		sprintf(tempLog, strLog, m_strServerIp.c_str(), m_strGameName.c_str(), m_strServerName.c_str());//m_strDBName.c_str()
		/*
		sprintf(sqlCommand, "insert into EXPRESS_LOG(id, LOG_DATE, LOG_GAME, LOG_SERVER, LOG_DB, LOG_TABLE, LOG_TYPE, LOG_SOURCE, LOG_RESULT, LOG_DETAIL) values(Err_ID.NEXTVAL, '%s', %d, '%s', '%s', '%s', %d, %d, %d, '%s')",
		//sprintf(sqlCommand, "{call pd_game_admin.pd_gamedbinfo_update('%s', %d, '%s', '%s', '%s', %d, %d, %d, '%s')}",
			strDate,
			m_iGameId,
			m_strServerIp.c_str(),
			m_strDBName.c_str(),
			"",
			iResult,
			m_iRecordNum,
			m_iInsertNum,
			tempLog
			);*/

	}
		
	try
	{
 		if (!myado.connDB(strConn))
 		{
 			WriteCollectLog("���ݿ���־����ʧ��");
 			return;
 		}
		else
		{
			//if(!myado.execCommand(sqlCommand))
			//{
			//	WriteCollectLog("���ݿ���־����ʧ��");
			//}
			_CommandPtr pCommandPtr = NULL;

			pCommandPtr.CreateInstance(__uuidof(Command));
			pCommandPtr->CommandType=adCmdText;

			pCommandPtr->ActiveConnection = myado.m_pAdoConnect_;

			_variant_t   vResult; 
			_ParameterPtr pParam = NULL;

			pParam.CreateInstance(__uuidof(Parameter));

			pParam  =  pCommandPtr->CreateParameter(_bstr_t("v_logdate"),adDate,adParamInput,30,_variant_t(strDate));
			pCommandPtr->Parameters->Append(pParam);

			pParam = pCommandPtr->CreateParameter(_bstr_t("v_loggame"), adVarChar, adParamInput, 30, _variant_t(m_strGameName.c_str()));
			pCommandPtr->Parameters->Append(pParam);

			pParam = pCommandPtr->CreateParameter(_bstr_t("v_logserver"), adVarChar, adParamInput, 30, _variant_t(m_strServerIp.c_str()));
			pCommandPtr->Parameters->Append(pParam);

			pParam = pCommandPtr->CreateParameter(_bstr_t("v_logdb"), adVarChar, adParamInput, 30, _variant_t(m_strServerName.c_str()));//m_strDBName.c_str()
			pCommandPtr->Parameters->Append(pParam);

			pParam = pCommandPtr->CreateParameter(_bstr_t("v_log_table"), adVarChar, adParamInput, 30, _variant_t(m_strSrcTable.c_str()));
			pCommandPtr->Parameters->Append(pParam);

			pParam = pCommandPtr->CreateParameter(_bstr_t("v_log_express"), adNumeric, adParamInput, 30, _variant_t(m_iExpressId));
			pCommandPtr->Parameters->Append(pParam);

			pParam = pCommandPtr->CreateParameter(_bstr_t("v_logtype"), adNumeric, adParamInput,10, _variant_t(iResult));
			pCommandPtr->Parameters->Append(pParam);

			pParam = pCommandPtr->CreateParameter(_bstr_t("v_logsource"), adNumeric, adParamInput, 10, _variant_t(m_iRecordNum));
			pCommandPtr->Parameters->Append(pParam);

			pParam = pCommandPtr->CreateParameter(_bstr_t("v_logresult"), adNumeric, adParamInput, 10, _variant_t(m_iInsertNum)); 
			pCommandPtr->Parameters->Append(pParam);

			pParam = pCommandPtr->CreateParameter(_bstr_t("v_logdetail"), adVarChar, adParamInput, 255, _variant_t(tempLog));
			pCommandPtr->Parameters->Append(pParam);

			pParam = pCommandPtr->CreateParameter(_bstr_t("v_result"), adNumeric, adParamOutput, 10, vResult);
			pCommandPtr->Parameters->Append(pParam);

			pCommandPtr->CommandText = "{call pd_game_admin.pd_expresslog_insert(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)}";
			pCommandPtr->Execute(NULL,NULL,adCmdText);

			if (atoi(_bstr_t(vResult)))
			{
				WriteCollectLog("���ݿ���־��¼����ʧ��");
			}
		}
	}
	catch (_com_error &e)
	{
		WriteCollectLog((char *)e.Description());
	}
	catch(...)
	{
		WriteCollectLog("��¼���ݿ���־δ֪����");	
	}

	return;
}

void CDataThread::WriteCollectLog(char *strLog)
{
	char strCollectLog[512];

	//��־��ʽstrLog /��ϷID/��ϷIP/name/��Ϸ���ݿ���: ��־����
	sprintf(strCollectLog, "%s/%s/%s/%s/%s: %s", m_strGameName.c_str(), m_strServerIp.c_str(), m_strServerName.c_str(), m_strDBName.c_str(), m_strSrcTable.c_str(), strLog);

	m_clog.WriteLog(strCollectLog);
}

bool CDataThread::QueryTotalRecord()
{
	try
	{
		CAdoHelper AdoHelper;
		char tempConn[255];
		char strConn[255];
		char strQuery[255]; 

		//�������ݿ����ͣ���ȡ�����ַ���
		MakeDBConnectString(tempConn, m_iDbType);

//		sprintf(strConn, tempConn, m_strServerIp.c_str(),m_strUserId.c_str(),m_strPassWd.c_str(),m_strDBName.c_str());

		switch (m_iDbType)
		{
		case 1:
			sprintf(strConn, tempConn, m_strServerIp.c_str(),m_strUserId.c_str(),m_strPassWd.c_str(),m_strDBName.c_str());
			break;
		case 3:
			sprintf(strConn, tempConn, m_strServerIp.c_str(), m_iPort, m_strUserId.c_str(),m_strPassWd.c_str(),m_strDBName.c_str());
			break;
		}

		//�����������ݿ�����
		for(int i=0; i<3; i++)
		{
			if (AdoHelper.connDB(strConn))
			{
				break;
			}
			else
			{
				if(i == 2)
				{
					WriteCollectLog("������Ϸ���ݿ�ʧ��");
					return FALSE;				
				}	
			}
			Sleep(3000);
		}
		string strHtsql = GetTotalSql();

		sprintf(strQuery, "%s", strHtsql.c_str());
		_RecordsetPtr pRecordset = NULL;
		pRecordset = AdoHelper.execQuery(strQuery);
		if (pRecordset)
		{
			m_iRecordNum = atoi((char*)_bstr_t(pRecordset->GetCollect("Count")));
			printf("%s-%s-%s:GetRecord: %d\n",m_strServerName.c_str(), m_strDBName.c_str(), m_strSrcTable.c_str(), m_iRecordNum);
			return TRUE;
		}
		else
		{
			return FALSE;
		}

/*		pRecordset->Close();*/
		Sleep(1000);
	}
	catch (_com_error &e)
	{
		WriteCollectLog((char *)e.Description());
		return FALSE;
	}
	catch (...)
	{
		WriteCollectLog("δ֪�Ĵ���");
		return FALSE;
	}

}

string CDataThread::GetTotalSql()
{
	string strDes;
	string strtemp;
	strtemp = strstr((const char *)m_strBcpExpress.c_str(), "FROM");
	if(!strtemp.empty())
	{
		strDes = "select count(*) as COUNT " + strtemp;
	}
	return strDes;
}

//ִ��sqlload���빤��
int CDataThread::ExecSQLDR()
{
	char path[256];
	char ctlPath[256];
	char LoadLogPath[256];

	//ϵͳ������ý��
	int iCmdRet;
	char strCmd[1024];
	GetCurrentDirectory(256,path);//��ȡ��ǰĿ¼
	sprintf(ctlPath, "%s%s%s\\",path,"\\ctl\\", m_strGameName.c_str());
	sprintf(LoadLogPath, "%s%s%s\\",path,"\\LoadLog\\", m_strGameName.c_str());

	int pos = m_strCtlName.find('.');
	string strLogname = m_strCtlName.substr(0, pos);

	sprintf(strCmd, "sqlldr userid=%s/%s@%s control=%s\%s log=%s\%s.log", m_strColUser.c_str(), m_strColPwd.c_str(), m_strTns.c_str(), ctlPath, m_strCtlName.c_str(), LoadLogPath, strLogname.c_str());

	//����ϵͳ���ִ��sql loader�������ݿ�,ʧ�ܼ���־
	iCmdRet = system(strCmd);

	if (iCmdRet == 0)
	{
		//��ȡ��������,���а����ַ���"�� ���سɹ�"
		m_iInsertNum = GetInsertNum("�� ���سɹ�");

		//�Ƚ����ݿ�����������ʵ�ʲ��������Ƿ����, ���˵������������ȷ��
//		if (m_iInsertNum == m_iRecordNum) //(m_iInsertNum)&&
		if (m_iInsertNum > m_iRecordNum-300 && m_iInsertNum < m_iRecordNum+300) //(m_iInsertNum)&&
		{
			//���ɹ���־�������ݿ���־
			printf("%s-%s-%s\n", m_strServerName.c_str(), m_strDBName.c_str(), "Insert Date Finish");
			WriteDBlog(1,"SUCCESS");
			return 0;
		}
		else
		{
			//��ʧ����־�������ݿ���־
			WriteDBlog(2, "FAIL_SQLLDR");
			return -1;
		}
	}
	else
	{
		printf("SQL LOADER Excute Error\n");

		//��������־�������ݿ���־
		WriteDBlog(2, "FAIL_SQLLDR");
		return -1;

	}
}