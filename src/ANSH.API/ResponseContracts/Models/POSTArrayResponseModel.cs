using System;
namespace ANSH.API.ResponseContracts.Models {
    /// <summary>
    /// 数组对象
    /// </summary>
    public abstract class POSTArrayResponseModel {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Item_MsgCode {
            get;
            set;
        } = 0;

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Item_Msg {
            get;
            set;
        } = "SUCCESS";
    }
}