using System;
namespace ANSH.SDK.API.ResponseContracts.Models {
    /// <summary>
    /// 数组对象
    /// </summary>
    public abstract class ANSHPostArrayResponseModelBase {
        /// <summary>
        /// 返回码
        /// </summary>
        public virtual int ItemMsgCode {
            get;
            set;
        } = 0;

        /// <summary>
        /// 错误消息
        /// </summary>
        public virtual string ItemMsg {
            get;
            set;
        } = "SUCCESS";
    }
}