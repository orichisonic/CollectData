#ifndef _THREAD_H_
#define _THREAD_H_

#include <windows.h>

#define THREAD_WAITING	0
#define THREAD_RUNNING	1
#define THREAD_SUSPEND	2


class Thread
{
protected:
	HANDLE			m_thread;
	HANDLE			m_eventThreadStop;
	BYTE			m_threadMode;

	int				m_FinishFlag;

private:
	DWORD			start();
	void			terminate();
	void			suspend();
	void			resume();
	virtual DWORD	run() = 0;
	static unsigned __stdcall threadFunc( void *pParam );

public:
	friend class ThreadManager;
	Thread();
	virtual ~Thread();
};

#endif