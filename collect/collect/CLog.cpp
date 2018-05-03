#include "stdafx.h"
#include "CLog.h"
#include <stdio.h>
#include <io.h>
#include <fcntl.h>
#include <Windows.h>
#include <fstream> 
#include <iostream>
using namespace std;

#pragma warning(disable:4996)
CLog::CLog(void)
{
}

CLog::~CLog(void)
{
}
//д�ļ�
void CLog::WriteLog( const char * strText)
{
	SYSTEMTIME nowtime;
	GetLocalTime(&nowtime);//��ȡϵͳ��ǰʱ��
	
	char path[256];
	char filepath[256];
	char filename[256];
	ZeroMemory(path,256);
	ZeroMemory(filepath,256);
	ZeroMemory(filename,256);
	GetCurrentDirectory(256,path);//��ȡ��ǰ����Ŀ¼

	sprintf(filepath,"%s%s",path,"\\Log\\Log_%4d%02d%02d.log");
	sprintf(filename,filepath,nowtime.wYear,nowtime.wMonth,nowtime.wDay);//��ȡ�ļ�·��
	
	FILE *hr;
	hr = fopen(filename,"a");
	if (hr == NULL)
	{
		printf("Open File Error");
		return;
	}

	fprintf(hr, "%4d-%02d-%02d %02d:%02d:%02d %s\n", nowtime.wYear, nowtime.wMonth, nowtime.wDay, nowtime.wHour, nowtime.wMinute, nowtime.wSecond, strText);
	fclose(hr);	
	
	/*
	ofstream out;
	out.open(filename,ios::app);
	if (!out) 
	{ 
		cout<<"Open File Error"<<endl;
		return;
	}
	
	//����־�ļ���д�뵱ǰ��ʱ��
	out<<nowtime.wYear<<"-"<<nowtime.wMonth<<"-"<<nowtime.wDay<<" "<<nowtime.wHour<<":"<<nowtime.wMinute<<":"<<nowtime.wMinute<<" ";
	out<<strText<<"\n";
	out.close();*/

}
