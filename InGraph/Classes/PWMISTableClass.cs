using System;
using System.Collections.Generic;
using System.Text;

namespace InGraph.Classes
{
    /// <summary>
    /// 台账信息类。
    /// Inner.idf文件中的台账信息读取之后，存储在台账信息类对象中。
    /// </summary>
    class PWMISTableClass
    {
        /// <summary>
        /// 台账信息中文名称
        /// </summary>
        public string name;
        /// <summary>
        /// 表名称--台账信息在Inner.idf数据库中的表名称
        /// </summary>
        public string table_name;
        /// <summary>
        /// 起始公里标--台账信息在Inner.idf数据库中的列名称
        /// </summary>
        public string startMileage;
        /// <summary>
        /// 结束公里标--台账信息在Inner.idf数据库中的列名称
        /// </summary>
        public string endMileage;
        /// <summary>
        /// 台账信息是否可见
        /// </summary>
        public bool bGraph;
        /// <summary>
        /// 重要的列？
        /// </summary>
        public string sText;
        /// <summary>
        /// 需要显示的列
        /// </summary>
        public string sDisplayField;
    }
}
