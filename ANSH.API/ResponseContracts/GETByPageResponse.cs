using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ANSH.API.ResponseContracts {
    /// <summary>
    /// 响应
    /// <para>分页</para>
    /// </summary>
    public abstract class GETByPageResponse : BaseResponse {

        /// <summary>
        /// 总页数
        /// </summary>
        /// <returns></returns>
        public PageModel Page {
            get;
            set;
        }

        /// <summary>
        /// 分页信息
        /// </summary>
        public abstract class PageModel {

            /// <summary>
            /// 总页数
            /// </summary>
            /// <returns></returns>
            public int? page_count {
                get;
                set;
            }

            /// <summary>
            /// 当前页
            /// </summary>
            /// <returns></returns>
            public int? page {
                get;
                set;
            }

            /// <summary>
            /// 是否还有下一页
            /// </summary>
            /// <returns></returns>
            public bool? has_next {
                get;
                set;
            }
        }
    }
}