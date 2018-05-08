using System;
using System.Collections.Generic;
using System.Text;

namespace ANSH.API.ResponseContracts {
    /// <summary>
    /// 响应
    /// <para>数组POST</para>
    /// </summary>
    /// <typeparam name="TModelResponse">响应模型</typeparam>
    public abstract class POSTArrayResponse<TModelResponse> : BaseResponse where TModelResponse : POSTArrayResponse<TModelResponse>.POSTArrayModelResponse {

        /// <summary>
        /// 返回值信息
        /// </summary>
        public List<TModelResponse> array_list {
            get;
            set;
        }

        /// <summary>
        /// 数组对象
        /// </summary>
        public class POSTArrayModelResponse {
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
}