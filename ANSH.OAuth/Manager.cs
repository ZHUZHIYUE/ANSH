using System;
using System.Collections.Generic;

namespace ANSH.OAuth
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="STORE">存储方式</param>
        public Manager(IStore STORE)
        {
            _STORE = STORE;
            FailureOutOfDate();
        }

        /// <summary>
        /// 存储方式
        /// </summary>
        IStore _STORE
        {
            get;
            set;
        }

        #region 授权码
        /// <summary>
        /// 创建授权码
        /// <param name="authorize">授权人</param>
        /// <param name="authorized">被授权人</param>
        /// <param name="expires">有效时间（单位分）</param>
        /// </summary>
        public TokenCode CreateCODE(string authorize, string authorized, int expires = 2)
        {
            TokenCode tokenCode = new TokenCode()
            {
                Code = CreateRandomNumb()
                ,
                Create_Times = DateTime.Now
                ,
                Expires_IN = expires
            };

            StoreToken stoken = new StoreToken()
            {
                Authorize = authorize
                ,
                Authorized = authorized
                ,
                Create_Times = tokenCode.Create_Times
                ,
                Expires_Times = tokenCode.Create_Times.AddMinutes(tokenCode.Expires_IN)
                ,
                Token = tokenCode.Code
                ,
                Token_Type = StoreToken.TokenTypes.code
            };

            _STORE.OutToken((_STORE.GetToken(stoken.Authorize, stoken.Authorized, stoken.Token_Type) ?? new List<OAuth.StoreToken>()).ToArray());

            _STORE.SaveToken(stoken);

            return tokenCode;
        }

        /// <summary>
        /// 验证授权码
        /// </summary>
        /// <param name="CODE">授权码值</param>
        /// <param name="stoken">对应token</param>
        bool VerifyCODE(string CODE, out StoreToken stoken)
        {
            stoken = _STORE.GetToken(CODE);
            if (stoken != null
                && stoken.Token_Type == StoreToken.TokenTypes.code
                && stoken.Expires_Times >= DateTime.Now)
            {
                _STORE.OutToken(stoken);

                return true;
            }
            return false;
        }
        #endregion

        #region 刷新令牌
        /// <summary>
        /// 创建刷新令牌
        /// <param name="authorize">授权人</param>
        /// <param name="authorized">被授权人</param>
        /// <param name="expires">有效时间（单位分）</param>
        /// </summary>
        RefreshToken CreateRefreshToken(string authorize, string authorized, int expires = 60 * 24 * 7)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                Refresh_Token = CreateRandomNumb()
                ,
                Create_Times = DateTime.Now
                ,
                Expires_IN = expires
            };

            var stoken = new StoreToken()
            {
                Authorize = authorize
                ,
                Authorized = authorized
                ,
                Create_Times = refreshToken.Create_Times
                ,
                Expires_Times = refreshToken.Create_Times.AddMinutes(refreshToken.Expires_IN)
                ,
                Token = refreshToken.Refresh_Token
                ,
                Token_Type = StoreToken.TokenTypes.reftoken
            };

            _STORE.OutToken((_STORE.GetToken(stoken.Authorize, stoken.Authorized, stoken.Token_Type) ?? new List<OAuth.StoreToken>()).ToArray());

            _STORE.SaveToken(stoken);

            return refreshToken;
        }

        /// <summary>
        /// 验证刷新令牌
        /// </summary>
        /// <param name="REFRESH_TOKEN">刷新令牌值</param>
        /// <param name="stoken">对应token</param>
        bool VerifyRefreshToken(string REFRESH_TOKEN, out StoreToken stoken)
        {
            stoken = _STORE.GetToken(REFRESH_TOKEN);
            if (stoken != null
                && stoken.Token_Type == StoreToken.TokenTypes.reftoken
                && stoken.Expires_Times >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 令牌

        /// <summary>
        /// 创建令牌
        /// </summary>
        /// <param name="grant_key">授权值</param>
        /// <param name="grant_type">授权方式</param>
        /// <param name="token">令牌值</param>
        /// <param name="access_token_expires">access_token有效时间（单位分钟）</param>
        /// <param name="refresh_token_expires">refresh_token有效时间（单位分钟）</param>
        /// <returns>成功创建令牌返回True，反之false</returns>
        public bool CreateToken(string grant_key, GrantTypes grant_type, out Token token, int access_token_expires = 120, int refresh_token_expires = 120)
        {
            token = null;
            StoreToken stoken = null;

            string access_token = CreateRandomNumb(), refresh_token = string.Empty;
            string authorize = string.Empty, authorized = string.Empty;

            StoreToken.TokenTypes token_type = StoreToken.TokenTypes.client_token;

            switch (grant_type)
            {
                case GrantTypes.authorization_code:
                    {
                        if (!VerifyCODE(grant_key, out stoken)) return false;

                        authorize = stoken.Authorize;
                        authorized = stoken.Authorized;
                        var refToken = CreateRefreshToken(authorize, authorized, refresh_token_expires);
                        refresh_token = refToken.Refresh_Token;
                        refresh_token_expires = refToken.Expires_IN;
                        token_type = StoreToken.TokenTypes.token;
                    }
                    break;
                case GrantTypes.refresh_token:
                    {
                        if (!VerifyRefreshToken(grant_key, out stoken)) return false;
                        authorize = stoken.Authorize;
                        authorized = stoken.Authorized;
                        refresh_token_expires = (stoken.Expires_Times - DateTime.Now).Minutes;
                        refresh_token = stoken.Token;
                        token_type = StoreToken.TokenTypes.token;
                    }
                    break;
            }

            token = new Token()
            {
                Access_Token = access_token
                ,
                Create_Times = DateTime.Now
                ,
                Expires_IN = access_token_expires
                ,
                Refresh_Token = refresh_token
                ,
                Refexpires_IN = refresh_token_expires
            };

            stoken = new StoreToken()
            {
                Authorize = authorize
                ,
                Authorized = authorized
                ,
                Create_Times = token.Create_Times
                ,
                Expires_Times = token.Create_Times.AddMinutes(token.Expires_IN)
                ,
                Token = token.Access_Token
                ,
                Token_Type = token_type
            };

            _STORE.OutToken((_STORE.GetToken(stoken.Authorize, stoken.Authorized, stoken.Token_Type) ?? new List<OAuth.StoreToken>()).ToArray());

            _STORE.SaveToken(stoken);

            return true;
        }


        /// <summary>
        /// 验证身份令牌
        /// </summary>
        /// <param name="access_token">令牌值</param>
        /// <param name="stoken">对应token</param>
        public bool VerifyToken(string access_token, out StoreToken stoken)
        {
            stoken = _STORE.GetToken(access_token);
            if (stoken != null
                && (stoken.Token_Type == StoreToken.TokenTypes.token)
                && stoken.Expires_Times >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 创建随即值
        /// </summary>
        /// <returns></returns>
        static string CreateRandomNumb()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 过期失效处理
        /// </summary>
        void FailureOutOfDate()
        {
            _STORE.OutToken((_STORE.GetTokenOutOfDate() ?? new List<OAuth.StoreToken>()).ToArray());
        }
    }
}