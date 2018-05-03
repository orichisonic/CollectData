/************************************************************************/
/*																		*/
/*																		*/
/*		简单工厂模式，根据数据库类型生成不同数据库对象					*/
/*																		*/
/************************************************************************/
#pragma once
#include "DataThread.h"
#include "MssqlThread.h"
#include "MysqlThread.h"

class CDataThread;

class CDBFactory
{
public:
	CDBFactory(void);
public:
	~CDBFactory(void);
public:
	//创建线程对象 1 sqlserver  2 oracle  3 mysql
	static CDataThread *CreateDBThread(int type);
};
