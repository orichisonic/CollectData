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

	//在管理线程map中添加对象
	void addThread(std::string strKey, Thread *thread);

	//在管理线程map中移除对象
	bool removeThread(std::string strKey);
	
	//在管理线程map中查找对象
	Thread *ThreadManager::find( std::string strKey); 

	//清空map中的对象，并释放对象占用资源
	void clearMap();

	//清理已经完成的线程对象
	void clearFinalthread();

private:
	ThreadMap m_threadMap;
	FlagMap m_statusMap;
};

#define g_threadManager ThreadManager::getInstance()
#endif
