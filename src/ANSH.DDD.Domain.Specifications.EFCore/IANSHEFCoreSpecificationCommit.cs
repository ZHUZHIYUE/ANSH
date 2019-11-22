using ANSH.DataBase.EFCore;

namespace ANSH.DDD.Domain.Specifications.EFCore {
    /// <summary>
    /// EFCore规约接口
    /// </summary>
    /// <typeparam name="TEntity">规约模型</typeparam>
    public interface IANSHEFCoreSpecificationCommit<TEntity> : IANSHSpecificationCommit<TEntity> where TEntity : class, IANSHDbEntityBase {

    }
}