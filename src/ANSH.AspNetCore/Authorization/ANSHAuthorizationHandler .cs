using Microsoft.AspNetCore.Authorization;

namespace ANSH.AspNetCore.Authorization {
    /// <summary>
    /// 授权处理程序的基类
    /// </summary>
    public abstract class ANSHAuthorizationHandler<TANSHRequirement> : AuthorizationHandler<TANSHRequirement>
        where TANSHRequirement : ANSHRequirement {

        }

    /// <summary>
    /// 授权处理程序的基类
    /// </summary>
    public abstract class ANSHAuthorizationHandler : AuthorizationHandler<ANSHRequirement> {

    }
}