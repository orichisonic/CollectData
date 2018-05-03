#include "stdafx.h"
#include "AdoHelper.h"
#include <string>

using namespace std;

#pragma warning(disable:4996)

CAdoHelper::CAdoHelper(void)
{
	//初始化ADO句柄
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

//连接数据库
bool CAdoHelper::connDB(char *strDriver)
{
	try
	{
		//连接数据库
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

//执行查询，获取结果集	
_RecordsetPtr CAdoHelper::execQuery(const char *cstrQuery)
{
	try
	{
		//查询语句
		_variant_t vQuery(cstrQuery);

		//连接句柄
		_variant_t vDispatch((IDispatch*)m_pAdoConnect_);

		//创建数据集智能指针，执行查询
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
		//连接句柄
		_variant_t vDispatch((IDispatch*)m_pAdoConnect_);

		//创建数据集智能指针，执行操作
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


