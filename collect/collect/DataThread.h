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

	//查找特定的字符
	int GetInsertNum(const int Line);
	int GetInsertNum(char * strchar);
	int FindLine(char *des, char *search);

	//iResult表示是否成功（ 0 成功、1 失败 ）， plog 日志详细说明
	void WriteDBlog(int iResult, char *plog);
	
	//获取指定游戏的bcp语句
//	int GetExpress(char *szBcpExpress, int gameid);
	
	//记录采集线程日志
	void WriteCollectLog(char *strLog);

	//统计指定表的数据量
	bool QueryTotalRecord();

	//获取统计的的sql语句
	string GetTotalSql();

	//执行sqlload导入工作
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
