using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DBUtility
{
    public static class Helper
    {
        /// <summary>
        /// 得到当前程序集的路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLocalWay(string path)
        {
            Assembly entryAssenbly = Assembly.GetEntryAssembly();
            //  \.. 拿到当前上一层的文件夹
            //string ss = System.IO.Path.GetDirectoryName(entryAssenbly.Location) + "\\..";
            string newPath = System.IO.Path.GetDirectoryName(entryAssenbly.Location) + path;

            return newPath;
        }

        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReaderTxtFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        public static void Save(string path, params string[] datas)
        {
            File.WriteAllLines(path, datas);
        }

        public static string[] Read(string path)
        {
            try
            {
                return File.ReadAllLines(path);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取文件夹包含里面的文件总大小
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns>大小</returns>
        public static long GetDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0; //目录文件大小总和

            //定义一个DirectoryInfo对象 
            DirectoryInfo di = new DirectoryInfo(dirPath);

            //通过GetFiles方法,获取di目录中的所有文件的大小
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            //fileNum += di.GetFiles().Length;

            ////获取di中所有的文件夹,并存到一个新的对象数组中,以进行递归
            //DirectoryInfo[] dis = di.GetDirectories();

            //dirNum += dis.Length;

            //if (dis.Length > 0)
            //{

            //    for (int i = 0; i < dis.Length; i++)
            //    {
            //        len += GetDirectoryLength(dis[i].FullName);
            //    }
            //}
            return len;

        }


    }
}
