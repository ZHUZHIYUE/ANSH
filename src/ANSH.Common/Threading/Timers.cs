using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ANSH.Common.Threading {
    /// <summary>
    /// 定时执行
    /// </summary>
    public class Timers {
        /// <summary>
        /// 间隔传递参数
        /// </summary>
        class TimerParam {
            /// <summary>
            /// 唯一值
            /// </summary>
            public Guid Item_Guid {
                get;
                set;
            }

            /// <summary>
            /// 当上一次未执行完成时，是否开始一次新的执行。
            /// </summary>
            public bool Repeat_EXE {
                get;
                set;
            }
        }

        /// <summary>
        /// 间隔执行
        /// </summary>
        /// <param name="action">执行的方法</param>
        /// <param name="interval">间隔时间，单位毫秒</param>
        /// <param name="repeat_exe">当上一次未执行完成时，是否开始一次新的执行。</param>
        /// <param name="cancellationToken">Task取消</param>
        public static void ExecuteInterval (Action action, long interval, bool repeat_exe = false, CancellationToken cancellationToken = default (CancellationToken)) {
            Task.Factory.StartNew (() => {
                using (new Timer (param => {
                    Execute ((TimerParam) param, action);
                }, new TimerParam () { Item_Guid = Guid.NewGuid (), Repeat_EXE = repeat_exe }, 0, interval)) {
                    cancellationToken.WaitHandle.WaitOne ();
                }
            }, cancellationToken);
        }

        /// <summary>
        /// 执行线程池
        /// </summary>
        static Dictionary<Guid, Task> _Tasks = null;

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="param">间隔传递参数</param>
        /// <param name="action">执行的方法</param>
        static void Execute (TimerParam param, Action action) {
            if (param.Repeat_EXE) {
                Task.Factory.StartNew (action);
            } else {
                Add (param.Item_Guid, action);
            }
        }

        static object _lock = new Object ();

        /// <summary>
        /// 删除已完成的线程
        /// </summary>
        /// <param name="guid">线程唯一标识</param>
        static void Remove (Guid guid) {
            lock (_lock) {
                _Tasks = _Tasks ?? new Dictionary<Guid, Task> ();
                _Tasks.Remove (guid);
            }
        }

        /// <summary>
        /// 添加线程
        /// </summary>
        /// <param name="key">线程唯一标识</param>
        /// <param name="action">执行的方法</param>
        static void Add (Guid key, Action action) {
            lock (_lock) {
                _Tasks = _Tasks ?? new Dictionary<Guid, Task> ();
                if (!_Tasks.ContainsKey (key)) {
                    _Tasks.Add (key, Task.Factory.StartNew (action).ContinueWith ((task, obj) => Remove ((Guid) obj), key));
                }
            }
        }

        /// <summary>
        /// 每天执行
        /// </summary>
        /// <param name="action">执行的方法</param>
        /// <param name="hh">每天时</param>
        /// <param name="mm">每天分</param>
        /// <param name="ss">每天秒</param>
        /// <param name="repeat_exe">当上一次未执行完成时，是否开始一次新的执行。</param>
        public static void ExecuteEverDay (Action action, int hh, int mm, int ss, bool repeat_exe = false) {
            Task.Factory.StartNew (() => {
                using (new Timer (param => {
                    DateTime date = DateTime.Now;
                    DateTime exe_date = new DateTime (date.Year, date.Month, date.Day, hh, mm, ss);
                    if (date.ToTimeStamp () == exe_date.ToTimeStamp ()) {
                        Execute ((TimerParam) param, action);
                    }
                }, new TimerParam () { Item_Guid = Guid.NewGuid (), Repeat_EXE = repeat_exe }, 0, 1000)) {

                    Thread.Sleep (Timeout.Infinite);
                }
            });
        }

        /// <summary>
        /// 每月执行
        /// </summary>
        /// <param name="action">执行的方法</param>
        /// <param name="dd">每月日</param>
        /// <param name="hh">每月时</param>
        /// <param name="mm">每月分</param>
        /// <param name="ss">每月秒</param>
        /// <param name="repeat_exe">当上一次未执行完成时，是否开始一次新的执行。</param>
        public static void ExecuteEverMonth (Action action, int dd, int hh, int mm, int ss, bool repeat_exe = false) {
            Task.Factory.StartNew (() => {
                using (new Timer (param => {
                    DateTime date = DateTime.Now;
                    DateTime exe_date = new DateTime (date.Year, date.Month, dd, hh, mm, ss);
                    if (date.ToTimeStamp () == exe_date.ToTimeStamp ()) {
                        Execute ((TimerParam) param, action);
                    }
                }, new TimerParam () { Item_Guid = Guid.NewGuid (), Repeat_EXE = repeat_exe }, 0, 1000)) {

                    Thread.Sleep (Timeout.Infinite);
                }
            });
        }

        /// <summary>
        /// 每年执行
        /// </summary>
        /// <param name="action">执行的方法</param>
        /// <param name="MM">每年月</param>
        /// <param name="dd">每年日</param>
        /// <param name="hh">每年时</param>
        /// <param name="mm">每年分</param>
        /// <param name="ss">每年秒</param>
        /// <param name="repeat_exe">当上一次未执行完成时，是否开始一次新的执行。</param>
        public static void ExecuteEverYear (Action action, int MM, int dd, int hh, int mm, int ss, bool repeat_exe = false) {
            Task.Factory.StartNew (() => {
                using (new Timer (param => {
                    DateTime date = DateTime.Now;
                    DateTime exe_date = new DateTime (date.Year, MM, dd, hh, mm, ss);
                    if (date.ToTimeStamp () == exe_date.ToTimeStamp ()) {
                        Execute ((TimerParam) param, action);
                    }
                }, new TimerParam () { Item_Guid = Guid.NewGuid (), Repeat_EXE = repeat_exe }, 0, 1000)) {
                    Thread.Sleep (Timeout.Infinite);
                }
            });
        }
    }
}