using System;
namespace ANSH.SDK.API.ResponseContracts.Models {
    /// <summary>
    /// 分页信息
    /// </summary>
    public class ANSHPageResponesModelBase : IANSHPageResponesModelBase {

        /// <summary>
        /// 总页数
        /// </summary>
        /// <returns></returns>
        public virtual int? PageCount {
            get;
            set;
        }

        /// <summary>
        /// 当前页
        /// </summary>
        /// <returns></returns>
        public virtual int? PageCur {
            get;
            set;
        }

        /// <summary>
        /// 查询数据总条数
        /// </summary>
        public virtual int? DataCount {
            get;
            set;
        }

        /// <summary>
        /// 是否还有下一页
        /// </summary>
        /// <returns></returns>
        public virtual bool? HasNext {
            get;
            set;
        }
    }
}