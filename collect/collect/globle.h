#pragma once

#include <stdio.h>
#include "CLog.h"
#include "AdoHelper.h"
#include <string>
#include <vector>

using namespace std;

#define		SQLSERVER		1
#define		ORACLE			2
//#define		MYSQL			3

#define		MAXTHREAD       3


//根据本地数据库类型组成连接字符串，说明在配置文件中
void MakeLocalConnectString(char *strConnect);

//获取不同数据的数据库连接字符串 1 sqlserver  2 oracle  3 mysql
void MakeDBConnectString(char *strConnect, int type);

bool CreateCTL();

//创建文件夹
bool CreateFolder(char *path, char *gameName);

//创建ctl文件
bool CreateCtlFile(string gameName, string destable, string bcpName, string ctlName, string writeMode, string dbRow, int dbtype, string path);

//替换源字符串中所有的子字符串
string &replace_all_distinct(string &str,const string &old_value, const string &new_value);

vector <string> SplitString(string strSrc);



