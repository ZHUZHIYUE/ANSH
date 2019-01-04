using ANSH.DataBase.EFCore;
using Microsoft.EntityFrameworkCore;

namespace ANSH.DataBase.IUnitOfWorks.EFCore {
    /// <summary>
    /// EFCore仓储操作工作基类接口
    /// </summary>
    public interface IANSHEFCoreUnitOfWork : IANSHUnitOfWork {

        /// <summary>
        /// 创建对应的访问层对象
        /// <remarks>创建的对象都一直保存在集合中，直到集合批量Dispose。</remarks>
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        TResult Register<TResult> () where TResult : DBContext, new ();
    }
}