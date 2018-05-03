/************************************************************************/
/*																		*/
/*																		*/
/*		�򵥹���ģʽ���������ݿ��������ɲ�ͬ���ݿ����					*/
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
	//�����̶߳��� 1 sqlserver  2 oracle  3 mysql
	static CDataThread *CreateDBThread(int type);
};
