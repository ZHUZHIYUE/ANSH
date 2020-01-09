using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using ANSH.Common.Enums;
using ANSH.Common.HTTP;
using ANSH.SDK.API.RequestContracts;
using ANSH.SDK.API.RequestContracts.Models;
using ANSH.SDK.API.ResponseContracts;
using ANSH.SDK.API.ResponseContracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace ANSH.SDK.API {

    /// <summary>
    /// 执行api请求
    /// </summary>
    public abstract class ANSHApiClient {

        /// <summary>
        /// 获取GET请求地址
        /// </summary>
        /// <param name="apiName">api方法名称</param>
        /// <param name="apiVersion">api版本号</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>api请求完整地址</returns>
        protected virtual Uri CreateGETUrl (string apiName, string apiVersion, string accessToken) => CreatePOSTUrl (apiName, apiVersion, accessToken);

        /// <summary>
        /// 获取POST请求地址
        /// </summary>
        /// <param name="apiName">api方法名称</param>
        /// <param name="apiVersion">api版本号</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>api请求完整地址</returns>
        protected virtual Uri CreatePOSTUrl (string apiName, string apiVersion, string accessToken) => new Uri ($"{APIDoman?.TrimEnd('/')}/{apiName?.TrimEnd('/')}/?apiVersion={apiVersion}&access_token={accessToken}");

        // /// <summary>
        // /// 创建GET请求URL参数
        // /// </summary>
        // /// <param name="request">api请求参数</param>
        // /// <param name="apiDoman">api域名地址</param>
        // /// <param name="accessToken">令牌值</param>
        // /// <returns>api请求完整地址</returns>
        // protected virtual string CreateGETParameter<ANSHTResponse, ANSHTModelResponse> (ANSHGetRequestBase<ANSHTResponse, ANSHTModelResponse> request, string apiDoman, string accessToken)
        // where ANSHTResponse : ANSHGetResponseBase<ANSHTModelResponse>
        //     where ANSHTModelResponse : class {
        //         List<string> parameters = new List<string> ();
        //         foreach (var item_parameters in request.GetParameters ()) {
        //             parameters.Add ($"{item_parameters.Key}={item_parameters.Value}");
        //         }
        //         return string.Join ("&", parameters.ToArray ());
        //     }

        /// <summary>
        /// 创建POST请求Body参数
        /// </summary>
        /// <typeparam name="ANSHTResponse">响应</typeparam>
        /// <typeparam name="ANSHTMODELRequest">请求模型</typeparam>
        /// <typeparam name="ANSHTModelResponse">响应模型</typeparam>
        /// <param name="request">api请求参数</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>api请求完整地址</returns>
        protected virtual string CreatePOSTArrayParameter<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> (ANSHPostArrayRequestBase<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> request, string accessToken)
        where ANSHTResponse : ANSHPostArrayResponseBase<ANSHTModelResponse>
            where ANSHTMODELRequest : ANSHPostRequestModelBase
        where ANSHTModelResponse : ANSHPostArrayResponseModelBase {
            return request.ToJson ();
        }

        /// <summary>
        /// 创建POST请求Body参数
        /// </summary>
        /// <typeparam name="ANSHTResponse">响应</typeparam>
        /// <typeparam name="ANSHTMODELRequest">请求模型</typeparam>
        /// <typeparam name="ANSHTModelResponse">响应模型</typeparam>
        /// <param name="request">api请求参数</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>api请求完整地址</returns>
        protected virtual string CreatePOSTItemParameter<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> (ANSHPostRequestBase<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> request, string accessToken)
        where ANSHTResponse : ANSHPostResponseBase<ANSHTModelResponse>
            where ANSHTMODELRequest : ANSHPostRequestModelBase
        where ANSHTModelResponse : class {
            return request.ToJson ();
        }

        /// <summary>
        /// api域名地址
        /// </summary>
        string APIDoman {
            get;
        }

        /// <summary>
        /// 令牌值
        /// </summary>
        string AccessToken {
            get;
            set;
        }

        /// <summary>
        /// 构建API应用对象
        /// </summary>
        /// <param name="apiDoman">api域名地址</param>
        public ANSHApiClient (string apiDoman) {

            APIDoman = apiDoman;
        }

        /// <summary>
        /// 构建API应用对象
        /// </summary>
        /// <param name="apiDoman">api域名地址</param>
        /// <param name="accessToken">令牌值</param>
        public ANSHApiClient (string apiDoman, string accessToken) {
            this.APIDoman = apiDoman;
            this.AccessToken = accessToken;
        }

        /// <summary>
        /// 执行POST请求
        /// </summary>
        /// <typeparam name="ANSHTResponse">响应</typeparam>
        /// <typeparam name="ANSHTMODELRequest">请求模型</typeparam>
        /// <typeparam name="ANSHTModelResponse">响应模型</typeparam>
        /// <param name="request">请求参数</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>响应参数</returns>
        public async Task<ANSHTResponse> ExecuteAsync<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> (ANSHPostArrayRequestBase<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> request, string accessToken = null)
        where ANSHTResponse : ANSHPostArrayResponseBase<ANSHTModelResponse>
            where ANSHTMODELRequest : ANSHPostRequestModelBase
        where ANSHTModelResponse : ANSHPostArrayResponseModelBase {
            var _accessToken = accessToken??AccessToken;
            Uri url = CreatePOSTUrl (request.APIName, request.APIVersion, _accessToken);
            var request_json = CreatePOSTArrayParameter (request, _accessToken);
            var httpmsg_response = await HTTPClient.PostAsync (url, request_json, MediaTypeHeaderValue.Parse ("application/json;charset=utf-8"));
            string response = await httpmsg_response.Content.ReadAsStringAsync ();

            try {
                return response.ToJsonObj<ANSHTResponse> ();
            } catch (Newtonsoft.Json.JsonReaderException) {
                throw new Newtonsoft.Json.JsonReaderException ($"未识别的JSON字符串：{response}");
            }
        }

        /// <summary>
        /// 执行POST请求
        /// </summary>
        /// <typeparam name="ANSHTResponse">响应</typeparam>
        /// <typeparam name="ANSHTMODELRequest">请求模型</typeparam>
        /// <typeparam name="ANSHTModelResponse">响应模型</typeparam>
        /// <param name="request">请求参数</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>响应参数</returns>
        public async Task<ANSHTResponse> ExecuteAsync<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> (ANSHPostRequestBase<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> request, string accessToken = null)
        where ANSHTResponse : ANSHPostResponseBase<ANSHTModelResponse>
            where ANSHTMODELRequest : ANSHPostRequestModelBase
        where ANSHTModelResponse : class {
            var _accessToken = accessToken??AccessToken;
            Uri url = CreatePOSTUrl (request.APIName, request.APIVersion, _accessToken);
            var request_json = CreatePOSTItemParameter (request, _accessToken);
            var httpmsg_response = await HTTPClient.PostAsync (url, request_json, MediaTypeHeaderValue.Parse ("application/json;charset=utf-8"));
            string response = await httpmsg_response.Content.ReadAsStringAsync ();
            try {
                return response.ToJsonObj<ANSHTResponse> ();
            } catch (Newtonsoft.Json.JsonReaderException) {
                throw new Newtonsoft.Json.JsonReaderException ($"未识别的JSON字符串：{response}");
            }
        }

        /// <summary>
        /// 执行GET请求
        /// </summary>
        /// <typeparam name="ANSHQueryRequest">查询参数</typeparam>
        /// <typeparam name="ANSHTResponse">响应</typeparam>
        /// <typeparam name="TModelResponse">响应模型</typeparam>
        /// <param name="request">请求参数</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>响应参数</returns>
        public async Task<ANSHTResponse> ExecuteAsync<ANSHQueryRequest, ANSHTResponse, TModelResponse> (ANSHGetRequestBase<ANSHQueryRequest, ANSHTResponse, TModelResponse> request, string accessToken = null)
        where ANSHTResponse : ANSHGetResponseBase<TModelResponse>
            where TModelResponse : class
        where ANSHQueryRequest : ANSHGetRequestModelBase {
            var _accessToken = accessToken??AccessToken;
            Uri base_uri = CreateGETUrl (request.APIName, request.APIVersion, _accessToken);

            string typeOfQueryPrefix = string.Empty;
            var queryProperties = request.GetType ().GetProperty (nameof (request.Query));

            typeOfQueryPrefix = queryProperties.Name;

            var typeOfQuery = request.Query.GetType ();
            var propertiesOfQuery = typeOfQuery.GetProperties ();
            List<string> parametersOfQuery = new List<string> ();
            if (propertiesOfQuery?.Length > 0) {
                foreach (var propertiesOfQueryItem in propertiesOfQuery) {
                    if (propertiesOfQueryItem.CanRead) {

                        Type propertiesOfType = Nullable.GetUnderlyingType (propertiesOfQueryItem.PropertyType) ?? propertiesOfQueryItem.PropertyType;

                        if (
                            typeof (string).IsAssignableFrom (propertiesOfType) ||
                            typeof (int).IsAssignableFrom (propertiesOfType) ||
                            typeof (long).IsAssignableFrom (propertiesOfType) ||
                            typeof (decimal).IsAssignableFrom (propertiesOfType) ||
                            typeof (double).IsAssignableFrom (propertiesOfType) ||
                            typeof (float).IsAssignableFrom (propertiesOfType) ||
                            typeof (Enum).IsAssignableFrom (propertiesOfType) ||
                            typeof (bool).IsAssignableFrom (propertiesOfType)
                        ) {
                            string propertiesOfQueryItemKey = string.Empty, propertiesOfQueryItemValue = string.Empty;

                            if (typeof (Enum).IsAssignableFrom (propertiesOfType)) {
                                if (propertiesOfQueryItem.IsDefined (typeof (ANSHCustomEnumConverAttribute))) {
                                    var propertiesOfQueryAttribute = (ANSHCustomEnumConverAttribute) propertiesOfQueryItem.GetCustomAttribute (typeof (ANSHCustomEnumConverAttribute));
                                    if (propertiesOfQueryAttribute.Convert == ANSHEnumConvertMethod.String) {
                                        propertiesOfQueryItemValue = System.Enum.Parse (propertiesOfType, propertiesOfQueryItemValue)?.ToString ();
                                    } else {
                                        propertiesOfQueryItemValue = ((int?) System.Enum.Parse (propertiesOfType, propertiesOfQueryItemValue))?.ToString ();
                                    }
                                } else {
                                    propertiesOfQueryItemValue = propertiesOfQueryItem.GetValue (request.Query)?.ToString () ?? string.Empty;
                                }
                            } else {
                                if (propertiesOfQueryItem.IsDefined (typeof (FromQueryAttribute))) {
                                    var propertiesOfQueryAttribute = (FromQueryAttribute) propertiesOfQueryItem.GetCustomAttribute (typeof (FromQueryAttribute));
                                    propertiesOfQueryItemKey = propertiesOfQueryAttribute.Name;
                                } else {
                                    propertiesOfQueryItemKey = propertiesOfQueryItem.Name;
                                }
                                propertiesOfQueryItemValue = propertiesOfQueryItem.GetValue (request.Query)?.ToString () ?? string.Empty;
                            }

                            parametersOfQuery.Add ($"{typeOfQueryPrefix}.{propertiesOfQueryItemKey}={propertiesOfQueryItemValue}");
                        } else {
                            throw new Exception ($"查询参数暂不支持该类型：{propertiesOfType.Name}。");
                        }
                    }
                }
            }
            var get_url = new Uri ($"{base_uri.AbsoluteUri.TrimEnd('/','\\')}{(string.IsNullOrWhiteSpace (base_uri?.Query) ? "?" : "&")}{ string.Join ("&", parametersOfQuery.ToArray ())}");

            var httpmsg_response = await HTTPClient.GetAsync (get_url);
            string response = await httpmsg_response.Content.ReadAsStringAsync ();
            try {
                return response.ToJsonObj<ANSHTResponse> ();
            } catch (Newtonsoft.Json.JsonReaderException) {
                throw new Newtonsoft.Json.JsonReaderException ($"未识别的JSON字符串：{response}");
            }
        }

        /// <summary>
        /// 执行POST请求
        /// </summary>
        /// <typeparam name="ANSHTResponse">响应</typeparam>
        /// <typeparam name="ANSHTMODELRequest">请求模型</typeparam>
        /// <typeparam name="ANSHTModelResponse">响应模型</typeparam>
        /// <param name="request">请求参数</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>响应参数</returns>
        public ANSHTResponse Execute<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> (ANSHPostArrayRequestBase<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> request, string accessToken = null)
        where ANSHTResponse : ANSHPostArrayResponseBase<ANSHTModelResponse>
            where ANSHTMODELRequest : ANSHPostRequestModelBase
        where ANSHTModelResponse : ANSHPostArrayResponseModelBase {
            return ExecuteAsync (request, accessToken).Result;
        }

        /// <summary>
        /// 执行POST请求
        /// </summary>
        /// <typeparam name="ANSHTResponse">响应</typeparam>
        /// <typeparam name="ANSHTMODELRequest">请求模型</typeparam>
        /// <typeparam name="ANSHTModelResponse">响应模型</typeparam>
        /// <param name="request">请求参数</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>响应参数</returns>
        public ANSHTResponse Execute<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> (ANSHPostRequestBase<ANSHTResponse, ANSHTMODELRequest, ANSHTModelResponse> request, string accessToken = null)
        where ANSHTResponse : ANSHPostResponseBase<ANSHTModelResponse>
            where ANSHTMODELRequest : ANSHPostRequestModelBase
        where ANSHTModelResponse : class {
            return ExecuteAsync (request, accessToken).Result;
        }

        /// <summary>
        /// 执行GET请求
        /// </summary>
        /// <typeparam name="ANSHQueryRequest">查询参数</typeparam>
        /// <typeparam name="ANSHTResponse">响应</typeparam>
        /// <typeparam name="ANSHTModelResponse">响应模型</typeparam>
        /// <param name="request">请求参数</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>响应参数</returns>
        public ANSHTResponse Execute<ANSHQueryRequest, ANSHTResponse, ANSHTModelResponse> (ANSHGetRequestBase<ANSHQueryRequest, ANSHTResponse, ANSHTModelResponse> request, string accessToken = null)
        where ANSHTResponse : ANSHGetResponseBase<ANSHTModelResponse>
            where ANSHTModelResponse : class
        where ANSHQueryRequest : ANSHGetRequestModelBase => ExecuteAsync (request, accessToken).Result;
    }
}