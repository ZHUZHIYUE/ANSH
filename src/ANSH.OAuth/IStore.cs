using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.OAuth {
    /// <summary>
    /// 存储方式
    /// </summary>
    public interface IStore {
        /// <summary>
        /// 保存令牌
        /// </summary>
        /// <param name="store">令牌信息</param>
        void SaveToken (StoreToken store);

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="token">令牌值</param>
        /// <returns>找不到对应令牌信息返回null</returns>
        StoreToken GetToken (string token);

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="authorize">授权人</param>
        /// <param name="authorized">被授权人</param>
        /// <param name="token_type">令牌类型</param>
        /// <returns>找不到对应令牌信息返回null</returns>
        List<StoreToken> GetToken (string authorize, string authorized, StoreToken.TokenTypes token_type);

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="authorize">授权人</param>
        /// <param name="authorized">被授权人</param>
        /// <returns>找不到对应令牌信息返回null</returns>
        List<StoreToken> GetToken (string authorize, string authorized);

        /// <summary>
        /// 删除令牌
        /// </summary>
        /// <param name="stoken">令牌信息</param>
        void OutToken (params StoreToken[] stoken);

        /// <summary>
        /// 返回所有expires_times过期的令牌信息
        /// </summary>
        /// <returns>找不到对应令牌信息返回null</returns>
        List<StoreToken> GetTokenOutOfDate ();
    }

}