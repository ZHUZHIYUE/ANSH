using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ANSH.Common.IO
{
    /// <summary>
    /// 对CSV文件类操作
    /// </summary>
    public class CSV
    {
        /// <summary>
        /// 读取CSV文件内容
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>返回读取的内容</returns>
        public static List<List<string>> ReadCSV(string path)
        {
            using (Stream stream = System.IO.File.OpenRead(path))
            {
                return ReadCSV(stream);
            }
        }

        /// <summary>
        /// 读取CSV文件内容
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="encoding">以指定编码方式读取</param>
        /// <returns>返回读取的内容</returns>
        public static List<List<string>> ReadCSV(string path, Encoding encoding)
        {
            using (Stream stream = System.IO.File.OpenRead(path))
            {
                return ReadCSV(stream, encoding);
            }
        }

        /// <summary>
        /// 读取CSV文件内容
        /// </summary>
        /// <param name="stream">CSV文件流</param>
        /// <returns>返回读取的内容</returns>
        public static List<List<string>> ReadCSV(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            var encode = bytes.GetEncode();

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return ReadCSV(ms, encode);
            }
        }

        /// <summary>
        /// 读取CSV文件内容
        /// </summary>
        /// <param name="stream">CSV文件流</param>
        /// <param name="encoding">以指定编码方式读取</param>
        /// <returns>返回读取的内容</returns>
        public static List<List<string>> ReadCSV(Stream stream, Encoding encoding)
        {
            List<List<string>> result = null;
            using (System.IO.StreamReader reader = new StreamReader(stream, encoding))
            {
                string line;
                List<string> group_line = new List<string>();
                while ((line = reader.ReadLine()) != null)
                {
                    result = result ?? new List<List<string>>();
                    var group = line.Split(new char[] { ',' }, StringSplitOptions.None);
                    string group_item = "";
                    //该列是否读完
                    bool readed = true;
                    group_line = new List<string>();

                    foreach (var in_group in group)
                    {
                        if (!readed)
                        {
                            if (in_group.EndsWith("\""))
                            {
                                group_item += in_group;
                                group_line.Add(group_item);
                                readed = true;
                            }
                            else
                            {
                                group_item += in_group + ",";
                            }
                        }
                        else
                        {
                            if (in_group.StartsWith("\""))
                            {
                                group_item = in_group + ",";
                                if (!in_group.EndsWith("\""))
                                {
                                    readed = false;
                                }
                            }
                            else
                            {
                                group_line.Add(in_group);
                            }
                        }
                    }
                    result.Add(group_line);
                }
            }
            return result;
        }
    }
}
