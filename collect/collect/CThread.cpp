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
#include "stdafx.h"
#include "CThread.h"
#include <iostream>
using namespace std;
//////////////////////////////////////////////////////
CRITICAL_SECTION protect;//保护公共全局变量
CMThread::CMThread()
{
  ProcessSumNum=0;
  ProcessHighRate=0.8;
  ProcessorThread=0;
  InitializeCriticalSection(&protect);
}
CMThread::~CMThread()
{
}
//把线程放入队列中
HANDLE CMThread::addthread(Thread_Param* param,unsigned Mode)
{
	HANDLE threadhandle;
	threadhandle=(HANDLE)_beginthreadex(NULL,0,&(CThread::threadFunc),(void*)param,Mode,&(param->THREAD_ID));
	//Thread_List.insert(Thread_List.push_back(),m_thread);
	m_thread.m_handle=threadhandle;
	Thread_List.push_back(m_thread);
	EnterCriticalSection(&protect);
	CThread::thread_count++;
	LeaveCriticalSection(&protect);
	return threadhandle;
}
//把特定的线程赋予特定处理器
bool CMThread::SetThreadProcessor(HANDLE Threadhandle,int ProecessIndex)
{
	if(ProecessIndex>ProcessSumNum)
	{
		return false;
	}
	if(SetThreadAffinityMask(Threadhandle,ProecessIndex)==0)
	{
		return false;
	}
	else
	{
		return true;
	}
}
//返回线程的个数
int CMThread::getThreadCount()
{
	return CThread::thread_count;
	//return Thread_List.size();
}
//从队列中删除线程 被动停止方式
bool CMThread::removethread(HANDLE threadhandle)
{
	CThread tempthread;
	for(list<CThread>::iterator i=Thread_List.begin();i!=Thread_List.end();i++)
	{
		tempthread=(*i);
		if(tempthread.m_handle==threadhandle)
		{
			tempthread.MTerminateThread(threadhandle);
			(i)->thread_state=Thread_State::Terminate;
			Thread_List.erase(i);			
			EnterCriticalSection(&protect);
			CThread::thread_count--;
			LeaveCriticalSection(&protect);
			return true;
		}
	}
	return false;
}
//采取CRITICAL_SECTION保护
void CMThread::Lock()
{
EnterCriticalSection(&protect);
}
//对CRITICAL_SECTION解保护
void CMThread::UnLock()
{
	LeaveCriticalSection(&protect);
}
//获得线程状态
int CMThread::getThreadState(HANDLE threadhandle)
{
	CThread tempthread;
	for(list<CThread>::iterator i=Thread_List.begin();i!=Thread_List.end();i++)
	{
		tempthread=(*i);
		if(tempthread.m_handle==threadhandle)
		{
			return tempthread.thread_state;
		}
	}
	return -1;
}
//设置线程重启或者挂起状态
bool CMThread::setThreadState(HANDLE threadhandle,int state)
{
	CThread set_thread;
	for(list<CThread>::iterator i=Thread_List.begin();i!=Thread_List.end();i++)
	{
		set_thread=(*i);
		if(set_thread.m_handle==threadhandle)
		{
			if(state==Thread_State::Resume)
			{
				ResumeThread(threadhandle);
				(i)->thread_state=Thread_State::Resume;
				return true;
			}
			else if(state==Thread_State::Suspend)
			{
				SuspendThread(threadhandle);
				(i)->thread_state=Thread_State::Suspend;
				return true;
			}

		}
		
	}
	return false;
}
//////////////////////////////////////////////////////
//////////////////////////////////////////////////////
//////////////////////////////////////////////////////
CThread::CThread()
{
}
CThread::~CThread()
{
	ClearAll();
}
void CThread::ClearAll()
{

}
//挂起线程
bool CThread::MSuspendThread(HANDLE threadhandel)
{
	if(SuspendThread(threadhandel)==-1)
	{
      return false;
	}
	else
	{
		return true;
	}
}
//重启线程
bool CThread::MResumeThread(HANDLE threadhandel)
{
 if(ResumeThread(threadhandel)==-1)
 {
	 return false;
 }
 else
 {
	 return true;
 }
}
//结束线程
void CThread::MTerminateThread(HANDLE threadhandel)
{
	//_endthreadex();
	//CloseHandle(threadhandel);
	/**********************************/
	if(TerminateThread(threadhandel,0))
	{
		Sleep(10);
		CloseHandle(threadhandel);
		//return true;
	}
	else
	{
		//return false;
	}
	/***********************************/
}
//锁定线程内存区域
void CThread::ThreadLock(CRITICAL_SECTION* LockSection)
{
	 EnterCriticalSection(LockSection);
}
//解除锁定线程内存区域
void CThread::ThreadUnLock(CRITICAL_SECTION* LockSection)
{
    LeaveCriticalSection(LockSection);
}
unsigned __stdcall CThread::threadFunc(void* pParam)
{
    
	Thread_Param* m_param=(Thread_Param*)(pParam);
	string one=m_param->ConnectString;
	while (true)
	{
		static int i=0;
	printf("%s\n",one.c_str());
		//cout<<m_param->ConnectString;
	Sleep(500);
	i++;
	printf("%d\n",i);
	if(i>10)
	{
    break;
	}
	}
	//printf("%d\n",m_param->THREAD_ID);
	EnterCriticalSection(&protect);
	thread_count--;
	LeaveCriticalSection(&protect);
	_endthreadex(0);//在此过程中程序退出

	
return 0;
}
int CThread::thread_count=0;
//CRITICAL_SECTION CThread::protect=NULL;







