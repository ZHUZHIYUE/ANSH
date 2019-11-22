using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ANSH.DDD.Domain.Entities.EFCore;
using ANSH.DDD.Domain.Interface.IEntities;
using ANSH.DDD.Domain.Interface.IRepositories;
using ANSH.DDD.Domain.Specifications;

namespace ANSH.DDD.Domain.Server {
    /// <summary>
    /// 聚合根EFCore仓储服务
    /// </summary>
    public abstract class ANSHDomainServerBase : IANSHDomainServer {
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose () { }
    }
}