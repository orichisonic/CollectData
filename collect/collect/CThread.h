/*thread.h
********************************************************************************
����         : CThread
�ļ���       : thread.h thread.cpp
�ļ�ʵ�ֹ��� : �ṩ�̴߳������̷߳��䣬�̰߳�ȫ���߳��ͷŵĵĲ�����
����         : GMToolsС��  �����
�汾         : 1.1
��ע         : ����Ϣ�ɼ�ϵͳ�߳̿���ʹ��
�޸ļ�¼ :   
�� �� 090918   �汾1.1     �޸���  �����            �޸����� ������
*******************************************************************************/
#pragma once
#include <windows.h>
#include <list>
using namespace std;
#include <process.h>
typedef unsigned __stdcall threadFunc( void *pParam );//����ʹ�õĺ���
//ö���߳�״̬
enum Thread_State
{
Run=0,
Suspend=1,
Resume=2,
Terminate=3
};
//�����߳�void*����
struct Thread_Param
{
	string ConnectString;//�����ַ���
	string SaveString;//���Ŀ���ַ���
	unsigned THREAD_ID;
};
class CThread
{
public:
	CThread();
	~CThread();
	bool MSuspendThread(HANDLE threadhandel);//�����߳�
	bool MResumeThread(HANDLE threadhandel);//�����߳�
	void MTerminateThread(HANDLE threadhandel);//�����߳�
	void ThreadLock(CRITICAL_SECTION* LockSection);//�����߳��ڴ�����
	void ThreadUnLock(CRITICAL_SECTION* LockSection);//��������߳��ڴ�����
	int  ClearMysqlThread(HANDLE threadhandle);//��Mysql�ض��߳̽������
	bool LockProcessV(void* var);//�Խ����е�ĳһ��ȫ�ֱ�����������
	bool UnLockProcessV(void* var);//�Խ����е�ĳһ��ȫ�ֱ������н���
	static unsigned __stdcall threadFunc(void* pParam);//ִ���̵߳ĺ���	
	HANDLE m_handle;
	int thread_state;
	static int thread_count;
	
private:
	void ClearAll();//���������Դ
	
};
class CMThread
{
public:
	
	CMThread();//���캯��
	~CMThread();//��������
	bool initialize();//��ʼ���̶߳�����
	//�̶߳��м��̶߳��в�������
	list<CThread> Thread_List;//�߳�ջ�����д��Thread��
	HANDLE addthread(Thread_Param* param,unsigned Mode);//���̷߳��������
	bool removethread(HANDLE threadhandle);//�Ӷ�����ɾ���߳�
	int getThreadCount();//�����̵߳ĸ���
	int getThreadState(HANDLE threadhandle);//���ĳ���̵߳ĵ�ǰ״̬
    bool setThreadState(HANDLE threadhandle,int state);//���̶߳�����ĳ���߳�����״̬
	bool removeAllThread();//��������߳�
	void Lock();//��ȡCRITICAL_SECTION����
	void UnLock();//��CRITICAL_SECTION�Ᵽ��
	//���������̲߳�������
    bool SetThreadProcessor(HANDLE Threadhandle,int ProecessIndex);//���ض����̸߳����ض�������
	void SetProcessHighRate();//�趨��ֵ
	double  ProcessHighRate;//һ����ֵ����������������ǰʹ���ʴ��ڴ˶�ֵ�����̲߳�����˴�����
	int  ProcessSumNum;//������������
	int  ThreadSumCount;//�ܵ��߳�����
	int  ProcessorThread;//�������������߳���
private:	
	int GetProcessorNum();//��ô�����������
	int GetProcessUsing(int ProcessIndex);//����ض��������ĵ�ǰʹ����
	CThread m_thread;
	
};
