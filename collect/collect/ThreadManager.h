#ifndef _THREADMANAGER_H_
#define _THREADMANAGER_H_
#pragma warning( disable : 4786 )
#pragma warning( disable : 4127 )

#include "Singleton.h"
#include "Thread.h"
#include <string>
#include <map>
using namespace std;

#define  THREADSTOP   0
#define  THREADRUNING 1

typedef std::map<std::string, Thread *> ThreadMap;
typedef std::map<std::string, int> FlagMap;

class ThreadManager : public SingleTon<ThreadManager>
{
public:
	ThreadManager();
	virtual ~ThreadManager();

public:
	void initialize();
	void release();

	//�ڹ����߳�map����Ӷ���
	void addThread(std::string strKey, Thread *thread);

	//�ڹ����߳�map���Ƴ�����
	bool removeThread(std::string strKey);
	
	//�ڹ����߳�map�в��Ҷ���
	Thread *ThreadManager::find( std::string strKey); 

	//���map�еĶ��󣬲��ͷŶ���ռ����Դ
	void clearMap();

	//�����Ѿ���ɵ��̶߳���
	void clearFinalthread();

private:
	ThreadMap m_threadMap;
	FlagMap m_statusMap;
};

#define g_threadManager ThreadManager::getInstance()
#endif
