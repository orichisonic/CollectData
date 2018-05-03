#include "stdafx.h"
#include "ThreadManager.h"


ThreadManager::ThreadManager()
{
}

ThreadManager::~ThreadManager()
{
}

//初始化
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

//在管理线程map中添加对象
void ThreadManager::addThread(std::string strKey, Thread *thread)
{
	if (thread == NULL)
		return;

	//按关键字查找，没有就添加
	if (find(strKey) == NULL)
	{
		m_threadMap.insert(ThreadMap::value_type(strKey, thread));
	}
	else
	{
		delete thread;
	}
}

//在管理线程map中移除对象
bool ThreadManager::removeThread(std::string strKey)
{
	//容器为空，返回错误
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

//删除map容器中所有元素
void ThreadManager::clearMap()
{
	//容器为空，返回
	if (m_threadMap.empty())
	{
		return;
	}

	Thread * pDataThread = NULL;

	ThreadMap::iterator iter;

	//遍历map容器，删除所有元素
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

//清理已经完成的线程对象
void ThreadManager::clearFinalthread()
{
	//容器为空，返回
	if (m_threadMap.empty())
	{
		return;
	}

	Thread * pDataThread = NULL;

	ThreadMap::iterator iter;

	//遍历map容器，删除所有元素
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
















