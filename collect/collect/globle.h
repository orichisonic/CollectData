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


//���ݱ������ݿ�������������ַ�����˵���������ļ���
void MakeLocalConnectString(char *strConnect);

//��ȡ��ͬ���ݵ����ݿ������ַ��� 1 sqlserver  2 oracle  3 mysql
void MakeDBConnectString(char *strConnect, int type);

bool CreateCTL();

//�����ļ���
bool CreateFolder(char *path, char *gameName);

//����ctl�ļ�
bool CreateCtlFile(string gameName, string destable, string bcpName, string ctlName, string writeMode, string dbRow, int dbtype, string path);

//�滻Դ�ַ��������е����ַ���
string &replace_all_distinct(string &str,const string &old_value, const string &new_value);

vector <string> SplitString(string strSrc);



