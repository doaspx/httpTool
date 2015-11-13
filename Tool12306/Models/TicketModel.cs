using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool12306.Models
{
    /// <summary>
    /// T9#24:25#15:20#24000000T90H#BXP#CUW#15:45#北京西#重庆北#1*****31504*****00001*****07103*****0086#69A48BA075C06B6354ECA4C158F89F0C7950787220CCEF91ADE4EF1C
    /// </summary>
    public class TicketModel
    {
        /// <summary>
        /// 发车日期
        /// </summary>
        public string StartDate { get; set; }

        public string TrainCode { get; set; }
        public string TrainName { get; set; }
        public string StartCity { get; set; }
        public string StartTime { get; set; }
        public string ArriveCity { get; set; }
        public string ArriveTime{get;set;}

        public string TotalTime { get; set; }

        public bool Num_SWZ { get; set; }
        public bool Num_TDZ { get; set; }
        public bool Num_YDZ { get; set; }
        public bool Num_EDZ { get; set; }
        public bool Num_GJRW { get; set; }
        public bool Num_RW { get; set; }
        public bool Num_YW { get; set; }
        public bool Num_RZ { get; set; }
        public bool Num_YZ { get; set; }
        public bool Num_WZ { get; set; }
        public bool Num_QT { get; set; }

        /// <summary>
        /// 0-车号T9
        /// </summary>
        public string station_train_code { get; set; }
        /// <summary>
        /// 1-历史
        /// </summary>
        public string lishi { get; set; }
        /// <summary>
        /// 2-发车时间
        /// </summary>
        public string train_start_time { get; set; }
        /// <summary>
        /// 3-车号24000000T90H
        /// </summary>
        public string trainno4 { get; set; }
        /// <summary>
        /// 4-发车地编号
        /// </summary>
        public string from_station_telecode { get; set; }
        /// <summary>
        /// 5-到达地编号
        /// </summary>
        public string to_station_telecode { get; set; }
        /// <summary>
        /// 6-到达时间
        /// </summary>
        public string arrive_time { get; set; }
        /// <summary>
        /// 7-出发地名称
        /// </summary>
        public string from_station_name { get; set; }
        /// <summary>
        /// 8-到达地名称
        /// </summary>
        public string to_station_name { get; set; }

        /// <summary>
        /// 9-出发地数字编号
        /// </summary>
        public string from_station_no { get; set; }

        /// <summary>
        /// 10-到达地数字编号
        /// </summary>
        public string to_station_no { get; set; }

        /// <summary>
        /// 11-验证码1
        /// </summary>
        public string ypInfoDetail { get; set; }
        /// <summary>
        /// 12-验证码2
        /// </summary>
        public string mmStr { get; set; }

        /// <summary>
        /// 13
        /// </summary>
        public string locationCode { get; set; }
    }
}
