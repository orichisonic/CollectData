#pragma once

class CLog
{
public:
	//���캯��
	CLog(void);
	
	//��������
	~CLog(void);
	
	//д�ļ�
    void WriteLog(const char * strText);
};
