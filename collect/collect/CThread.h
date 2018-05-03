/*thread.h
********************************************************************************
类名         : CThread
文件名       : thread.h thread.cpp
文件实现功能 : 提供线程创建、线程分配，线程安全和线程释放的的操作。
作者         : GMTools小组  彭根国
版本         : 1.1
备注         : 供信息采集系统线程控制使用
修改记录 :   
日 期 090918   版本1.1     修改人  彭根国            修改内容 创建类
*******************************************************************************/
#pragma once
#include <windows.h>
#include <list>
using namespace std;
#include <process.h>
typedef unsigned __stdcall threadFunc( void *pParam );//定义使用的函数
//枚举线程状态
enum Thread_State
{
Run=0,
Suspend=1,
Resume=2,
Terminate=3
};
//传出线程void*参数
struct Thread_Param
{
	string ConnectString;//连接字符串
	string SaveString;//输出目的字符串
	unsigned THREAD_ID;
};
class CThread
{
public:
	CThread();
	~CThread();
	bool MSuspendThread(HANDLE threadhandel);//挂起线程
	bool MResumeThread(HANDLE threadhandel);//重启线程
	void MTerminateThread(HANDLE threadhandel);//结束线程
	void ThreadLock(CRITICAL_SECTION* LockSection);//锁定线程内存区域
	void ThreadUnLock(CRITICAL_SECTION* LockSection);//解除锁定线程内存区域
	int  ClearMysqlThread(HANDLE threadhandle);//对Mysql特定线程进行清除
	bool LockProcessV(void* var);//对进程中的某一个全局变量进行锁定
	bool UnLockProcessV(void* var);//对进程中的某一个全局变量进行解锁
	static unsigned __stdcall threadFunc(void* pParam);//执行线程的函数	
	HANDLE m_handle;
	int thread_state;
	static int thread_count;
	
private:
	void ClearAll();//清除所有资源
	
};
class CMThread
{
public:
	
	CMThread();//构造函数
	~CMThread();//析构函数
	bool initialize();//初始化线程队列类
	//线程队列及线程队列操作方法
	list<CThread> Thread_List;//线程栈，其中存放Thread类
	HANDLE addthread(Thread_Param* param,unsigned Mode);//把线程放入队列中
	bool removethread(HANDLE threadhandle);//从队列中删除线程
	int getThreadCount();//返回线程的个数
	int getThreadState(HANDLE threadhandle);//获得某个线程的当前状态
    bool setThreadState(HANDLE threadhandle,int state);//对线程队列中某个线程设置状态
	bool removeAllThread();//清除所用线程
	void Lock();//采取CRITICAL_SECTION保护
	void UnLock();//对CRITICAL_SECTION解保护
	//处理器与线程操作方法
    bool SetThreadProcessor(HANDLE Threadhandle,int ProecessIndex);//把特定的线程赋予特定处理器
	void SetProcessHighRate();//设定定值
	double  ProcessHighRate;//一个定值。决定当处理器当前使用率大于此定值，则线程不赋予此处理器
	int  ProcessSumNum;//处理器的数量
	int  ThreadSumCount;//总的线程数量
	int  ProcessorThread;//单个处理器的线程数
private:	
	int GetProcessorNum();//获得处理器的数量
	int GetProcessUsing(int ProcessIndex);//获得特定处理器的当前使用率
	CThread m_thread;
	
};
