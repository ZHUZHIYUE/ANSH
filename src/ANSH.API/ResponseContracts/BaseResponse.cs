using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ANSH.API.ResponseContracts
{
    /// <summary>
    /// 响应
    /// </summary>
    public abstract class BaseResponse
    {
        int _error = 0;
        /// <summary>
        /// 返回码
        /// </summary>
        public int error
        {
            get { return _error; }
            set { _error = value; }
        }


        string _msg = "SUCCESS";
        /// <summary>
        /// 错误消息
        /// </summary>
        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }


        string _result = "SUCCESS";
        /// <summary>
        /// 返回信息
        /// </summary>
        /// <returns></returns>
        public string result
        {
            get { return _result; }
            set { _result = value; }
        }
    }
}
