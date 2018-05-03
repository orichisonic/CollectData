#pragma once
#include "thread.h"
#include <string>
#include "CLog.h"
using namespace std;

#define SUCCESSNUM 43

class CDataThread :public Thread
{

protected:
	CDataThread(void);
public:
	~CDataThread(void);
public:
//	virtual DWORD run();

	//�����ض����ַ�
	int GetInsertNum(const int Line);
	int GetInsertNum(char * strchar);
	int FindLine(char *des, char *search);

	//iResult��ʾ�Ƿ�ɹ��� 0 �ɹ���1 ʧ�� ���� plog ��־��ϸ˵��
	void WriteDBlog(int iResult, char *plog);
	
	//��ȡָ����Ϸ��bcp���
//	int GetExpress(char *szBcpExpress, int gameid);
	
	//��¼�ɼ��߳���־
	void WriteCollectLog(char *strLog);

	//ͳ��ָ�����������
	bool QueryTotalRecord();

	//��ȡͳ�Ƶĵ�sql���
	string GetTotalSql();

	//ִ��sqlload���빤��
	int ExecSQLDR();
public:
//	BOOL m_bCollect;
	int m_iExpressId; 
	string m_strServerIp;
	string m_strDBName;
	string m_strUserId;
	string m_strPassWd;
	string m_strServerName;
	string m_strSrcTable;
	string m_strDesTable;
	int m_iGameId;
	string m_strGameName;
	string m_strBcpName;
	string m_strBcpExpress;
	string m_strCtlName;
	CLog m_clog;
	string m_strTns;
	string m_strColUser;
	string m_strColPwd;
	
	int m_iDbType;
	int m_iRecordNum;
	int m_iInsertNum;
	int m_iPort;
};
