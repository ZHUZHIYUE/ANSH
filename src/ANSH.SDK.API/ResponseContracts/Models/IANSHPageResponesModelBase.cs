using System;
namespace ANSH.SDK.API.ResponseContracts.Models {
    /// <summary>
    /// 分页信息
    /// </summary>
    public interface IANSHPageResponesModelBase {

        /// <summary>
        /// 总页数
        /// </summary>
        /// <returns></returns>
        int? PageCount {
            get;
            set;
        }

        /// <summary>
        /// 当前页
        /// </summary>
        /// <returns></returns>
        int? PageCur {
            get;
            set;
        }

        /// <summary>
        /// 查询数据总条数
        /// </summary>
        int? DataCount {
            get;
            set;
        }

        /// <summary>
        /// 是否还有下一页
        /// </summary>
        /// <returns></returns>
        bool? HasNext {
            get;
            set;
        }
    }
}