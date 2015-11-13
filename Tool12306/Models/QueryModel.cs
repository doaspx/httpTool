using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool12306.Models
{
    public class QueryModel
    {
        public string method { get; set; }
        /// <summary>
        /// 查询时输入的发车日期
        /// </summary>
        public string orderRequest_train_date { get; set; }

        public string orderRequest_from_station_telecode { get; set; } //BJP
        public string orderRequest_to_station_telecode { get; set; } //CQW
        /// <summary>
        /// 查询时输入的出发地名称
        /// </summary>
        public string from_station_telecode_name { get; set; }
        /// <summary>
        /// 查询时输入的到达地名称
        /// </summary>
        public string to_station_telecode_name { get; set; }
        public string orderRequest_train_no { get; set; }
        /// <summary>
        /// 查询途经的类型，始发还是路过
        /// </summary>
        public string trainPassType { get; set; } //QB
        /// <summary>
        /// 查询列车的类型qb#D#T#Z#
        /// </summary>
        public string trainClass { get; set; } //QB%23D%23Z%23T%23K%23QT
        /// <summary>
        /// 是否包含学生票 00 不包含
        /// </summary>
        public string includeStudent { get; set; }
        public string seatTypeAndNum { get; set; }
        /// <summary>
        /// 预订票的时间范围00:00-24:00
        /// </summary>
        public string orderRequest_start_time_str { get; set; }
            //includeStudent=00&seatTypeAndNum=&orderRequest.start_time_str=00%3A00--24%3A00

        public QueryModel()
        {
            method = "queryLeftTicket";
            trainPassType = "QB";
            trainClass = "QB%23D%23Z%23T%23K%23QT%23";// "QB#D#Z#T#K#QT#";
            includeStudent = "00";
            orderRequest_start_time_str = "00:00--24:00";
        }
    }
}
