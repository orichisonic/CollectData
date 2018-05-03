using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace DataCollectMonitor
{
    public enum BetweennessValue
    {
        SUCESS,
        FAILURE
    }

    public class Betweenness
    {
        /// <summary>
        /// 窗体操作结果变量
        /// 用于控制是否要刷新datagrid控件的列表
        /// </summary>
        private BetweennessValue _result = BetweennessValue.FAILURE;

        /// <summary>
        /// 窗体临时值
        /// 用于传输给调用给窗体的父窗体
        /// </summary>
        private int _tempValue = 0;

        /// <summary>
        /// hashtable类临时变量
        /// </summary>
        private Hashtable _htTempValue = new Hashtable();


        public BetweennessValue RESULT
        {
            set
            {
                _result = value;
            }
            get
            {
                return _result;
            }
        }

        public int TEMPVALUE
        {
            set
            {
                _tempValue = value;
            }
            get
            {
                return _tempValue;
            }
        }

        public Hashtable HASHTABLE
        {
            set
            {
                _htTempValue = value;
            }
            get
            {
                return _htTempValue;
            }
        }
    }
}
