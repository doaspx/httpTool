using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool12306.Models
{
    public class ConfirmModel
    {
        public string org_apache_struts_taglib_html_TOKEN { get; set; }
        public string leftTicketStr { get; set; }

        public string orderRequest_train_date { get; set; }
        public string orderRequest_train_no { get; set; }
        public string orderRequest_station_train_code { get; set; }
        public string orderRequest_from_station_telecode { get; set; }
        public string orderRequest_to_station_telecode { get; set; }
        public string orderRequest_seat_type_code { get; set; }
        public string orderRequest_seat_detail_type_code { get; set; }
        public string orderRequest_ticket_type_order_num { get; set; }
        public string orderRequest_bed_level_order_num { get; set; }
        public string orderRequest_start_time { get; set; }
        public string orderRequest_end_time { get; set; }
        public string orderRequest_from_station_name { get; set; }
        public string orderRequest_to_station_name { get; set; }
        public string orderRequest_cancel_flag { get; set; }
        public string orderRequest_id_mode { get; set; }


        public string randCode { get; set; }
        public string orderRequest_reserve_flag { get; set; }

        public ConfirmModel(
            string org_apache_struts_taglib_html_TOKEN,
            string leftTicketStr,
            string randCode,
            string orderRequest_reserve_flag)
        {
            this.org_apache_struts_taglib_html_TOKEN = org_apache_struts_taglib_html_TOKEN;
            this.leftTicketStr = leftTicketStr;
            this.randCode = randCode;
            this.orderRequest_reserve_flag = orderRequest_reserve_flag;
        }
    }
}
