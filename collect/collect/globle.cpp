#include "stdafx.h"
#include "globle.h"
#include <string>
#include <vector>

using namespace std;

void MakeLocalConnectString(char *strConnect) 
{
	char path[256];
	char filepath[256];

	int iSqltype;
	char strUserId[40];
	char strPassWd[40];
	char strAlias[40];

	GetCurrentDirectory(256,path);//获取当前目录
	sprintf(filepath,"%s%s",path,"\\config\\scheme.ini");//获取scheme.ini文件路径

//	iSqltype = GetPrivateProfileInt("LOCALSERVER","TYPE",1, filepath);
	iSqltype = 2;	//本地是oracle数据库
	GetPrivateProfileString("LOCALSERVER", "User", "", strUserId, 40, filepath);
	GetPrivateProfileString("LOCALSERVER", "Pwd", "", strPassWd, 40, filepath);
	GetPrivateProfileString("LOCALSERVER", "ALIAS", "", strAlias, 40, filepath);

	switch (iSqltype)
	{
	case 1:			//sql server
		sprintf(strConnect,"DSN=%s;UID=%s;PWD=%s",strAlias,strUserId,strPassWd);
		break;
	case 2:			//oracle
		sprintf(strConnect,"Provider=OraOLEDB.Oracle.1;Password=%s;User ID=%s;Data Source=%s;Persist Security Info=True;PLSQLRSet=1", strPassWd, strUserId, strAlias);
		break;
	}

}


void MakeDBConnectString(char *strConnect, int type)
{
	switch (type)
	{
	case 1:			//sql server
		strcpy(strConnect, "DRIVER={SQL Server};Server=%s;UID=%s;PWD=%s;database=%s");
		break;
	case 2:
		strcpy(strConnect, "Provider=OraOLEDB.Oracle.1;User ID=%s;Password=%s;Data Source=%s;Persist Security Info=True;PLSQLRSet=1");
		break;
	case 3:
		strcpy(strConnect,"Driver={MySQL ODBC 3.51 Driver};Server=%s;charset=GBK;Port=%d;User=%s;Password=%s;Database=%s;Option=3;");
//		strcpy(strConnect,"Driver={MySQL ODBC 5.1 Driver};Server=%s;character_set_results=GBK;Port=3306;User=%s; Password=%s;Database=%s;Option=3;");
		break;
	}

}

bool CreateCTL()
{
	char strConn[255];
	char strSql[255];
	CAdoHelper adoHelper;
	char path[256];
	char filepath[256];
	char fileName[256];

	long lDataSize = 0;
	int ictl;
	int iTypedb;
	string strGameName;
	string strDestable;
	string strBcpname;
	string strCltName;
	string strWritemode;
	string strDbrow;
	string strGamedbid;

	vector<string> BcpNameList, ctlNameList, gamedbidList;


	GetCurrentDirectory(256,path);//获取当前目录
	sprintf(filepath,"%s%s",path,"\\ctl");//获取scheme.ini文件路径

	MakeLocalConnectString(strConn);

	try
	{
		if (!adoHelper.connDB(strConn))
		{
			printf("连接数据库失败，请查看日志\n");
			return FALSE;
		}
		else
		{
			_RecordsetPtr pRset_ctl = NULL;


				//获取需要创建的ctl文件
//			sprintf(strSql, "select c.game_name, c.type_db, a.id, a.destable, b.* from gamedb_info a, import_express b, game_info c where a.id = b.gamedb_id and b.createflag = 0 and a.game_id = c.game_id");
			sprintf(strSql, "select * from import_express a, game_info b where a.game_id = b.game_id and a.createflag = 0 order by id");
//			sprintf(strSql, "{call pd_gameinfo_pack.pd_sqlexpress_queryall()}");

			pRset_ctl = adoHelper.execQuery(strSql);
			while(!pRset_ctl->adoEOF)
			{
//				lDataSize = pRset_ctl->GetFields()->GetItem("ctl_byte")->ActualSize;
				strGameName = (char *)_bstr_t(pRset_ctl->GetCollect("game_name"));
				ictl = atoi((char *)_bstr_t(pRset_ctl->GetCollect("id")));
				strDestable = (char *)_bstr_t(pRset_ctl->GetCollect("destable"));
				iTypedb = atoi((char *)_bstr_t(pRset_ctl->GetCollect("type_db")));
				strBcpname = (char *)_bstr_t(pRset_ctl->GetCollect("bcp_name"));
				strCltName = (char *)_bstr_t(pRset_ctl->GetCollect("ctlname"));
				strWritemode = (char *)_bstr_t(pRset_ctl->GetCollect("writemode"));
				strDbrow = (char *)_bstr_t(pRset_ctl->GetCollect("dbrow"));
				strGamedbid = (char *)_bstr_t(pRset_ctl->GetCollect("gamedb_id"));

				//创建新游戏文件夹
				if (!CreateFolder(path, (char *)strGameName.c_str()))
				{
					return FALSE;
				}

				BcpNameList	= SplitString(strBcpname);
				ctlNameList = SplitString(strCltName);
//				gamedbidList = SplitString(strGamedbid);

				vector<string>::iterator i,j,k;
				for(i=BcpNameList.begin(),j=ctlNameList.begin();i!=BcpNameList.end();i++,j++)
				{
					if (!CreateCtlFile(strGameName, strDestable, *i/*strBcpname*/, *j/*strCltName*/, strWritemode, strDbrow, iTypedb, path))
					{
						return FALSE;
					}					
				}
				BcpNameList.clear();
				ctlNameList.clear();

				sprintf(strSql, "update import_express set createflag = 1 where id = %d", ictl);
// 				int result = -1;
 
 //				sprintf(strSql, "{call pd_game_admin.PD_ImportExpressFlag_Update(%d, %d)}", ictl, 1);
				adoHelper.execCommand(strSql);

// 				_CommandPtr pCommandPtr;
// 
// 				pCommandPtr.CreateInstance(__uuidof(Command));
// 				pCommandPtr->CommandType=adCmdText;
// 
// 				pCommandPtr->ActiveConnection = adoHelper.m_pAdoConnect_; 
// 
// 				_variant_t   vResult; 
// 				_ParameterPtr pParam = NULL;
// 				pParam.CreateInstance(__uuidof(Parameter));
// 
// 				pParam  =  pCommandPtr->CreateParameter(_bstr_t("v_gameid"),adNumeric,adParamInput,10,_variant_t(ictl));
// 				pCommandPtr->Parameters->Append(pParam);
// 
// 				pParam = pCommandPtr->CreateParameter(_bstr_t("v_status"), adNumeric, adParamInput, 10, _variant_t(1));
// 				pCommandPtr->Parameters->Append(pParam);
// 
// 				pParam = pCommandPtr->CreateParameter(_bstr_t("v_result"), adNumeric, adParamOutput, 10, vResult);
// 				pCommandPtr->Parameters->Append(pParam);
// 
// 				pCommandPtr->CommandText = "{call pd_game_admin.PD_ImportExpressFlag_Update(?, ?, ?)}";
// 				pCommandPtr->Execute(NULL,NULL,adCmdText);
				

				strGameName.clear();
				strCltName.clear();
				ictl = -1;
				/*
				if (lDataSize)
				{			
				_variant_t varBLOB;
				varBLOB = pRset_ctl->GetFields()->GetItem("ctl_byte")->GetChunk(lDataSize);

				if(varBLOB.vt == (VT_ARRAY | VT_UI1))    
				{    
				char * m_pJPGBuffer;
				int m_nFileLen;

				if(m_pJPGBuffer = new char[lDataSize+1])           
				{       
				char *pBuf = NULL;    
				SafeArrayAccessData(varBLOB.parray,(void **)&pBuf);    
				memcpy(m_pJPGBuffer,pBuf,lDataSize);                   
				SafeArrayUnaccessData (varBLOB.parray);  
				m_nFileLen = lDataSize; 
				}   

				//读取二进制数据，生成本地文件
				FILE *hr;
				sprintf(fileName, "%s\\%s\\%s", filepath, strGameName.c_str(), strCltName.c_str());
				hr = fopen(fileName,"wb");
				if (hr == NULL)
				{
				printf("Open File Error");
				return 0;
				}

				fwrite(m_pJPGBuffer, m_nFileLen, 1, hr);
				fclose(hr);
				}

				sprintf(strSql, "update import_express set createflag = 1 where id = %d", ictl);
				adoHelper.execCommand(strSql);

				strGameName.clear();
				strCltName.clear();
				ictl = -1;
				}*/

				pRset_ctl->MoveNext();
			}

			adoHelper.m_pAdoConnect_->Close();
		}
		return TRUE;
	}
	catch (_com_error &e)
	{
		printf("ERROR:%s", (char *)e.Description());
		return FALSE;
	}
	catch (...)
	{
		printf("UNKNOWN ERROR");
		return FALSE;
	}
}

//创建每个游戏需要文件夹
bool CreateFolder(char *path, char *gameName)
{
	char FilePath[256];
	char strMsg[256];
	int iResult;
	CLog myLog;

	//创建ctl控制文件文件夹
	sprintf(FilePath, "%s\\%s\\%s",path, "ctl", gameName);
	if ((iResult = ::CreateDirectory(FilePath, NULL)) == 0)
	{
		if (GetLastError()!=ERROR_ALREADY_EXISTS)
		{
			LPVOID lpMsgBuf;
			FormatMessage( 
				FORMAT_MESSAGE_ALLOCATE_BUFFER | 
				FORMAT_MESSAGE_FROM_SYSTEM | 
				FORMAT_MESSAGE_IGNORE_INSERTS,
				NULL,
				GetLastError(),
				0, // Default language
				(LPTSTR) &lpMsgBuf,
				0,
				NULL 
				);
			sprintf(strMsg, "%s %s", FilePath, (LPCTSTR)lpMsgBuf);
			myLog.WriteLog(strMsg);
			LocalFree( lpMsgBuf );
			return FALSE;
		}
	}

	//创建ctl日志文件夹
	sprintf(FilePath, "%s\\%s\\%s",path, "LoadLog", gameName);
	if ((iResult = ::CreateDirectory(FilePath, NULL)) == 0)
	{
		if (GetLastError()!=ERROR_ALREADY_EXISTS)
		{
			LPVOID lpMsgBuf;
			FormatMessage( 
				FORMAT_MESSAGE_ALLOCATE_BUFFER | 
				FORMAT_MESSAGE_FROM_SYSTEM | 
				FORMAT_MESSAGE_IGNORE_INSERTS,
				NULL,
				GetLastError(),
				0, // Default language
				(LPTSTR) &lpMsgBuf,
				0,
				NULL 
				);
			sprintf(strMsg, "%s %s", FilePath, (LPCTSTR)lpMsgBuf);
			myLog.WriteLog(strMsg);
			LocalFree( lpMsgBuf );
			return FALSE;
		}
	}

	//创建导入文件TXT文件夹
	sprintf(FilePath, "%s\\%s\\%s",path, "temp", gameName);
	if ((iResult = ::CreateDirectory(FilePath, NULL)) == 0)
	{
		if (GetLastError()!=ERROR_ALREADY_EXISTS)
		{
			LPVOID lpMsgBuf;
			FormatMessage( 
				FORMAT_MESSAGE_ALLOCATE_BUFFER | 
				FORMAT_MESSAGE_FROM_SYSTEM | 
				FORMAT_MESSAGE_IGNORE_INSERTS,
				NULL,
				GetLastError(),
				0, // Default language
				(LPTSTR) &lpMsgBuf,
				0,
				NULL 
				);
			sprintf(strMsg, "%s %s", FilePath, (LPCTSTR)lpMsgBuf);
			myLog.WriteLog(strMsg);
			LocalFree( lpMsgBuf );
			return FALSE;
		}
	}
	return TRUE;
}

//创建ctl文件
bool CreateCtlFile(string gameName, string destable, string bcpName, string ctlName, string writeMode, string dbRow, int dbtype, string path)
{
	char fileName[256];
	char fiSign[] = ",";
	char seSign[] = " ";
	char strPlace[10];
	char *token;
	string strDbRow;
	vector <string> mystring;
	string strContent = "LOAD DATA\t\n";


/*
	if (dbtype ==3)
	{
		int itxtNum=0;
		string tempName;
		for(itxtNum; itxtNum<MAXTHREAD; itxtNum++)
		{
			sprintf(strPlace, "_%d.", itxtNum);
			tempName = "'" + path + "\\temp\\"+ gameName + "\\"+bcpName + "'";
			replace_all_distinct(tempName, ".", strPlace);
			strContent += "INFILE " + tempName;
			strContent += "\r\n";
		}
	}
	else*/

	{
		strContent += "INFILE '" +  path + "\\temp\\"+ gameName + "\\" + bcpName + "'";
		strContent += "\r\n";
	}

	strContent += writeMode + "\r\n";
	strContent += "INTO TABLE ";
	strContent += destable;
	strContent += "\r\nFIELDS TERMINATED by X'09'\r\nTRAILING NULLCOLS\r\n(\r\n";
	
	//解析dbrow字段
	token = strtok((char *)dbRow.c_str(), fiSign);

	while( token != NULL )
	{
		mystring.push_back(token);		
		token = strtok( NULL, fiSign );
	}

	vector<string>::iterator i,j;
	int num=1;
	int size = mystring.size();

	for(num=1,i=mystring.begin();i!=mystring.end();i++,num++)
	{
		string a = *i;
		int iNUm=1;
		token = strtok((char *)a.c_str(), seSign);
		while( token != NULL )
		{

			switch(iNUm)
			{
			case 1:
				printf("%s", token);
				strDbRow += token;
			case 2:
				if (!strcmp(token,"DATE")||!strcmp(token,"date"))
				{
					printf(" DATE \"YYYY-MM-DD HH24:MI:SS\"");
					strDbRow += " DATE \"YYYY-MM-DD HH24:MI:SS\"";
				}
				break;
			default:
				if (num==size)
				{
					printf("\n");
					strDbRow += "\r\n";
				}
				else
				{
					printf(",\r\n");
					strDbRow += ",\r\n";
				}

				break;
			}
			iNUm++;
			token = strtok( NULL, seSign );
		}
	}
	strContent += strDbRow;
	strContent += ")";

	FILE *hr;
	sprintf(fileName, "%s\\ctl\\%s\\%s", path.c_str(), gameName.c_str(), ctlName.c_str());
	hr = fopen(fileName,"wb");
	if (hr == NULL)
	{
		printf("Open File Error");
		return FALSE;
	}

	fwrite(strContent.c_str(), strContent.length(), 1, hr);
	fclose(hr);
	return TRUE;
}

//替换源字符串中所有的子字符串
string &replace_all_distinct(string &str,const string &old_value, const string &new_value)  
{  
	for(string::size_type   pos(0);   pos!=string::npos;   pos+=new_value.length())
	{  
		if( (pos=str.find(old_value,pos))!=string::npos)
		{
			str.replace(pos,old_value.length(),new_value); 
		}
 		else
		{
			break;
		}
	}  

	return  str;
} 

vector <string> SplitString(string strSrc)
{
// 	char strsplit[2048];
// 	sprintf(strsplit, "%s", strSrc.c_str());
	char fiSign[] = ",";
	char strPlace[10];
	char *token;

	vector<string> vetString;

	token = strtok((char *)strSrc.c_str(), fiSign);

	while( token != NULL )
	{
		vetString.push_back(token);		
		token = strtok( NULL, fiSign );
	}

	return vetString;
}