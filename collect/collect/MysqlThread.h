#pragma once
#include "datathread.h"

class CMysqlThread : public CDataThread
{
public:
	CMysqlThread(void);
public:
	~CMysqlThread(void);
protected:
	virtual DWORD run();
public:

	//��mysql���ݿ⣬дTXT�ı�
//	bool WriteRecord(int first, int second, int threadNo);
	bool WriteRecord();
//	static unsigned _stdcall threadFunc(LPVOID parma);
	CRITICAL_SECTION m_mysqlCritical;
};
