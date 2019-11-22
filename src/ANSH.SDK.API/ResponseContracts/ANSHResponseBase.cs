using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace ANSH.SDK.API.ResponseContracts {
    /// <summary>
    /// 响应基类
    /// </summary>
    public abstract class ANSHResponseBase {
        int _MsgCode = 0;
        /// <summary>
        /// 消息代码
        /// </summary>
        public virtual int MsgCode {
            get { return _MsgCode; }
            set { _MsgCode = value; }
        }

        string _Msg = "SUCCESS";
        /// <summary>
        /// 消息内容
        /// </summary>
        public virtual string Msg {
            get { return _Msg; }
            set { _Msg = value; }
        }
    }
}