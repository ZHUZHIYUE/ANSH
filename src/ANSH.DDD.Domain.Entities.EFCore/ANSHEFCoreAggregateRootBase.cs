using System;
using System.ComponentModel.DataAnnotations;

namespace ANSH.DDD.Domain.Entities.EFCore {
    /// <summary>
    /// EFCore聚合根的抽象实现类，定义聚合根的公共属性和行为
    /// </summary>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public class ANSHEFCoreAggregateRootBase<TPKey> : ANSHEFCoreEntityBase<TPKey>, IANSHEFCoreAggregateRoot<TPKey> where TPKey : struct, IEquatable<TPKey> {

    }
}