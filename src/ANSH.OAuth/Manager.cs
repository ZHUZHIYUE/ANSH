using System;
using System.Collections.Generic;
using ANSH.JWT;

namespace ANSH.OAuth {
    /// <summary>
    /// 授权服务
    /// </summary>
    public class Manager {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Manager () { }

        #region 授权码
        /// <summary>
        /// 创建授权码
        /// <param name="authorize">授权人</param>
        /// <param name="authorized">被授权人</param>
        /// <param name="secretKey">密匙</param>
        /// <param name="expires">有效时间（单位分）</param>
        /// </summary>
        public TokenCode CreateCODE (string authorize, string authorized, string secretKey, int expires = 2) {
            TokenCode tokenCode = new TokenCode () {
            Code = CreateAccessToken (authorize, authorized, TokenTypes.code, secretKey, expires),
            Create_Times = DateTime.Now,
            Expires_IN = expires
            };
            return tokenCode;
        }

        /// <summary>
        /// 验证授权码
        /// </summary>
        /// <param name="CODE">授权码值</param>
        /// <param name="secretKey">密匙</param>
        /// <param name="accessTokenModel">accessToken内容</param>
        bool VerifyCODE (string CODE, string secretKey, out ANSHAccessToken accessTokenModel) {
            if (VerifyAccessToken (CODE, secretKey, out accessTokenModel) && accessTokenModel.TokenType == TokenTypes.code) {
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
        /// <param name="secretKey">密匙</param>
        /// <param name="expires">有效时间（单位分）</param>
        /// </summary>
        RefreshToken CreateRefreshToken (string authorize, string authorized, string secretKey, int expires = 60 * 24 * 7) {
            RefreshToken refreshToken = new RefreshToken () {
            Refresh_Token = CreateAccessToken (authorize, authorized, TokenTypes.reftoken, secretKey, expires),
            Create_Times = DateTime.Now,
            Expires_IN = expires
            };
            return refreshToken;
        }

        /// <summary>
        /// 验证刷新令牌
        /// </summary>
        /// <param name="REFRESH_TOKEN">刷新令牌值</param>
        /// <param name="secretKey">密匙</param>
        /// <param name="accessTokenModel">accessToken内容</param>
        bool VerifyRefreshToken (string REFRESH_TOKEN, string secretKey, out ANSHAccessToken accessTokenModel) {
            if (VerifyAccessToken (REFRESH_TOKEN, secretKey, out accessTokenModel) && accessTokenModel.TokenType == TokenTypes.reftoken) {
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
        /// <param name="secretKey">密匙</param>
        /// <param name="token">令牌值</param>
        /// <param name="access_token_expires">access_token有效时间（单位分钟）</param>
        /// <param name="refresh_token_expires">refresh_token有效时间（单位分钟）</param>
        /// <returns>成功创建令牌返回True，反之false</returns>
        public bool CreateToken (string grant_key, GrantTypes grant_type, string secretKey, out Token token, int access_token_expires = 120, int refresh_token_expires = 120) {
            token = null;
            ANSHAccessToken accessTokenModel = null;

            string refresh_token = string.Empty;
            string authorize = string.Empty, authorized = string.Empty;
            int access_token_expires_new = access_token_expires, refresh_token_expires_new = refresh_token_expires;

            switch (grant_type) {
                case GrantTypes.authorization_code:
                    {
                        if (!VerifyCODE (grant_key, secretKey, out accessTokenModel)) return false;
                        authorize = accessTokenModel.Authorize;
                        authorized = accessTokenModel.Authorized;
                        var refToken = CreateRefreshToken (authorize, authorized, secretKey, refresh_token_expires_new);
                        refresh_token = refToken.Refresh_Token;
                        refresh_token_expires_new = refToken.Expires_IN;
                    }
                    break;
                case GrantTypes.refresh_token:
                    {
                        if (!VerifyRefreshToken (grant_key, secretKey, out accessTokenModel)) return false;
                        authorize = accessTokenModel.Authorize;
                        authorized = accessTokenModel.Authorized;
                        access_token_expires_new = refresh_token_expires_new = (accessTokenModel.Exp.Value.ToTimeStamp () - DateTime.Now).Minutes;
                        refresh_token = grant_key;
                    }
                    break;
            }

            token = new Token () {
                Access_Token = CreateAccessToken (authorize, authorized, TokenTypes.token, secretKey, access_token_expires_new),
                Create_Times = DateTime.Now,
                Expires_IN = access_token_expires_new,
                Refresh_Token = refresh_token,
                Refexpires_IN = refresh_token_expires_new
            };
            return true;
        }

        /// <summary>
        /// 验证身份令牌
        /// </summary>
        /// <param name="access_token">令牌值</param>
        /// <param name="secretKey">密匙</param>
        /// <param name="accessTokenModel">accessToken内容</param>
        public bool VerifyToken (string access_token, string secretKey, out ANSHAccessToken accessTokenModel) {
            if (VerifyAccessToken (access_token, secretKey, out accessTokenModel) && accessTokenModel.TokenType == TokenTypes.token) {
                return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 创建AccessToken
        /// </summary>
        /// <param name="authorize">授权人</param>
        /// <param name="authorized">被授权人</param>
        /// <param name="tokenType">令牌类型</param>
        /// <param name="secretKey">密匙</param>
        /// <param name="expires">有效时间（单位分钟）</param>
        /// <returns>AccessToken</returns>
        static string CreateAccessToken (string authorize, string authorized, TokenTypes tokenType, string secretKey, int expires) {
            var jwtPayload = new ANSHAccessToken ();
            jwtPayload.Exp += expires * 60;
            jwtPayload.Jti = Guid.NewGuid ().ToString ("N");
            jwtPayload.Authorize = authorize;
            jwtPayload.Authorized = authorized;
            jwtPayload.TokenType = tokenType;
            jwtPayload.Iat = DateTime.Now.ToTimeStamp ();
            return ANSHJWT.Encode (jwtPayload, secretKey);
        }

        /// <summary>
        /// 验证AccessToken
        /// </summary>
        /// <param name="access_token">令牌值</param>
        /// <param name="secretKey">密匙</param>
        /// <param name="accessTokenModel">accessToken内容</param>
        /// <returns>AccessToken</returns>
        static bool VerifyAccessToken (string access_token, string secretKey, out ANSHAccessToken accessTokenModel) => ANSHJWT.Decode (access_token, secretKey, out accessTokenModel);
    }
}