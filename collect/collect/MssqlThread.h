#pragma once
#include "DataThread.h"

class CMssqlThread:public CDataThread
{
public:
	CMssqlThread(void);
public:
	~CMssqlThread(void);
protected:
	virtual DWORD run();
};
