#include "stdafx.h"
#include "DBFactory.h"

CDBFactory::CDBFactory(void)
{
}

CDBFactory::~CDBFactory(void)
{
}

CDataThread *CDBFactory::CreateDBThread(int type)
{
	CDataThread *dateThread = NULL;
	switch(type)
	{
	case 1:	//sqlserver
		dateThread = new CMssqlThread();
		return dateThread;
	case 2:	//oracle
		return dateThread;
	case 3:	//mysql
		dateThread = new CMysqlThread();
		return dateThread;
	default:
		exit(0);
	}
};
