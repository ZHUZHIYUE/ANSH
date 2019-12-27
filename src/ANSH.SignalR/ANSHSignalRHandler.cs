using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ANSH.SignalR {
    /// <summary>
    /// 帮助类
    /// </summary>
    public static class ANSHSignalRHandler {
        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="userName">用户名（验证成功后存储于connection.User）</param>
        /// <param name="securityKey">加密密匙</param>
        /// <param name="expires">过期时间单位分钟</param>
        /// <returns>Token值</returns>
        public static string CreateJwtToken (string userName, string securityKey, int expires) => CreateJwtToken (new JwtSecurityToken (
            expires: DateTime.Now.AddMinutes (expires),
            signingCredentials: new SigningCredentials (new SymmetricSecurityKey (Encoding.UTF8.GetBytes (securityKey)), SecurityAlgorithms.HmacSha256Signature),
            claims : new Claim[] {
                new Claim (ClaimTypes.Name, userName),
                    new Claim (ClaimTypes.NameIdentifier, userName)
            }));

        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="userName">用户名（验证成功后存储于connection.User）</param>
        /// <param name="securityKey">加密密匙</param>
        /// <returns>Token值</returns>
        public static string CreateJwtToken (string userName, string securityKey) => CreateJwtToken (userName, securityKey, 20);

        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="jwtSecurityToken">Token参数</param>
        /// <returns>Token值</returns>
        public static string CreateJwtToken (JwtSecurityToken jwtSecurityToken) => new JwtSecurityTokenHandler ().WriteToken (jwtSecurityToken);
    }
}