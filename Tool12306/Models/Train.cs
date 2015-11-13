using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool12306.Models
{
    public class TrainModel
    {
        /// <summary>
        /// 列车号T9,K518....
        /// </summary>
        public string station_train_code { get; set; }
        /// <summary>
        /// 暂时没用
        /// </summary>
        public string seattype_num { get; set; }
        /// <summary>
        /// 出发地 编号BXP(北京) 
        /// </summary>
        public string from_station_telecode { get; set; }
        /// <summary>
        /// 到站 编号(重庆)
        /// </summary>
        public string to_station_telecode { get; set; }
        /// <summary>
        /// 是否包涵学生票  00不包含
        /// </summary>
        public string include_student { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string from_station_telecode_name { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string to_station_telecode_name { get; set; }
        /// <summary>
        /// 订票日期
        /// </summary>
        public string round_train_date { get; set; }
        /// <summary>
        /// 订票选择的时间范围 00:00-24:00
        /// </summary>
        public string round_start_time_str { get; set; }
        /// <summary>
        /// 单程票1
        /// </summary>
        public string single_round_type { get; set; }
        /// <summary>
        /// 列车类型QB（全部）
        /// </summary>
        public string train_pass_type { get; set; }
        /// <summary>
        /// QB#T#D#
        /// </summary>
        public string train_class_arr { get; set; }
        /// <summary>
        /// 00:00--24:00
        /// </summary>
        public string start_time_str { get; set; }
        /// <summary>
        /// 历史多少时间
        /// </summary>
        public string lishi { get; set; }
        public string train_start_time { get; set; }
        /// <summary>
        /// 列车号24000000T90H
        /// </summary>
        public string trainno4 { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public string arrive_time { get; set; }
        /// <summary>
        /// 出发站
        /// </summary>
        public string from_station_name { get; set; }
        /// <summary>
        /// 到达站
        /// </summary>
        public string to_station_name { get; set; }

        public TrainModel(
            string station_train_code,
            string seattype_num,
            string from_station_telecode,
            string to_station_telecode,
            string include_student,
            string from_station_telecode_name,
            string to_station_telecode_name,
            string round_train_date,
            string round_start_time_str,
            string single_round_type,
            string train_pass_type,
            string train_class_arr,
            string start_time_str,
            string lishi,
            string train_start_time,
            string trainno4,
            string arrive_time,
            string from_station_name,
            string to_station_name)
        {
            this.station_train_code = station_train_code;
            this.seattype_num = seattype_num;
            this.from_station_telecode = from_station_telecode;
            this.to_station_telecode = to_station_telecode;
            this.include_student = include_student;
            this.from_station_telecode_name = from_station_telecode_name;
            this.to_station_telecode_name = to_station_telecode_name;
            this.round_train_date = round_train_date;
            this.round_start_time_str = round_start_time_str;
            this.single_round_type = single_round_type;
            this.train_pass_type = train_pass_type;
            this.train_class_arr = train_class_arr;
            this.start_time_str = start_time_str;
            this.lishi = lishi;
            this.train_start_time = train_start_time;
            this.trainno4 = trainno4;
            this.arrive_time = arrive_time;
            this.from_station_name = from_station_name;
            this.to_station_name = to_station_name;
        }
    }

    public static class Trains
    {
        private static TrainModel[] _data = new TrainModel[]{
            new TrainModel(
                "T9",
                "",
                "BXP",
                "CUW",
                "00",
                "北京",
                "重庆",
                "2012-10-23",
                "00:00--24:00",
                "1",
                "QB",
                "QB#D#Z#T#K#QT#",
                "00:00--24:00",
                "24:25:00",
                "15:20",
                "24000000T90H",
                "15:45",
                "北京西",
                "重庆北")
        };
        private static TrainType[] _types = new TrainType[]{
            new TrainType("QB", "全部"),
            new TrainType("D", "D字头"),
            new TrainType("Z", "Z字头"),
            new TrainType("T", "T字头"),
            new TrainType("K", "K字头"),
            new TrainType("QT", "其他")
        };

        public static TrainModel[] Data
        {
            get
            {
                return _data;
            }
        }

        public static TrainType[] Types
        {
            get
            {
                return _types;
            }
        }
    }
}
