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
#include "stdafx.h"
#include "CThread.h"
#include <iostream>
using namespace std;
//////////////////////////////////////////////////////
CRITICAL_SECTION protect;//��������ȫ�ֱ���
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
//���̷߳��������
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
//���ض����̸߳����ض�������
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
//�����̵߳ĸ���
int CMThread::getThreadCount()
{
	return CThread::thread_count;
	//return Thread_List.size();
}
//�Ӷ�����ɾ���߳� ����ֹͣ��ʽ
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
//��ȡCRITICAL_SECTION����
void CMThread::Lock()
{
EnterCriticalSection(&protect);
}
//��CRITICAL_SECTION�Ᵽ��
void CMThread::UnLock()
{
	LeaveCriticalSection(&protect);
}
//����߳�״̬
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
//�����߳��������߹���״̬
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
//�����߳�
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
//�����߳�
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
//�����߳�
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
//�����߳��ڴ�����
void CThread::ThreadLock(CRITICAL_SECTION* LockSection)
{
	 EnterCriticalSection(LockSection);
}
//��������߳��ڴ�����
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
	_endthreadex(0);//�ڴ˹����г����˳�

	
return 0;
}
int CThread::thread_count=0;
//CRITICAL_SECTION CThread::protect=NULL;







