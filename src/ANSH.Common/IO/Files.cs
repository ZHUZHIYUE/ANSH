using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ANSH.Common.IO
{
    /// <summary>
    /// 对文件类操作
    /// </summary>
    public class Files
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">保存文件夹路径</param>
        /// <param name="file">文件名
        /// <para>包含扩展名</para>
        /// </param>
        /// <param name="value">文件内容</param>
        public static void Save(string path, string file, string value)
        {
            path = path.Trim('/', '\\');
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            using (StreamWriter writer = System.IO.File.AppendText($"{path}/{file}"))
            {
                writer.Write(value);
            }
        }
    }
}
