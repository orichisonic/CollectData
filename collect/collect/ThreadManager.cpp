#include "stdafx.h"
#include "ThreadManager.h"


ThreadManager::ThreadManager()
{
}

ThreadManager::~ThreadManager()
{
}

//��ʼ��
void ThreadManager::initialize()
{
	Thread *pDataThread = NULL;
	ThreadMap::iterator iter;
    
	for (iter=m_threadMap.begin(); iter!=m_threadMap.end(); iter++)
	{
		pDataThread = iter->second;
		if (pDataThread->m_FinishFlag == 0)
		{
			pDataThread->start();
			pDataThread->m_FinishFlag = 2;
		}

		Sleep(1000);
	}
}

//�ڹ����߳�map����Ӷ���
void ThreadManager::addThread(std::string strKey, Thread *thread)
{
	if (thread == NULL)
		return;

	//���ؼ��ֲ��ң�û�о����
	if (find(strKey) == NULL)
	{
		m_threadMap.insert(ThreadMap::value_type(strKey, thread));
	}
	else
	{
		delete thread;
	}
}

//�ڹ����߳�map���Ƴ�����
bool ThreadManager::removeThread(std::string strKey)
{
	//����Ϊ�գ����ش���
	if (m_threadMap.empty())
	{
		return FALSE;
	}

	Thread * RemoveThread = NULL;
	RemoveThread = find(strKey);	

	if(RemoveThread != NULL)
	{
		m_threadMap.erase(ThreadMap::key_type(strKey));
//		RemoveThread->terminate();
		delete RemoveThread;
		RemoveThread = NULL;

		return TRUE;
	}

	return FALSE;
}


Thread* ThreadManager::find( std::string strKey) 
{
	Thread * FindThread = NULL;

	ThreadMap::iterator itr;
	itr = m_threadMap.find(strKey);

	if (itr != m_threadMap.end())
	{		
		FindThread = itr->second;
	}

	return FindThread;
}

//ɾ��map����������Ԫ��
void ThreadManager::clearMap()
{
	//����Ϊ�գ�����
	if (m_threadMap.empty())
	{
		return;
	}

	Thread * pDataThread = NULL;

	ThreadMap::iterator iter;

	//����map������ɾ������Ԫ��
	for(iter = m_threadMap.begin(); iter != m_threadMap.end();iter++)
	{
		pDataThread = iter->second;
//		pDataThread->terminate();
		delete pDataThread;
		pDataThread = NULL;
	}
	m_threadMap.clear();

	return;
}

//�����Ѿ���ɵ��̶߳���
void ThreadManager::clearFinalthread()
{
	//����Ϊ�գ�����
	if (m_threadMap.empty())
	{
		return;
	}

	Thread * pDataThread = NULL;

	ThreadMap::iterator iter;

	//����map������ɾ������Ԫ��
	for(iter = m_threadMap.begin(); iter != m_threadMap.end();)
	{
		pDataThread = iter->second;
		if (pDataThread->m_FinishFlag == 1)
		{
			delete pDataThread;
			pDataThread = NULL;
			m_threadMap.erase(iter++);
		}
		else
		{
			++iter;
		}
	}

	return;	
}
















