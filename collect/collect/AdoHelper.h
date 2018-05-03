#pragma once
#include "CLog.h"

#import "C:\Program Files\Common Files\System\ADO\msado15.dll" no_namespace rename("EOF", "adoEOF")

class CAdoHelper
{
public:
	CAdoHelper(void);

	//�������ݿ�
	bool connDB(char *strDriver);

	//ִ�в�ѯ����ȡ�����	 
//	bool execQuery(const char *cstrQuery);
	_RecordsetPtr execQuery(const char *cstrQuery);

	//ִ�в���������ִ���Ƿ�ɹ�
	bool execCommand(const char *cstrCommand);

public:
	~CAdoHelper(void);
public:
//	_RecordsetPtr m_pRecordset_;

	//ADO Connection����
	_ConnectionPtr m_pAdoConnect_;
	
	//ADO Command ����
//	_CommandPtr m_pCommand;

private:

	//������
	HRESULT m_pHr;

	CLog m_CLog;
};
