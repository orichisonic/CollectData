#pragma once
#include "CLog.h"

#import "C:\Program Files\Common Files\System\ADO\msado15.dll" no_namespace rename("EOF", "adoEOF")

class CAdoHelper
{
public:
	CAdoHelper(void);

	//连接数据库
	bool connDB(char *strDriver);

	//执行查询，获取结果集	 
//	bool execQuery(const char *cstrQuery);
	_RecordsetPtr execQuery(const char *cstrQuery);

	//执行操作，返回执行是否成功
	bool execCommand(const char *cstrCommand);

public:
	~CAdoHelper(void);
public:
//	_RecordsetPtr m_pRecordset_;

	//ADO Connection对象
	_ConnectionPtr m_pAdoConnect_;
	
	//ADO Command 对象
//	_CommandPtr m_pCommand;

private:

	//结果句柄
	HRESULT m_pHr;

	CLog m_CLog;
};
