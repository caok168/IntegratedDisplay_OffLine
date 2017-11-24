using System;
using System.Collections.Generic;
using System.Text;

namespace InGraph.Classes
{
    public class OpenDataPackClass
    {
        public int iType;
        /// <summary>
        /// cit波形文件名字(全路径格式)
        /// </summary>
        public string sFileName;
        public string sArrayConfigFile;
        /// <summary>
        /// 与cit波形文件同名的文件，文件后缀为idf
        /// </summary>
        public string sAddFileName;
        /// <summary>
        /// 是否加载波形索引
        /// </summary>
        public bool bIndex;
        public int iIndexID=-1;
        public string sDate;
        /// <summary>
        /// 软件应用模式：0-本地模式；1-网络模式；
        /// </summary>
        public int iAppMode;
    }
}
