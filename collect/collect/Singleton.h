#ifndef __SINGLETON_H__
#define __SINGLETON_H__

/******************************************************************************
Module:  SingleTon.h
Purpose: For create one instance of class in using project

NOTE: New _TClass() disable
******************************************************************************/

template < class _TClass > 
class SingleTon
{
private:
	static	_TClass						*m_selfInstance;

public:
	static	_TClass*	getInstance()
	{
		if( !m_selfInstance )
		{
			m_selfInstance = new _TClass();
		}

		return ( m_selfInstance );
	}

	static	void		releaseInstance()
	{
		if( m_selfInstance != 0 )
		{
			delete m_selfInstance;
			m_selfInstance = 0;
		}
	}
};

template < class _TClass > _TClass*	SingleTon< _TClass >::m_selfInstance = 0;

#endif
