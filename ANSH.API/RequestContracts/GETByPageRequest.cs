using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANSH.API.ResponseContracts;

namespace ANSH.API.RequestContracts {
    /// <summary>
    /// 请求
    /// <para>分页</para>
    /// </summary>
    /// <typeparam name="TResponse">响应</typeparam>
    public abstract class GETByPageRequest<TResponse> : GETRequest<TResponse>
        where TResponse : GETByPageResponse {
            /// <summary>
            /// 列表分页当前页
            /// </summary>
            public virtual int page {
                get;
                set;
            } = 1;

            /// <summary>
            /// 列表分页每页显示条数
            /// </summary>
            public virtual int page_size {
                get;
                set;
            } = 15;

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
                ["page"] = page.ToString (),
                ["page_size"] = page_size.ToString ()
            };
        }
}