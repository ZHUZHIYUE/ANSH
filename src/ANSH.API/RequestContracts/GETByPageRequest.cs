using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSH.API.ResponseContracts;
using ANSH.API.ResponseContracts.Models;

namespace ANSH.API.RequestContracts {
    /// <summary>
    /// 请求
    /// <para>分页</para>
    /// </summary>
    /// <typeparam name="TResponse">响应</typeparam>
    /// <typeparam name="TModelResponse">响应模型</typeparam>
    public abstract class GETByPageRequest<TResponse, TModelResponse> : GETRequest<TResponse>
        where TResponse : GETByPageResponse<TModelResponse>
        where TModelResponse : GETArrayResponseModel {
            /// <summary>
            /// 列表分页当前页
            /// </summary>
            public virtual string page_cur {
                get;
                set;
            } = "1";

            /// <summary>
            /// 列表分页每页显示条数
            /// </summary>
            public virtual string page_size {
                get;
                set;
            } = "15";

            /// <summary>
            /// 每页显示条数上限
            /// </summary>
            protected virtual int page_size_limit {
                get;
                set;
            } = 100;

            /// <summary>
            /// 获取URL参数
            /// </summary>
            public override Dictionary<string, string> GetParameters () {
                var page_paramters = SetByPageParameters ();
                var set_paramters = SetParameters () ?? new Dictionary<string, string> ();
                foreach (var in_set_paramters in set_paramters) {
                    page_paramters.Add (in_set_paramters.Key, in_set_paramters.Value);
                }
                return page_paramters;
            }

            /// <summary>
            /// 准备URL参数
            /// </summary>
            Dictionary<string, string> SetByPageParameters () => new Dictionary<string, string> {
                ["page_cur"] = page_cur,
                ["page_size"] = page_size
            };

            /// <summary>
            /// 验证参数合法性
            /// </summary>
            /// <param name="msg">提示信息</param>
            /// <returns>验证通过返回True，验证失败返回False</returns>
            public override bool Validate (out string msg) {
                if (!base.Validate (out msg)) { return false; }

                if (!page_cur.IsInt (out int _page_cur) || _page_cur < 1) {
                    msg = $"参数page_cur格式错误，应为大于等于1的整数";
                    return false;
                }

                if (!page_size.IsInt (out int _page_size) || _page_size < 1) {
                    msg = $"参数page_size格式错误，应为大于等于1的整数";
                    return false;
                }

                if (_page_size > page_size_limit) {
                    msg = $"参数page_size错误，每页显示条数上限为{page_size_limit}";
                    return false;
                }
                return true;
            }
        }
}