#pragma once

class CLog
{
public:
	//构造函数
	CLog(void);
	
	//析构函数
	~CLog(void);
	
	//写文件
    void WriteLog(const char * strText);
};
