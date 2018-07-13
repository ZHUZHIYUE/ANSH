using System;
namespace ANSH.API.RequestContracts.Models {
    /// <summary>
    /// 数组对象
    /// </summary>
    public abstract class POSTArrayRequestModel {

        /// <summary>
        /// 验证参数合法性
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <returns>验证通过返回True，验证失败返回False</returns>
        public virtual bool Validate (out string msg) {
            msg = "SUCCESS";
            return true;
        }
    }
}