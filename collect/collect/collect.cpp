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
	//Ӧ�ó���ֻ������һ��
	HANDLE hexeMutex;
	hexeMutex = CreateMutex(NULL, true, "collect.exe");
	if (GetLastError() == ERROR_ALREADY_EXISTS)
	{
		return 0;
	}

	//��ʼ��COM�ӿ�
	CoInitialize(NULL);

	//��ʼ�����̻߳���
	int rc = mysql_library_init(0, NULL, NULL);	

	printf("\t\t\t[��Ϸ���ݲɼ�������]\n\n");

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

			//�������ݿ���Ϣ������Ҫ���ļ����Լ�ctl�ļ�
			if (!CreateCTL())
			{
				printf("�����ļ�ʧ�ܣ���ȷ�����˳�������飡\n");
			}

			//�����Ѿ�����̶߳���
			g_threadManager->clearFinalthread();

			//��ȡ�����ַ���
			MakeLocalConnectString(strConn_info);

			if (!adoHelper.connDB(strConn_info))
			{
				printf("�������ݿ�ʧ�ܣ���鿴��־\n");
			}
			else
			{
				_RecordsetPtr pRset_game = NULL;

				//��ȡ��Ҫ�ɼ�����Ϸ�б�
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

			//�����̳߳�ʼ�������̶߳���
			g_threadManager->initialize();

			Sleep(120*1000);
			GetLocalTime(&CurrentTime);
			
			printf("�ȴ�������, ��ǰʱ�䣺%4d/%02d/%02d %02d:%02d:%02d ...\n", 
				CurrentTime.wYear,CurrentTime.wMonth,CurrentTime.wDay,CurrentTime.wHour,CurrentTime.wMinute,CurrentTime.wSecond);

		}


		/*
		while(TRUE)
		{
			Sleep(3600*1000);

			//�����Ѿ�����̶߳���
			g_threadManager->clearFinalthread();
			
			//�������ݿ⣬ȡ�µļ�¼
			char strConn_info[255];
			char strQuery[512];
			char strKey[30];

			CAdoHelper adoHelper;

			//�������ݿ���Ϣ������Ҫ���ļ����Լ�ctl�ļ�
			if (!CreateCTL())
			{
				printf("�����ļ�ʧ�ܣ���ȷ�����˳�������飡");
			}

			//��ȡ�����ַ���
			MakeLocalConnectString(strConn_info);

			if (!adoHelper.connDB(strConn_info))
			{
				printf("�������ݿ�ʧ�ܣ���鿴��־\n");
			}
			else
			{
				_RecordsetPtr pRset_game = NULL;

				//��ȡ��Ҫ�ɼ�����Ϸ�б�
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

			//�����̳߳�ʼ�������̶߳���
			g_threadManager->initialize();*/

		mysql_library_end();		

		//�ر�COM�ӿ�
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