using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
/// <summary>
/// 拓展类方法
/// </summary>
public static class ANSHSignalRExtensions {

    /// <summary>
    /// 添加JWT验证Token
    /// </summary>
    /// <param name="services">IServiceCollection对象</param>
    /// <param name="securityKey">加密密匙</param>
    public static void AddANSHSignalJWTAuthentication (this IServiceCollection services, string securityKey) =>
        services.AddAuthentication (options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer (options => {
            options.TokenValidationParameters =
                new TokenValidationParameters {
                    LifetimeValidator = (before, expires, token, param) => {
                            return expires > DateTime.UtcNow;
                        },
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateActor = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (securityKey))
                };
            options.Events = new JwtBearerEvents {
                OnMessageReceived = context => {
                    var accessToken = context.Request.Query["access_token"];
                    if (!string.IsNullOrWhiteSpace (accessToken)) {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

    /// <summary>
    /// 添加JWT验证Token
    /// </summary>
    /// <param name="services">IServiceCollection对象</param>
    /// <param name="tokenValidationParameters">验证Token参数</param>
    public static void AddANSHSignalJWTAuthentication (this IServiceCollection services, TokenValidationParameters tokenValidationParameters) =>
        services.AddAuthentication (options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer (options => {
            options.TokenValidationParameters =
                tokenValidationParameters;
            options.Events = new JwtBearerEvents {
                OnMessageReceived = context => {
                    var accessToken = context.Request.Query["access_token"];
                    if (!string.IsNullOrWhiteSpace (accessToken)) {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
}