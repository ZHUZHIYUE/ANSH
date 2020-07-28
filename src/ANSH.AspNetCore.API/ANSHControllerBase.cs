using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ANSH.SDK.API.RequestContracts;
using ANSH.SDK.API.RequestContracts.Models;
using ANSH.SDK.API.ResponseContracts;
using ANSH.SDK.API.ResponseContracts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ANSH.AspNetCore.API {
    /// <summary>
    /// APIController基类
    /// </summary>
    [ApiController]
    [Route ("[controller]/[action]")]
    public class ANSHControllerBase : ControllerBase {

        /// <summary>
        /// 日志记录
        /// </summary>
        protected ILogger Logger {
            get;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Logger">日志记录</param>
        public ANSHControllerBase (ILogger Logger) {
            this.Logger = Logger;
        }

        /// <summary>
        /// 验证参数合法性
        /// </summary>
        /// <param name="request">请求数据</param>
        ///<typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TMODELRequest">请求模型</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        protected void Validate<TResponse, TMODELRequest, TModelResponse> (ANSHPostRequestBase<TResponse, TMODELRequest, TModelResponse> request)
        where TResponse : ANSHPostResponseBase<TModelResponse>, new ()
        where TMODELRequest : ANSHPostRequestModelBase, new ()
        where TModelResponse : class, new () {
            TResponse response = new TResponse ();
            if (request == null) {
                throw new ANSHExceptions (ANSHErrorCodes.无效的数据, "未能识别的JSON数据");
            } {
                if (!request.Validate (out string msg)) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, msg);
                }
            } {
                if (!request.PostItem.Validate (out string msg)) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, msg);
                }
            }
        }

        /// <summary>
        /// 验证参数合法性
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="success">通过验证的项</param>
        /// <param name="error">未通过验证的项</param>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TMODELRequest">请求模型</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        protected void Validate<TResponse, TMODELRequest, TModelResponse> (ANSHPostArrayRequestBase<TResponse, TMODELRequest, TModelResponse> request, Action<TMODELRequest> success, Action<TMODELRequest, ANSHExceptions> error)
        where TResponse : ANSHPostArrayResponseBase<TModelResponse>, new ()
        where TMODELRequest : ANSHPostRequestModelBase, new ()
        where TModelResponse : ANSHPostArrayResponseModelBase, new () {
            TResponse response = new TResponse (); {
                if (request == null) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, "未能识别的JSON数据");
                }
            } {
                if (!request.Validate (out string msg)) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, msg);
                }
            } {
                foreach (var postItem in request.PostList) {
                    var TMResponse = new TModelResponse ();
                    if (!postItem.Validate (out string msg)) {
                        var exceptions = new ANSHExceptions (ANSHErrorCodes.无效的数据, msg);
                        if (error == null) {
                            throw exceptions;
                        }
                        error.Invoke (postItem, exceptions);
                    }
                    success (postItem);
                }
            }
        }

        /// <summary>
        /// 验证参数合法性
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <typeparam name="TQueryRequest">查询参数</typeparam>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        protected void Validate<TQueryRequest, TResponse, TModelResponse> (ANSHGetRequestBase<TQueryRequest, TResponse, TModelResponse> request)
        where TResponse : ANSHGetResponseBase<TModelResponse>, new ()
        where TModelResponse : class
        where TQueryRequest : ANSHGetRequestModelBase, new () {
            TResponse response = new TResponse (); {
                if (request == null) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, "未能识别的参数");
                }
                if (!request.Validate (out string msg)) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, msg);
                }

            } {
                request.Query = request.Query ?? new TQueryRequest ();
                if (!request.Query.Validate (out string msg)) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, msg);
                }
            }
        }

        /// <summary>
        /// 验证参数合法性
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <typeparam name="TQueryRequest">查询参数</typeparam>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <typeparam name="TPageResponesModel">分页信息模型</typeparam>
        protected void Validate<TQueryRequest, TResponse, TModelResponse, TPageResponesModel> (ANSHGetByPageRequestBase<TQueryRequest, TResponse, TModelResponse, TPageResponesModel> request)
        where TResponse : ANSHGetByPageResponseBase<TModelResponse, TPageResponesModel>, new ()
        where TModelResponse : class
        where TPageResponesModel : IANSHPageResponesModelBase, new ()
        where TQueryRequest : ANSHGetByPageRequestModelBase, new () {
            TResponse response = new TResponse (); {
                if (request == null) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, "未能识别的参数");
                }
                if (!request.Validate (out string msg)) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, msg);
                }

            } {
                request.Query = request.Query ?? new TQueryRequest ();
                if (!request.Query.Validate (out string msg)) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, msg);
                }
            }
        }

        /// <summary>
        /// 验证参数合法性
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <typeparam name="TQueryRequest">查询参数</typeparam>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        protected void Validate<TQueryRequest, TResponse, TModelResponse> (ANSHGetListRequestBase<TQueryRequest, TResponse, TModelResponse> request)
        where TResponse : ANSHGetListResponseBase<TModelResponse>, new ()
        where TModelResponse : class
        where TQueryRequest : ANSHGetRequestModelBase, new () {
            TResponse response = new TResponse (); {
                if (request == null) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, "未能识别的参数");
                }
                if (!request.Validate (out string msg)) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, msg);
                }

            } {
                request.Query = request.Query ?? new TQueryRequest ();
                if (!request.Query.Validate (out string msg)) {
                    throw new ANSHExceptions (ANSHErrorCodes.无效的数据, msg);
                }
            }
        }

        /// <summary>
        /// GET项处理
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        /// <typeparam name="TQueryRequest">查询参数</typeparam>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        protected TResponse Get<TQueryRequest, TResponse, TModelResponse> (ANSHGetRequestBase<TQueryRequest, TResponse, TModelResponse> request, Func<TQueryRequest, TModelResponse> func)
        where TResponse : ANSHGetResponseBase<TModelResponse>, new ()
        where TModelResponse : class
        where TQueryRequest : ANSHGetRequestModelBase, new () => GetAsync (request, async (parameter) => {
            await Task.CompletedTask;
            return func (parameter);
        }).Result;

        /// <summary>
        /// GET项处理
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        /// <typeparam name="TQueryRequest">查询参数</typeparam>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        protected async Task<TResponse> GetAsync<TQueryRequest, TResponse, TModelResponse> (ANSHGetRequestBase<TQueryRequest, TResponse, TModelResponse> request, Func<TQueryRequest, Task<TModelResponse>> func)
        where TResponse : ANSHGetResponseBase<TModelResponse>, new ()
        where TModelResponse : class
        where TQueryRequest : ANSHGetRequestModelBase, new () => await ExecuteAsync (async () => {
            TResponse response = new TResponse ();
            Validate (request);
            response.ResultItem = await func?.Invoke (request.Query);
            return response;
        });

        /// <summary>
        /// GET项处理
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        /// <typeparam name="TQueryRequest">查询参数</typeparam>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        protected TResponse Get<TQueryRequest, TResponse, TModelResponse> (ANSHGetListRequestBase<TQueryRequest, TResponse, TModelResponse> request, Func<TQueryRequest, List<TModelResponse>> func)
        where TResponse : ANSHGetListResponseBase<TModelResponse>, new ()
        where TModelResponse : class
        where TQueryRequest : ANSHGetRequestModelBase, new () => GetAsync (request, async (parameter) => {
            await Task.CompletedTask;
            return func (parameter);
        }).Result;

        /// <summary>
        /// GET项处理
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        /// <typeparam name="TQueryRequest">查询参数</typeparam>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        protected async Task<TResponse> GetAsync<TQueryRequest, TResponse, TModelResponse> (ANSHGetListRequestBase<TQueryRequest, TResponse, TModelResponse> request, Func<TQueryRequest, Task<List<TModelResponse>>> func)
        where TResponse : ANSHGetListResponseBase<TModelResponse>, new ()
        where TModelResponse : class
        where TQueryRequest : ANSHGetRequestModelBase, new () => await ExecuteAsync (async () => {
            TResponse response = new TResponse ();
            Validate (request);
            response.ResultList = await func?.Invoke (request.Query);
            return response;
        });

        /// <summary>
        /// GET项处理
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        /// <typeparam name="TQueryRequest">查询参数</typeparam>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <typeparam name="TPageResponesModel">分页信息模型</typeparam>
        /// <returns>响应</returns>
        protected TResponse Get<TQueryRequest, TResponse, TModelResponse, TPageResponesModel> (ANSHGetByPageRequestBase<TQueryRequest, TResponse, TModelResponse, TPageResponesModel> request, Func < TQueryRequest, (List<TModelResponse>, TPageResponesModel) > func)
        where TResponse : ANSHGetByPageResponseBase<TModelResponse, TPageResponesModel>, new ()
        where TModelResponse : class
        where TPageResponesModel : IANSHPageResponesModelBase, new ()
        where TQueryRequest : ANSHGetByPageRequestModelBase, new () => GetAsync (request, async (parameter) => {
            await Task.CompletedTask;
            return func (parameter);
        }).Result;

        /// <summary>
        /// GET项处理
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        /// <typeparam name="TQueryRequest">查询参数</typeparam>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <typeparam name="TPageResponesModel">分页信息模型</typeparam>
        /// <returns>响应</returns>
        protected async Task<TResponse> GetAsync<TQueryRequest, TResponse, TModelResponse, TPageResponesModel> (ANSHGetByPageRequestBase<TQueryRequest, TResponse, TModelResponse, TPageResponesModel> request, Func < TQueryRequest, Task < (List<TModelResponse>, TPageResponesModel) >> func)
        where TResponse : ANSHGetByPageResponseBase<TModelResponse, TPageResponesModel>, new ()
        where TModelResponse : class
        where TPageResponesModel : IANSHPageResponesModelBase, new ()
        where TQueryRequest : ANSHGetByPageRequestModelBase, new () => await ExecuteAsync (async () => {
            Validate (request);
            var result = await func?.Invoke (request.Query);
            TResponse response = new TResponse ();
            response.ResultList = result.Item1;
            response.PageInfo = result.Item2;
            return response;
        });

        /// <summary>
        /// POST项处理
        /// <para>数组POST</para>
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        /// <param name="callback">对返回集合进行处理</param>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TMODELRequest">请求模型</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        protected TResponse Post<TResponse, TMODELRequest, TModelResponse> (ANSHPostArrayRequestBase<TResponse, TMODELRequest, TModelResponse> request, Func<TMODELRequest, TModelResponse> func, Action<TMODELRequest, TModelResponse> callback = null)
        where TResponse : ANSHPostArrayResponseBase<TModelResponse>, new ()
        where TMODELRequest : ANSHPostRequestModelBase, new ()
        where TModelResponse : ANSHPostArrayResponseModelBase, new () => PostAsync (request, async (parameter) => {
            await Task.CompletedTask;
            return func (parameter);
        }, callback).Result;

        /// <summary>
        /// POST项处理
        /// <para>数组POST</para>
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        /// <param name="callback">对返回集合进行处理</param>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TMODELRequest">请求模型</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        protected async Task<TResponse> PostAsync<TResponse, TMODELRequest, TModelResponse> (ANSHPostArrayRequestBase<TResponse, TMODELRequest, TModelResponse> request, Func<TMODELRequest, Task<TModelResponse>> func, Action<TMODELRequest, TModelResponse> callback = null)
        where TResponse : ANSHPostArrayResponseBase<TModelResponse>, new ()
        where TMODELRequest : ANSHPostRequestModelBase, new ()
        where TModelResponse : ANSHPostArrayResponseModelBase, new () =>
            await ExecuteAsync (async () => {
                await Task.CompletedTask;
                TResponse response = new TResponse ();
                Validate (request, async in_request => {
                    var TMResponse = await PostAsync (in_request, func);
                }, (in_request, exception) => {
                    var TMResponse = new TModelResponse ();
                    TMResponse.ItemMsgCode = exception.ERRORCODE;
                    TMResponse.ItemMsg = exception.ERRORMSG;
                });
                return response;
            });

        /// <summary>
        /// POST项处理
        /// <para>数组POST</para>
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        /// <param name="callback">对返回集合进行处理</param>
        /// <typeparam name="TMODELRequest">请求模型</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        TModelResponse Post<TModelResponse, TMODELRequest> (TMODELRequest request, Func<TMODELRequest, TModelResponse> func, Action<TMODELRequest, TModelResponse> callback = null)
        where TMODELRequest : ANSHPostRequestModelBase, new ()
        where TModelResponse : ANSHPostArrayResponseModelBase, new () => PostAsync (request, async (parameter) => {
            await Task.CompletedTask;
            return func (parameter);
        }, callback).Result;

        /// <summary>
        /// POST项处理
        /// <para>数组POST</para>
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        /// <param name="callback">对返回集合进行处理</param>
        /// <typeparam name="TMODELRequest">请求模型</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        async Task<TModelResponse> PostAsync<TModelResponse, TMODELRequest> (TMODELRequest request, Func<TMODELRequest, Task<TModelResponse>> func, Action<TMODELRequest, TModelResponse> callback = null)
        where TMODELRequest : ANSHPostRequestModelBase, new ()
        where TModelResponse : ANSHPostArrayResponseModelBase, new () {
            var TMResponse = new TModelResponse ();
            try {
                TMResponse = await func?.Invoke (request);
            } catch (ANSHExceptions ex) {
                TMResponse.ItemMsgCode = ex.ERRORCODE;
                TMResponse.ItemMsg = ex.ERRORMSG;
            } catch (Exception ex) {
                Logger.LogError (ex, "服务器内部错误");
                TMResponse.ItemMsgCode = (int) ANSHErrorCodes.服务器内部错误;
                TMResponse.ItemMsg = ANSHErrorCodes.服务器内部错误.ToString ();
            } finally {
                try {
                    callback?.Invoke (request, TMResponse);
                } catch (Exception ex) {
                    Logger.LogError (ex, "服务器内部错误");
                }
            }
            return TMResponse;
        }

        /// <summary>
        /// POST项处理
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        ///<typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TMODELRequest">请求模型</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        protected TResponse Post<TMODELRequest, TResponse, TModelResponse> (ANSHPostRequestBase<TResponse, TMODELRequest, TModelResponse> request, Func<TMODELRequest, TModelResponse> func)
        where TResponse : ANSHPostResponseBase<TModelResponse>, new ()
        where TMODELRequest : ANSHPostRequestModelBase, new ()
        where TModelResponse : class, new () =>
            PostAsync (request, async (parameter) => {
                await Task.CompletedTask;
                return func (parameter);
            }).Result;

        /// <summary>
        /// POST项处理
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="func">项处理</param>
        ///<typeparam name="TResponse">响应</typeparam>
        /// <typeparam name="TMODELRequest">请求模型</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        protected async Task<TResponse> PostAsync<TMODELRequest, TResponse, TModelResponse> (ANSHPostRequestBase<TResponse, TMODELRequest, TModelResponse> request, Func<TMODELRequest, Task<TModelResponse>> func)
        where TResponse : ANSHPostResponseBase<TModelResponse>, new ()
        where TMODELRequest : ANSHPostRequestModelBase, new ()
        where TModelResponse : class, new () =>
            await ExecuteAsync (async () => {
                TResponse response = new TResponse ();
                Validate (request);
                response.ResultItem = await func?.Invoke (request.PostItem);
                return response;
            });

        /// <summary>
        /// 执行保护
        /// </summary>
        /// <param name="func">执行内容</param>
        /// <typeparam name="TResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        protected TResponse Execute<TResponse> (Func<TResponse> func) where TResponse : ANSHResponseBase, new () => ExecuteAsync (async () => {
            await Task.CompletedTask;
            return func ();
        }).Result;

        /// <summary>
        /// 执行保护
        /// </summary>
        /// <param name="action">执行内容</param>
        /// <typeparam name="TResponse">响应模型</typeparam>
        /// <returns>响应</returns>
        protected async Task<TResponse> ExecuteAsync<TResponse> (Func<Task<TResponse>> action)
        where TResponse : ANSHResponseBase, new () {
            TResponse response = null;
            try {
                response = await action?.Invoke ();
            } catch (ANSHExceptions ex) {
                response = new TResponse () { MsgCode = ex.ERRORCODE, Msg = ex.ERRORMSG };
            } catch (Exception ex) {
                Logger.LogError (ex, "服务器内部错误");
                response = new TResponse () {
                    MsgCode = (int) ANSHErrorCodes.服务器内部错误, Msg = ANSHErrorCodes.服务器内部错误.ToString ()
                };
            }
            return response;
        }
    }
}