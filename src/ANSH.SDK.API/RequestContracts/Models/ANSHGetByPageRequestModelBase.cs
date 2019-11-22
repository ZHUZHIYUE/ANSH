using System;
namespace ANSH.SDK.API.RequestContracts.Models {
    /// <summary>
    /// Get请求参数基类
    /// </summary>
    public abstract class ANSHGetByPageRequestModelBase : ANSHGetRequestModelBase {

        /// <summary>
        /// 列表分页当前页
        /// </summary>
        public virtual string PageCur {
            get;
            set;
        } = "1";

        /// <summary>
        /// 列表分页每页显示条数
        /// </summary>
        public virtual string PageSize {
            get;
            set;
        } = "50";

        /// <summary>
        /// 每页显示条数上限
        /// </summary>
        protected virtual int PageSizeLimit {
            get;
            set;
        } = 200;

        /// <summary>
        /// 验证参数合法性
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <returns>验证通过返回True，验证失败返回False</returns>
        public override bool Validate (out string msg) {
            if (!base.Validate (out msg)) { return false; }

            if (!PageCur.IsInt (out int _page_cur) || _page_cur < 1) {
                msg = $"参数pageCur格式错误，应为大于等于1的整数";
                return false;
            }

            if (!PageSize.IsInt (out int _page_size) || _page_size < 1) {
                msg = $"参数pageSize格式错误，应为大于等于1的整数";
                return false;
            }

            if (_page_size > PageSizeLimit) {
                msg = $"参数pageSize错误，每页显示条数上限为{PageSizeLimit}";
                return false;
            }
            return true;
        }
    }
}