#include "stdafx.h"
#include "MssqlThread.h"
#include "globle.h"

CMssqlThread::CMssqlThread(void)
{
}

CMssqlThread::~CMssqlThread(void)
{
}

DWORD CMssqlThread::run()
{	
	try
	{
		char path[256];
		char bcpPath[256];

		GetCurrentDirectory(256,path);//��ȡ��ǰĿ¼
		sprintf(bcpPath, "%s%s%s\\",path,"\\temp\\",m_strGameName.c_str());

		if (QueryTotalRecord())
		{

			//ϵͳ������ý��
			int iCmdRet;
			char strName[20];
			char strQuery[1024];
			char strCmd[1024];

			sprintf(strName, "%s%s", m_strServerName.c_str(), m_strDBName.c_str());
			sprintf(strQuery, m_strBcpExpress.c_str(), m_strServerIp.c_str(), m_strServerName.c_str()); //m_strDBName.c_str()

			sprintf(strCmd,	"bcp \"%s\" queryout %s%s -c -S\"%s\" -U\"%s\" -P\"%s\"",
				strQuery,
				bcpPath,
				m_strBcpName.c_str(),
				m_strServerIp.c_str(),
				m_strUserId.c_str(), 
				m_strPassWd.c_str());

			/************************************************************************/
			/*		    ִ��bcp����txt�ļ����ɹ��Ժ�sql loader�������ݿ�
						����ϵͳ���ִ��bcp�������ݿ�,�������Σ�ʧ�ܼ���־		*/
			/************************************************************************/

			for(int i=0; i<3; i++)
			{
				iCmdRet = system(strCmd);

				if (iCmdRet == 0)
				{
					printf("%s-%s-%s\n", m_strServerName.c_str(), m_strDBName.c_str(), "Data Copy Finish");
					break;
				}
				else
				{
					printf("Execute BCP Error\n");

					if(i == 2)
					{
						//��������־�������ݿ���־
						WriteDBlog(2, "FAIL_BCP");
						m_FinishFlag = 1;
						return -1;
					}
				}
			}

			//ִ��sqlload���빤��
			ExecSQLDR();

			printf("%s-%s-%s:�ɼ�����\n",m_strServerName.c_str(), m_strDBName.c_str(), m_strSrcTable.c_str());

			char textpath[255];
			sprintf(textpath, "%s\\%s", bcpPath, m_strBcpName.c_str());

			if(remove(textpath) == -1)
				printf("%s is not exists!", textpath);

		}
		else
		{
			printf("Query Error");
			WriteDBlog(2, "COLLECT_BCP");
			m_FinishFlag = 1;
			return -1;
		}

	}
	catch(...)
	{
		//��¼�߳����д�����־
		WriteCollectLog("Thread Run Unknown Error");
		m_FinishFlag = 1;
		return -1;
	}
	m_FinishFlag = 1;
	return 0;
}
