using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ANSH.Common.IO {
    /// <summary>
    /// 对文件类操作
    /// </summary>
    public class Files {
        /// <summary>
        /// 向指定文件追加字符串
        /// <para>如果文件不存在，则新建</para>
        /// </summary>
        /// <param name="path">保存文件夹路径</param>
        /// <param name="file">文件名
        /// <para>包含扩展名</para>
        /// </param>
        /// <param name="value">文件内容</param>
        public static void AppendText (string path, string file, string value) {
            AppendTextAsync (path, file, value).Wait ();
        }

        /// <summary>
        /// 保存文件
        /// <para>如果文件不存在，则新建</para>
        /// </summary>
        /// <param name="path">保存文件夹路径</param>
        /// <param name="file">文件名
        /// <para>包含扩展名</para>
        /// </param>
        /// <param name="value">文件内容</param>
        public static async Task AppendTextAsync (string path, string file, string value) {
            CreateDirectory (path);
            using (StreamWriter writer = File.AppendText ($"{path}/{file}")) {
                await writer.WriteAsync (value);
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件内容</param>
        /// <param name="filemodel">文件模式</param>
        public static void Save (string path, string filename, Stream file, FileMode filemodel = FileMode.Create) {
            SaveAsync (path, filename, file, filemodel).Wait ();
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件内容</param>
        /// <param name="filemodel">文件模式</param>
        public static async Task SaveAsync (string path, string filename, Stream file, FileMode filemodel = FileMode.Create) {
            CreateDirectory (path);
            if (file.CanSeek) { file.Seek (0, SeekOrigin.Begin); }
            using (var fileStream = new FileStream ($"{path}/{filename}", filemodel)) {
                await file.CopyToAsync (fileStream);
            }
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">路径</param>
        static void CreateDirectory (string path) {
            path = path.TrimEnd ('/', '\\');
            if (!Directory.Exists (path)) Directory.CreateDirectory (path);
        }
    }
}