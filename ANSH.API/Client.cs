﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ANSH.API.RequestContracts;
using ANSH.API.ResponseContracts;
using ANSH.Common.HTTP;

namespace ANSH.API {

    /// <summary>
    /// 执行api请求
    /// </summary>
    public abstract class Client {

        /// <summary>
        /// 获取GET请求地址
        /// </summary>
        /// <param name="APIDoman">api域名地址</param>
        /// <param name="APIName">api方法名称</param>
        /// <param name="APIVersion">api版本号</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>api请求完整地址</returns>
        protected virtual Uri CreateGETUrl (string APIDoman, string APIName, string APIVersion, string accessToken) {
            return CreatePOSTUrl (APIDoman, APIName, APIVersion, accessToken);
        }

        /// <summary>
        /// 获取POST请求地址
        /// </summary>
        /// <param name="APIDoman">api域名地址</param>
        /// <param name="APIName">api方法名称</param>
        /// <param name="APIVersion">api版本号</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>api请求完整地址</returns>
        protected virtual Uri CreatePOSTUrl (string APIDoman, string APIName, string APIVersion, string accessToken) {
            return new Uri ($"{APIDoman.TrimEnd('/')}/{APIName.TrimEnd('/')}/{APIVersion.TrimEnd('/')}/{accessToken.TrimEnd('/')}");
        }

        /// <summary>
        /// 创建GET请求URL参数
        /// </summary>
        /// <param name="request">api请求参数</param>
        /// <param name="APIDoman">api域名地址</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>api请求完整地址</returns>
        protected virtual string CreateGETParameter<TResponse> (GETRequest<TResponse> request, string APIDoman, string accessToken) where TResponse : BaseResponse {
            List<string> parameters = new List<string> ();
            foreach (var item_parameters in request.GetParameters ()) {
                parameters.Add ($"{item_parameters.Key}={item_parameters.Value}");
            }
            return string.Join ("&", parameters.ToArray ());
        }

        /// <summary>
        /// 创建POST请求Body参数
        /// </summary>
        /// <param name="request">api请求参数</param>
        /// <param name="APIDoman">api域名地址</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>api请求完整地址</returns>
        protected virtual string CreatePOSTParameter<TResponse> (POSTRequest<TResponse> request, string APIDoman, string accessToken) where TResponse : BaseResponse {
            return request.ToJson ();
        }

        /// <summary>
        /// api域名地址
        /// </summary>
        protected string APIDoman {
            get;
        }

        /// <summary>
        /// 令牌值
        /// </summary>
        protected string AccessToken {
            get;
            set;
        }

        /// <summary>
        /// 构建API应用对象
        /// </summary>
        /// <param name="api_doman">api域名地址</param>
        public Client (string api_doman) {

            APIDoman = api_doman;
        }

        /// <summary>
        /// 构建API应用对象
        /// </summary>
        /// <param name="api_doman">api域名地址</param>
        /// <param name="accessToken">令牌值</param>
        public Client (string api_doman, string accessToken) {
            APIDoman = api_doman;
            AccessToken = accessToken;
        }

        /// <summary>
        /// 执行POST请求
        /// </summary>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <param name="request">请求参数</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>响应参数</returns>
        public TResponse Execute<TResponse> (POSTRequest<TResponse> request, string accessToken) where TResponse : BaseResponse {
            AccessToken = accessToken;
            return Execute (request);
        }

        /// <summary>
        /// 执行POST请求
        /// </summary>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <param name="request">请求参数</param>
        /// <returns>响应参数</returns>
        public TResponse Execute<TResponse> (POSTRequest<TResponse> request) where TResponse : BaseResponse {
            Uri url = CreatePOSTUrl (APIDoman, request.APIName, request.APIVersion, AccessToken);
            var request_json = CreatePOSTParameter (request, APIDoman, AccessToken);
            string response = HTTPClient.PostString (url, request_json);
            try {
                return response.ToJsonObj<TResponse> ();
            } catch (Newtonsoft.Json.JsonReaderException) {
                throw new Newtonsoft.Json.JsonReaderException ($"未识别的JSON字符串：{response}");
            }
        }

        /// <summary>
        /// 执行GET请求
        /// </summary>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <param name="request">请求参数</param>
        /// <param name="accessToken">令牌值</param>
        /// <returns>响应参数</returns>
        public TResponse Execute<TResponse> (GETRequest<TResponse> request, string accessToken) where TResponse : BaseResponse {
            AccessToken = accessToken;
            return Execute (request);
        }

        /// <summary>
        /// 执行GET请求
        /// </summary>
        /// <typeparam name="TResponse">响应</typeparam>
        /// <param name="request">请求参数</param>
        /// <returns>响应参数</returns>
        public TResponse Execute<TResponse> (GETRequest<TResponse> request) where TResponse : BaseResponse {
            Uri uri = CreateGETUrl (APIDoman, request.APIName, request.APIVersion, AccessToken);

            if (!string.IsNullOrWhiteSpace (uri?.Query)) {
                throw new Exception ($"GET请求地址应保证不带任何参数，错误地址：{uri.AbsolutePath}");
            }

            string response = HTTPClient.GetString (new Uri ($"{uri.AbsoluteUri}?{ CreateGETParameter (request, APIDoman, AccessToken)}"));
            try {
                return response.ToJsonObj<TResponse> ();
            } catch (Newtonsoft.Json.JsonReaderException) {
                throw new Newtonsoft.Json.JsonReaderException ($"未识别的JSON字符串：{response}");
            }
        }
    }
}