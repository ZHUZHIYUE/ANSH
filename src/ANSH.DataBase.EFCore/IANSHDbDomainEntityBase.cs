using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ANSH.DataBase.EFCore {
    /// <summary>
    /// EF数据模型基类接口
    /// </summary>
    public interface IANSHDbDomainEntityBase<TPKey> : IANSHDbEntityBase where TPKey : struct, IEquatable<TPKey> {

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
        DateTime? CreateTimes {
            get;
            set;
        }

        /// <summary>
        /// 最近一次修改时间
        /// </summary>
        DateTime? UpdateTimes {
            get;
            set;
        }

        /// <summary>
        /// 行版本控制
        /// </summary>
        byte[] Timestamp {
            get;
            set;
        }
    }
}