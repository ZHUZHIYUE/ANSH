using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ANSH.Common.IO {
    /// <summary>
    /// 日志文件
    /// </summary>
    public class Logs {
        /// <summary>
        /// 物理路径
        /// </summary>
        static string Path => $"{Directory.GetCurrentDirectory()}\\logs";

        /// <summary>
        /// 日志名称
        /// </summary>
        static string File => $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt";

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex">异常描述</param>
        /// <param name="file">文件名默认{DateTime.Now.ToString("yyyy-MM-dd")}.txt</param>{
        /// <param name="path">保存地址默认{Directory.GetCurrentDirectory()}\\logs\\</param>
        public static void Write (Exception ex, string path = null, string file = null) {
            try {
                path = path ?? Path;
                file = file ?? File;
                List<string> log_msg = new List<string> {
                    $"***********{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}************"
                };
                Exception inner_ex = ex;
                List<string> log_item_msg = new List<string> ();
                do {
                    log_item_msg.Add ($"{inner_ex.GetType()}  {inner_ex.Message}");
                }
                while ((inner_ex = inner_ex.InnerException) != null);
                log_item_msg.Reverse ();
                log_msg.AddRange (log_item_msg);
                log_msg.Add ($"{ex.StackTrace}");
                log_msg.Add ("*********************************************\r\n\r\n\r\n");
                Save (log_msg, file, path);
            } catch {
                Write ("记录异常信息时发生的错误", path, file);
            }
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="msg">异常描述</param>
        /// <param name="file">文件名默认{DateTime.Now.ToString("yyyy-MM-dd")}.txt</param>
        /// <param name="path">保存地址默认{Directory.GetCurrentDirectory()}\\logs\\</param>
        public static void Write (string msg, string path = null, string file = null) {
            try {
                path = path ?? Path;
                file = file ?? File;
                List<string> log_msg = new List<string> {
                    $"***********{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}************",
                    $"异常消息：{msg}",
                    "***********本次错误结束************\r\n\r\n\r\n"
                };
                Save (log_msg, file, path);
            } catch {

            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="log_msg">错误信息</param>
        /// <param name="file">文件名默认{DateTime.Now.ToString("yyyy-MM-dd")}.txt</param>
        /// <param name="path">保存地址默认{Directory.GetCurrentDirectory()}\\logs\\</param>
        static void Save (List<string> log_msg, string file, string path) {
            Files.Save (path, file, string.Join ("\r\n", log_msg));
        }

    }
}