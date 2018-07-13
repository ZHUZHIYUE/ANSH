using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ANSH.API.ResponseContracts {
    /// <summary>
    /// 响应
    /// </summary>
    public abstract class BaseResponse {
        int _MsgCode = 0;
        /// <summary>
        /// 消息代码
        /// </summary>
        public int MsgCode {
            get { return _MsgCode; }
            set { _MsgCode = value; }
        }

        string _Msg = "SUCCESS";
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg {
            get { return _Msg; }
            set { _Msg = value; }
        }
    }
}