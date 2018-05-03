#include "stdafx.h"
#include "AdoHelper.h"
#include <string>

using namespace std;

#pragma warning(disable:4996)

CAdoHelper::CAdoHelper(void)
{
	//��ʼ��ADO���
	m_pAdoConnect_.CreateInstance(__uuidof(Connection));
	m_pAdoConnect_->ConnectionTimeout = 3600;

}

CAdoHelper::~CAdoHelper(void)
{
	if (m_pAdoConnect_->State)
	{
		m_pAdoConnect_->Close();
	}
}

//�������ݿ�
bool CAdoHelper::connDB(char *strDriver)
{
	try
	{
		//�������ݿ�
		m_pHr = m_pAdoConnect_->Open(strDriver, "", "", adModeUnknown);

		if (FAILED ( m_pHr ) )
		{
			 return FALSE;
		}
		else
		{
			return TRUE;
		}

	}
	catch(_com_error &e)
	{
		string strErrorMessage = "Connect Error <";
		strErrorMessage += static_cast<char *>(e.Description());
		strErrorMessage += ">";
		m_CLog.WriteLog(strErrorMessage.c_str());
//		printf(strErrorMessage);
		return FALSE;
	}
	catch(char *e)
	{
		m_CLog.WriteLog(e);
		printf(e);
		return FALSE;
	}
	catch(...)
	{
		m_CLog.WriteLog("Unknown Connect Error!");
//		printf("Unknown Connect Error!");
		return FALSE;
	}
}

//ִ�в�ѯ����ȡ�����	
_RecordsetPtr CAdoHelper::execQuery(const char *cstrQuery)
{
	try
	{
		//��ѯ���
		_variant_t vQuery(cstrQuery);

		//���Ӿ��
		_variant_t vDispatch((IDispatch*)m_pAdoConnect_);

		//�������ݼ�����ָ�룬ִ�в�ѯ
		_RecordsetPtr pRecordset_;
		pRecordset_.CreateInstance(__uuidof(Recordset));
//		pRecordset_->Open(vQuery, vDispatch, adOpenForwardOnly,adLockOptimistic,adCmdText);
		pRecordset_->Open(vQuery, vDispatch, adOpenStatic,adLockReadOnly,adCmdText);

//		m_pRecordset_.CreateInstance(__uuidof(Recordset));

//		m_pRecordset_->Open(vQuery, vDispatch, adOpenDynamic,adLockOptimistic,adCmdText);
//		m_pRecordset_->Open(vQuery, vDispatch, adOpenForwardOnly,adLockOptimistic,adCmdText);
//		m_pRecordset_->Open(cstrQuery,_variant_t((IDispatch *)m_pAdoConnect_,true),adOpenForwardOnly,adLockOptimistic,adCmdText);

		return pRecordset_;
	}
	catch(_com_error &e)
	{
		string strErrorMessage = "Query Error <";
		strErrorMessage += static_cast<char *>(e.Description());
		strErrorMessage += ">";
		m_CLog.WriteLog(strErrorMessage.c_str());
//		printf(strErrorMessage);
		return NULL;
	}
	catch(...)
	{
		m_CLog.WriteLog("Unknown Query Error!");
//		printf("Unknown Query Error!");
		return NULL;
	}
}

bool CAdoHelper::execCommand(const char *cstrCommand)
{
	try
	{
		//���Ӿ��
		_variant_t vDispatch((IDispatch*)m_pAdoConnect_);

		//�������ݼ�����ָ�룬ִ�в���
		_CommandPtr pCommad_ = NULL;
		pCommad_.CreateInstance(__uuidof(Command));
		pCommad_->put_ActiveConnection(vDispatch);
		pCommad_->CommandText = cstrCommand;
		
		pCommad_->Execute(NULL, NULL, adCmdText);

		pCommad_.Release();		
		return TRUE;
	}
	catch(_com_error &e)
	{
		string strErrorMessage = "Command Error <";
		strErrorMessage += static_cast<char *>(e.Description());
		strErrorMessage += ">";
		m_CLog.WriteLog(strErrorMessage.c_str());
//		printf(strErrorMessage);
		return FALSE;
	}
	catch(...)
	{
		m_CLog.WriteLog("Unknown Command Error!");
//		printf("Unknown Command Error!");
		return FALSE;
	}
}


