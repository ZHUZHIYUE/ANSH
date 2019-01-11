using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ANSH.DataBase.EFCore {
    /// <summary>
    /// EF数据模型基类接口
    /// </summary>
    public interface IDBDomainEntity<TPKey> : IDBEntity where TPKey : struct, IEquatable<TPKey> {

        /// <summary>
        /// 主键
        /// </summary>
        TPKey? Id {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? create_times {
            get;
            set;
        }

        /// <summary>
        /// 行版本控制
        /// </summary>
        byte[] timestamp {
            get;
            set;
        }
    }
}