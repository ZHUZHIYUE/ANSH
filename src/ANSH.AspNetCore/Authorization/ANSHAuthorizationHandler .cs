using Microsoft.AspNetCore.Authorization;

namespace ANSH.AspNetCore.Authorization {
    /// <summary>
    /// 授权处理程序的基类
    /// </summary>
    public abstract class ANSHAuthorizationHandler : AuthorizationHandler<ANSHRequirement> {

    }
}