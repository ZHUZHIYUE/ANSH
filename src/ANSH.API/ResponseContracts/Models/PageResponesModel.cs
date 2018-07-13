using System;
namespace ANSH.API.ResponseContracts.Models {
    /// <summary>
    /// 分页信息
    /// </summary>
    public sealed class PageResponesModel {

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
        public int? page_cur {
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