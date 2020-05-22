using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace ANSH.Common.IO {
    /// <summary>
    /// 对文件类操作
    /// </summary>
    public class FilesZip {

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceDirectory">来源目录</param>
        /// <param name="objectiveFileInfo">目的文件.zip</param>
        /// <param name="autoDeleteObjective">是否自动删除目的</param>
        /// <param name="autoDeleteSource">是否自动删除来源目录</param>
        /// <returns></returns>
        public static byte[] Compress (DirectoryInfo sourceDirectory, FileInfo objectiveFileInfo, bool autoDeleteObjective, bool autoDeleteSource) {

            if (!Directory.Exists (sourceDirectory.FullName)) {
                Directory.CreateDirectory (sourceDirectory.FullName);
            }

            if (File.Exists (objectiveFileInfo.FullName)) {
                File.Delete (objectiveFileInfo.FullName);
            }

            System.IO.Compression.ZipFile.CreateFromDirectory (sourceDirectory.FullName, objectiveFileInfo.FullName);

            byte[] result = null;
            using (var stream = objectiveFileInfo.OpenRead ()) {
                result = stream.ToByte ();
            }

            if (autoDeleteSource) {
                Directory.Delete (sourceDirectory.FullName);
            }

            if (autoDeleteObjective) {
                File.Delete (objectiveFileInfo.FullName);
            }
            return result;
        }
    }
}