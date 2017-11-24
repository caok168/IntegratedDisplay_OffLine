using System;
using System.Collections.Generic;
using System.Text;
using DataProcess;

namespace InGraph.Classes
{
    /// <summary>
    /// CIT文件信息
    /// </summary>
    class DataInfoClass : LayersClass
    {
        #region 检测日期
        /// <summary>
        /// 检测日期
        /// </summary>
        public string sDate;
        #endregion
        #region 线名
        /// <summary>
        /// 线名
        /// </summary>
        public string sTrackName;
        #endregion
        #region 线名代码---例如：AJJX
        /// <summary>
        /// 线名代码---例如：AJJX
        /// </summary>
        public string sTrackCode;
        #endregion
        #region 线路编号---例如：ZX01
        /// <summary>
        /// 线路编号---例如：ZX01
        /// </summary>
        public string sLineCode;
        #endregion
        #region 检测车
        /// <summary>
        /// 检测车
        /// </summary>
        public string sTrain;
        #endregion
        #region 行别
        /// <summary>
        /// 行别
        /// </summary>
        public string sDir;
        #endregion
        #region 增减里程
        /// <summary>
        /// 增减里程
        /// </summary>
        public string sKmInc;
        #endregion
        #region 运行方向
        /// <summary>
        /// 运行方向
        /// </summary>
        public string sRunDir;
        #endregion
        #region 检测起始时间
        /// <summary>
        /// 检测起始时间
        /// </summary>
        public string sTime;
        #endregion
        #region 通道数量
        /// <summary>
        /// 通道数量
        /// </summary>
        public int iChannelNumber;
        #endregion
        #region 通道原始名
        /// <summary>
        /// 通道原始名
        /// </summary>
        public string[] sChannelsName;
        #endregion
        #region 采样间隔
        /// <summary>
        /// 采样间隔
        /// </summary>
        public int iSmaleRate;
        #endregion
        #region 打开模式0本地,1网络
        /// <summary>
        /// 打开模式0本地,1网络
        /// </summary>
        public int iAppMode = 0;
        #endregion
        #region 显示的通道--通道号的列表
        /// <summary>
        /// 显示的通道--通道号的列表
        /// </summary>
        public List<int> listChannelsVisible;
        #endregion

        #region 通道(采样)数据起始位置在文件中的指针
        /// <summary>
        /// 通道(采样)数据起始位置在文件中的指针
        /// </summary>
        public long lStartPosition;
        #endregion
        #region 通道(采样)数据的结束位置在文件中的指针
        /// <summary>
        /// 通道(采样)数据的结束位置在文件中的指针
        /// </summary>
        public long lEndPosition;
        #endregion

        #region 修正值
        /// <summary>
        /// 修正值
        /// </summary>
        public long lReviseValue;
        #endregion

        #region CIT文件(全路径)
        /// <summary>
        /// CIT文件(全路径)
        /// </summary>
        public string sFilePath;
        #endregion
        #region 配置文件(全路径)
        /// <summary>
        /// 配置文件(全路径)
        /// </summary>
        public string sConfigFilePath;
        #endregion

        #region 与cit文件同名的idf文件
        /// <summary>
        /// 与cit文件同名的idf文件
        /// </summary>
        public string sAddFile;
        #endregion


        #region 文件类型：1轨检、2动力学、3弓网， …
        /// <summary>
        /// 文件类型：1轨检、2动力学、3弓网， …
        /// </summary>
        public int iDataType;
        #endregion


        #region 通道比例
        /// <summary>
        /// 通道比例
        /// </summary>
        public float[] fScale;
        #endregion
        #region 通道基线值(偏移)
        /// <summary>
        /// 通道基线值(偏移)
        /// </summary>
        public float[] fOffset;
        #endregion

        #region 通道数据
        /// <summary>
        /// 通道数据
        /// </summary>
        public List<ChannelsClass> listCC;
        #endregion

        #region 是否索引修正
        /// <summary>
        /// 是否索引修正
        /// </summary>
        public bool bIndex = false;
        #endregion
        #region 索引数据信息
        /// <summary>
        /// 索引数据信息
        /// </summary>
        public List<IndexStaClass> listIC;
        #endregion
        #region 索引ID
        /// <summary>
        /// 索引ID
        /// </summary>
        public int iIndexID = -1;
        #endregion

        #region 是否标注显示
        /// <summary>
        /// 是否标注显示
        /// </summary>
        public bool bLabel = true;
        #endregion
        #region 标注信息
        /// <summary>
        /// 标注信息
        /// </summary>
        public List<LabelInfoClass> listLIC;
        #endregion
        #region 标注ID
        /// <summary>
        /// 标注ID
        /// </summary>
        public int iLabelID = -1;
        #endregion

        #region 无效区段
        /// <summary>
        /// 无效区段
        /// </summary>
        public List<InvalidDataClass> listIDC;
        #endregion

        #region 对应里程
        /// <summary>
        /// 对应里程
        /// </summary>
        public List<WaveMeter> listMeter;
        #endregion

        #region 是否加密数据
        /// <summary>
        /// 是否加密数据
        /// </summary>
        public bool bEncrypt=false;
        #endregion

        //加密数组---没有使用
        public byte[] byteEncrypt;

        #region 是否反转
        /// <summary>
        /// 是否反转
        /// </summary>
        public bool bReverse = false;
        #endregion

        #region 是否是历史层
        /// <summary>
        /// 是否是历史层
        /// </summary>
        public bool bHis = false;
        #endregion

        #region 采样方式:true-距离采样；false-时间采样；
        /// <summary>
        /// 采样方式:true-距离采样；false-时间采样；
        /// </summary>
        public bool bSmaleRate = true;
        #endregion

        #region 案例---20141008
        /// <summary>
        /// 案例
        /// </summary>
        public List<AnalysisInfoClass> listAIC;
        #endregion

    }
}
