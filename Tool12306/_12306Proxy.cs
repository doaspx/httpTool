using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using WPF_hl_17xy_cn;
using Tool12306.Models;
using System.Xml;

namespace Tool12306
{
    public class _12306Proxy
    {
        private HttpClient _client;

        public _12306Proxy(HttpClient client)
        {
            _client = client;
        }

        public LoginModel GetLoginModel()
        {
            string content = _client.Get("https://dynamic.12306.cn/otsweb/loginAction.do?method=init");

            LoginModel model = new LoginModel();
            return model;
        }

        public byte[] GetVerifyImage()
        {
            return _client.GetSslBinary("https://dynamic.12306.cn/otsweb/passCodeAction.do?rand=sjrand");
        }

        public byte[] GetVerifyImage2()
        {
            return _client.GetSslBinary("https://dynamic.12306.cn/otsweb/passCodeAction.do?rand=randp");
        }

        private void GetLoginRand(LoginModel model)
        {
            string content = _client.Post(
                "https://dynamic.12306.cn/otsweb/loginAction.do?method=loginAysnSuggest",
                "https://dynamic.12306.cn/otsweb/loginAction.do?method=init");
            //{"loginRand":"392","randError":"Y"}

            Regex loginRandReg = new Regex("{\"loginRand\":\"(?<loginRand>\\d+)\"");
            MatchCollection loginRandMatchs = loginRandReg.Matches(content);
            if (loginRandMatchs.Count > 0)
            {
                model.LoginRand = loginRandMatchs[0].Groups["loginRand"].Value;
            }

            model.RefundLogin = "N";
            model.RefundFlag = "Y";
        }

        public bool Login(LoginModel model)
        {
            GetLoginRand(model);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["loginUser.user_name"] = model.UserName;
            parameters["user.password"] = model.Password;
            parameters["randCode"] = model.RandCode;
            parameters["loginRand"] = model.LoginRand;
            parameters["refundFlag"] = model.RefundFlag;
            parameters["refundLogin"] = model.RefundLogin;
            parameters["nameErrorFocus"] = model.NameErrorFocus;
            parameters["passwordErrorFocus"] = model.PasswordErrorFocus;
            parameters["randErrorFocus"] = model.RandErrorFocus;
            string content = _client.Post(
                "https://dynamic.12306.cn/otsweb/loginAction.do?method=login",
                parameters,
                "https://dynamic.12306.cn/otsweb/loginAction.do?method=init");

            //id_jiao_se :  39815
            Regex userIDReg = new Regex("var\\s*isLogin\\s*=\\s*true\\s*;{0,1}\\s*var\\s*u_name\\s*=\\s*'(?<name>[^']*)'");
            MatchCollection userIDMatchs = userIDReg.Matches(content);
            if (userIDMatchs.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void T()
        {
            //https://dynamic.12306.cn/otsweb/loginAction.do?method=login
            _client.Get("https://dynamic.12306.cn/otsweb/");
        }

        [Obsolete("Query已过期")]
        public List<TicketModel> Query(QueryModel model)
        {
            List<TicketModel> tickets = new List<TicketModel>();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["method"] = model.method;
            parameters["orderRequest.train_date"] = model.orderRequest_train_date;
            parameters["orderRequest.from_station_telecode"] = model.orderRequest_from_station_telecode;
            parameters["orderRequest.to_station_telecode"] = model.orderRequest_to_station_telecode;
            parameters["orderRequest.train_no"] = model.orderRequest_train_no;
            parameters["trainPassType"] = model.trainPassType;
            parameters["trainClass"] = model.trainClass;// Uri.EscapeUriString(model.trainClass);
            parameters["includeStudent"] = model.includeStudent;
            parameters["orderRequest.start_time_str"] = Uri.EscapeUriString(model.orderRequest_start_time_str);
            parameters["seatTypeAndNum"] = model.seatTypeAndNum;
            string content = _client.Get(
                "https://dynamic.12306.cn/otsweb/order/querySingleAction.do",
                parameters,
                "https://dynamic.12306.cn/otsweb/loginAction.do?method=init");

            content = content.Replace("&nbsp;", string.Empty);
            /*
             0,
            <span id='id_24000000T90H' class='base_txtdiv' onmouseover=javascript:onStopHover('24000000T90H#BXP#CUW') onmouseout='onStopOut()'>T9</span>,
            <img src='/otsweb/images/tips/first.gif'>北京西<br>15:20,
            <img src='/otsweb/images/tips/last.gif'>重庆北<br>15:45,
            24:25,
            --,
            --,
            --,
            --,
            --,
            <font color='darkgray'>无</font>,
            <font color='darkgray'>无</font>,
            --,
            <font color='darkgray'>无</font>,
            <font color='#008800'>有</font>,
            --,
            <input type='button' class='yuding_u' onmousemove=this.className='yuding_u_over' onmousedown=this.className='yuding_u_down' onmouseout=this.className='yuding_u' onclick=javascript:getSelected('T9#24:25#15:20#24000000T90H#BXP#CUW#15:45#北京西#重庆北#1*****31454*****00001*****00003*****0000#FB3A6C340D81A5FFA714F210095783F4466626E01A20491E5A0A1392') value='预订'></input>
            1,
            <span id='id_240000K5070I' class='base_txtdiv' onmouseover=javascript:onStopHover('240000K5070I#BXP#CUW') onmouseout='onStopOut()'>K507</span>,
            <img src='/otsweb/images/tips/first.gif'>北京西<br>21:45,
            重庆北<br>00:35,
            26:50,
            --,
            --,
            --,
            --,
            --,
            <font color='darkgray'>无</font>,
            <font color='darkgray'>无</font>,
            --,
            <font color='#008800'>有</font>,
            <font color='#008800'>有</font>,
            --,
            <input type='button' class='yuding_u' onmousemove=this.className='yuding_u_over' onmousedown=this.className='yuding_u_down' onmouseout=this.className='yuding_u' onclick=javascript:getSelected('K507#26:50#21:45#240000K5070I#BXP#CUW#00:35#北京西#重庆北#1*****30734*****00001*****04163*****0000#62DF83B44914B925EEA1CE49D2B9200A4FFAA3C922F79D4436EEBE77') value='预订'></input>
             * */

            string[] segments = content.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int index = 1; index < segments.Length; index += 16)
            {
                TicketModel ticket = new TicketModel();
                ticket.StartDate = model.orderRequest_train_date;

                ticket.TrainCode = ParseValue(segments[index + 0], "id='id_(?<value>[^']*)'");
                ticket.TrainName = ParseValue(segments[index + 0], ">(?<value>[^<>]*)<");

                ticket.StartCity = ParseValue(segments[index + 1], "(?<value>[^<>]*)<br");
                ticket.StartTime = ParseValue(segments[index + 1], "<br/>(?<value>[^<>]*)'");

                ticket.ArriveCity = ParseValue(segments[index + 2], "(?<value>[^<>]*)<br");
                ticket.ArriveTime = ParseValue(segments[index + 2], "<br/>(?<value>[^<>]*)'");

                ticket.TotalTime = segments[index + 3];

                ticket.Num_SWZ = ParseNum(segments[index + 4]);
                ticket.Num_TDZ = ParseNum(segments[index + 5]);
                ticket.Num_YDZ = ParseNum(segments[index + 6]);
                ticket.Num_EDZ = ParseNum(segments[index + 7]);
                ticket.Num_GJRW = ParseNum(segments[index + 8]);
                ticket.Num_RW = ParseNum(segments[index + 9]);
                ticket.Num_YW = ParseNum(segments[index + 10]);
                ticket.Num_RZ = ParseNum(segments[index + 11]);
                ticket.Num_YZ = ParseNum(segments[index + 12]);
                ticket.Num_WZ = ParseNum(segments[index + 13]);
                ticket.Num_QT = ParseNum(segments[index + 14]);

                string ticketStr = ParseValue(segments[index + 15], "javascript:getSelected\\('(?<value>[^']*)'\\)");
                string[] ticketInfos = ticketStr.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                if (ticketInfos.Length > 0)
                {
                    /// <summary>
                    /// 0-车号T9
                    /// </summary>
                    ticket.station_train_code = ticketInfos[0];
                    /// <summary>
                    /// 1-历史
                    /// </summary>
                    ticket.lishi = ticketInfos[1];
                    /// <summary>
                    /// 2-发车时间
                    /// </summary>
                    ticket.train_start_time = ticketInfos[2];
                    /// <summary>
                    /// 3-车号24000000T90H
                    /// </summary>
                    ticket.trainno4 = ticketInfos[3];
                    /// <summary>
                    /// 4-发车地编号
                    /// </summary>
                    ticket.from_station_telecode = ticketInfos[4];
                    /// <summary>
                    /// 5-到达地编号
                    /// </summary>
                    ticket.to_station_telecode = ticketInfos[5];
                    /// <summary>
                    /// 6-到达时间
                    /// </summary>
                    ticket.arrive_time = ticketInfos[6];
                    /// <summary>
                    /// 7-出发地名称
                    /// </summary>
                    ticket.from_station_name = ticketInfos[7];
                    /// <summary>
                    /// 8-到达地名称
                    /// </summary>
                    ticket.to_station_name = ticketInfos[8];
                    /// <summary>
                    /// 9-验证码1
                    /// </summary>
                    ticket.ypInfoDetail = ticketInfos[9];
                    /// <summary>
                    /// 10-验证码2
                    /// </summary>
                    ticket.mmStr = ticketInfos[10];
                }

                tickets.Add(ticket);
            }

            return tickets;
        }

        public List<TicketModel> Query2(QueryModel model, out string message)
        {
            message = string.Empty;

            List<TicketModel> tickets = new List<TicketModel>();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["method"] = model.method;
            parameters["orderRequest.train_date"] = model.orderRequest_train_date;
            parameters["orderRequest.from_station_telecode"] = model.orderRequest_from_station_telecode;
            parameters["orderRequest.to_station_telecode"] = model.orderRequest_to_station_telecode;
            parameters["orderRequest.train_no"] = model.orderRequest_train_no;
            parameters["trainPassType"] = model.trainPassType;
            parameters["trainClass"] = Uri.EscapeDataString(model.trainClass);
            parameters["includeStudent"] = model.includeStudent;
            parameters["seatTypeAndNum"] = model.seatTypeAndNum;
            parameters["orderRequest.start_time_str"] = Uri.EscapeDataString(model.orderRequest_start_time_str);
            string content = _client.Get2(
                "https://dynamic.12306.cn/otsweb/order/querySingleAction.do",
                parameters,
                "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=init");

            content = content.Replace("&nbsp;", string.Empty);

            string[] segments = content.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int index = 1; index < segments.Length; index += 16)
            {
                TicketModel ticket = new TicketModel();
                ticket.StartDate = model.orderRequest_train_date;

                ticket.TrainCode = ParseValue(segments[index + 0], "id='id_(?<value>[^']*)'");
                ticket.TrainName = ParseValue(segments[index + 0], ">(?<value>[^<>]*)<");

                ticket.StartCity = ParseValue(segments[index + 1], "(?<value>[^<>]*)<br");
                ticket.StartTime = ParseValue(segments[index + 1], "<br/>(?<value>[^<>]*)'");

                ticket.ArriveCity = ParseValue(segments[index + 2], "(?<value>[^<>]*)<br");
                ticket.ArriveTime = ParseValue(segments[index + 2], "<br/>(?<value>[^<>]*)'");

                ticket.TotalTime = segments[index + 3];

                ticket.Num_SWZ = ParseNum(segments[index + 4]);
                ticket.Num_TDZ = ParseNum(segments[index + 5]);
                ticket.Num_YDZ = ParseNum(segments[index + 6]);
                ticket.Num_EDZ = ParseNum(segments[index + 7]);
                ticket.Num_GJRW = ParseNum(segments[index + 8]);
                ticket.Num_RW = ParseNum(segments[index + 9]);
                ticket.Num_YW = ParseNum(segments[index + 10]);
                ticket.Num_RZ = ParseNum(segments[index + 11]);
                ticket.Num_YZ = ParseNum(segments[index + 12]);
                ticket.Num_WZ = ParseNum(segments[index + 13]);
                ticket.Num_QT = ParseNum(segments[index + 14]);

                string ticketStr = ParseValue(segments[index + 15], "javascript:getSelected\\('(?<value>[^']*)'\\)");
                if (!string.IsNullOrEmpty(ticketStr))
                {
                    string[] ticketInfos = ticketStr.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                    if (ticketInfos.Length > 0)
                    {
                        /// <summary>
                        /// 0-车号T9
                        /// </summary>
                        ticket.station_train_code = ticketInfos[0];
                        /// <summary>
                        /// 1-历史
                        /// </summary>
                        ticket.lishi = ticketInfos[1];
                        /// <summary>
                        /// 2-发车时间
                        /// </summary>
                        ticket.train_start_time = ticketInfos[2];
                        /// <summary>
                        /// 3-车号24000000T90H
                        /// </summary>
                        ticket.trainno4 = ticketInfos[3];
                        /// <summary>
                        /// 4-发车地编号
                        /// </summary>
                        ticket.from_station_telecode = ticketInfos[4];
                        /// <summary>
                        /// 5-到达地编号
                        /// </summary>
                        ticket.to_station_telecode = ticketInfos[5];
                        /// <summary>
                        /// 6-到达时间
                        /// </summary>
                        ticket.arrive_time = ticketInfos[6];
                        /// <summary>
                        /// 7-出发地名称
                        /// </summary>
                        ticket.from_station_name = ticketInfos[7];
                        /// <summary>
                        /// 8-到达地名称
                        /// </summary>
                        ticket.to_station_name = ticketInfos[8];

                        ticket.from_station_no = ticketInfos[9];

                        ticket.to_station_no = ticketInfos[10];

                        /// <summary>
                        /// 9-验证码1
                        /// </summary>
                        ticket.ypInfoDetail = ticketInfos[11];
                        /// <summary>
                        /// 10-验证码2
                        /// </summary>
                        ticket.mmStr = ticketInfos[12];
                        ticket.locationCode = ticketInfos[13];
                    }

                    tickets.Add(ticket);
                }
                else
                {
                    message = ParseValue(content, ">(?<value>[^<>]*)</a>");
                    return null;
                }
            }

            return tickets;
        }

        private string ParseValue(string content, string reg)
        {
            string value = string.Empty;
            Regex userIDReg = new Regex(reg);
            MatchCollection userIDMatchs = userIDReg.Matches(content);
            if (userIDMatchs.Count > 0)
            {
                return userIDMatchs[0].Groups[1].Value;
            }
            return string.Empty;
        }

        private bool ParseNum(string value)
        {
            if (value == "--")
            {
                return false;
            }
            else if (value.Contains("无"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ConfirmModel Submit(TrainModel train, DateTime trainDate, string randCode)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["station_train_code"] = train.station_train_code;
            parameters["train_date"] = trainDate.ToString("yyyy-MM-dd");
            parameters["seattype_num"] = train.seattype_num;
            parameters["from_station_telecode"] = train.from_station_telecode;
            parameters["to_station_telecode"] = train.to_station_telecode;
            parameters["include_student"] = train.include_student;
            parameters["from_station_telecode_name"] = Uri.EscapeUriString(train.from_station_telecode_name);
            parameters["to_station_telecode_name"] = Uri.EscapeUriString(train.to_station_telecode_name);
            parameters["round_train_date"] = DateTime.Now.ToString("yyyy-MM-dd");//Uri.EscapeUriString(train.round_train_date);
            parameters["round_start_time_str"] = Uri.EscapeUriString(train.round_start_time_str);
            parameters["single_round_type"] = train.single_round_type;
            parameters["train_pass_type"] = train.train_pass_type;
            parameters["train_class_arr"] = train.train_class_arr;
            parameters["start_time_str"] = Uri.EscapeUriString(train.start_time_str);
            parameters["lishi"] = Uri.EscapeUriString(train.lishi);
            parameters["train_start_time"] = Uri.EscapeUriString(train.train_start_time);
            parameters["trainno4"] = train.trainno4;
            parameters["arrive_time"] = Uri.EscapeUriString(train.arrive_time);
            parameters["from_station_name"] = Uri.EscapeUriString(train.from_station_name);
            parameters["to_station_name"] = Uri.EscapeUriString(train.to_station_name);
            parameters["ypInfoDetail"] ="1*****31504*****00001*****07083*****0092";
            parameters["mmStr"] = "832AEF635B2CFFDB77B1B4E526182B933ECF213BF2EAA45DAF91EC81";
            //parameters["ypInfoDetail"] ="1*****31964*****00001*****02323*****0000";
            //parameters["mmStr"] = "4344EE8C1729273E16C2FDAA443EB7A007F7604508B040E8687B0182";

            string content = _client.Post(
                "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=submutOrderRequest",
                parameters,
                "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=init");


            //content = _client.Get(
            //    "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init",
            //    "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=init");

            string token = string.Empty;
            string ticketStr = string.Empty;
            Regex tokenReg = new Regex("name=\"org.apache.struts.taglib.html.TOKEN\"[^<>]*value=\"(?<token>[^<>\"]*)");
            MatchCollection tokenMatchs = tokenReg.Matches(content);
            if (tokenMatchs.Count > 0)
            {
                token =  tokenMatchs[0].Groups[1].Value;
            }
            Regex ticketStrReg = new Regex("name=\"leftTicketStr\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)");
            MatchCollection ticketStrMatchs = ticketStrReg.Matches(content);
            if (ticketStrMatchs.Count > 0)
            {
                ticketStr = ticketStrMatchs[0].Groups[1].Value;
            }
            ConfirmModel model = new ConfirmModel(token, ticketStr, randCode, "A");
            /*
            <input type="hidden" name="orderRequest.train_date" value="2012-10-24" id="start_date">
            <input type="hidden" name="orderRequest.train_no" value="24000000T90H" id="train_no">
            <input type="hidden" name="orderRequest.station_train_code" value="T9" id="station_train_code">
            <input type="hidden" name="orderRequest.from_station_telecode" value="BXP" id="from_station_telecode">
            <input type="hidden" name="orderRequest.to_station_telecode" value="CUW" id="to_station_telecode">
            <input type="hidden" name="orderRequest.seat_type_code" value="" id="seat_type_code">
            <input type="hidden" name="orderRequest.seat_detail_type_code" value="" id="seat_detail_type_code">
            <input type="hidden" name="orderRequest.ticket_type_order_num" value="" id="ticket_type_order_num">
            <input type="hidden" name="orderRequest.bed_level_order_num" value="000000000000000000000000000000" id="bed_level_order_num">

            <input type="hidden" name="orderRequest.start_time" value="15:20" id="orderRequest_start_time">

            <input type="hidden" name="orderRequest.end_time" value="15:45" id="orderRequest_end_time">
            <input type="hidden" name="orderRequest.from_station_name" value="北京西" id="orderRequest_from_station_name">
            <input type="hidden" name="orderRequest.to_station_name" value="重庆北" id="orderRequest_to_station_name">
            <input type="hidden" name="orderRequest.cancel_flag" value="1" id="cancel_flag">
            <input type="hidden" name="orderRequest.id_mode" value="Y" id="orderRequest_id_mode">
             */
            Regex reg = new Regex("name=\"orderRequest.train_date\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)");
            MatchCollection matchs = reg.Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_train_date = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.train_no\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_train_no = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.station_train_code\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_station_train_code = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.from_station_telecode\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_from_station_telecode = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.to_station_telecode\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_to_station_telecode = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.seat_type_code\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_seat_type_code = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.seat_detail_type_code\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_seat_detail_type_code = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.ticket_type_order_num\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_ticket_type_order_num = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.bed_level_order_num\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_bed_level_order_num = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.start_time\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_start_time = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.end_time\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_end_time = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.from_station_name\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_from_station_name = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.to_station_name\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_to_station_name = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.cancel_flag\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_cancel_flag = matchs[0].Groups[1].Value;
            }
            matchs = new Regex("name=\"orderRequest.id_mode\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_id_mode = matchs[0].Groups[1].Value;
            }

            //https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=getQueueCount&train_date=2012-11-03&station=T9&seat=3&from=BXP&to=CUW&ticket=1023103150406420000010231007083040500093
            //https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init
            Dictionary<string, string> parameters2 = new Dictionary<string, string>();
            parameters2["method"] = "getQueueCount";
            parameters2["train_date"] = trainDate.ToString("yyyy-MM-dd");
            parameters2["station"] = train.station_train_code;
            parameters2["seat"] = "3";
            parameters2["from"] = train.from_station_telecode;
            parameters2["to"] = train.to_station_telecode;
            parameters2["ticket"] = model.leftTicketStr;
            content = _client.Get(
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do",
                parameters2,
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init");

            return model;
        }

        [Obsolete("使用Submit2,Submit已过期")]
        public ConfirmModel Submit(QueryModel query, TicketModel ticket, string randCode)
        {
            //TrainModel train = null;
            //foreach (TrainModel item in Trains.Data)
            //{
            //    if (item.station_train_code == ticket.TrainName)
            //    {
            //        train = item;
            //        break;
            //    }
            //}
            //if (train == null)
            //{
            //    return null;
            //}

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //车号
            parameters["station_train_code"] = ticket.station_train_code;
            parameters["train_date"] = query.orderRequest_train_date; // 查询时填写的出发日期
            parameters["seattype_num"] = "";// train.seattype_num;
            parameters["from_station_telecode"] = ticket.from_station_telecode; // 票上的起始站编号
            parameters["to_station_telecode"] = ticket.to_station_telecode;     // 票上的到达站编号
            parameters["include_student"] = "00"; // train.include_student;
            parameters["from_station_telecode_name"] = Uri.EscapeUriString(query.from_station_telecode_name); // 查询时输入的起始时间
            parameters["to_station_telecode_name"] = Uri.EscapeUriString(query.to_station_telecode_name); // 查询时输入的到达站名称
            parameters["round_train_date"] = DateTime.Now.ToString("yyyy-MM-dd"); //查询时返程票的查询时间，默认当前时间加一天
            parameters["round_start_time_str"] = Uri.EscapeUriString("00:00-24:00"); // 查询时的返程票查询时间范围
            parameters["single_round_type"] = "1"; // 查询时的往返类型1单程,2往返
            parameters["train_pass_type"] = query.trainPassType; // 查询时填写的列车经过类型，始发或经过
            parameters["train_class_arr"] = query.trainClass; // 查询时填写的列车类型 动车，特快等
            parameters["start_time_str"] = Uri.EscapeUriString(query.orderRequest_start_time_str); //查询时填写的时间范围
            parameters["lishi"] = Uri.EscapeUriString(ticket.lishi); //票上的历时时间
            parameters["train_start_time"] = Uri.EscapeUriString(ticket.train_start_time); // 票上的出发时间
            parameters["trainno4"] = ticket.trainno4; // 票上的列车系统编号
            parameters["arrive_time"] = Uri.EscapeUriString(ticket.arrive_time); //票上的到达时间
            parameters["from_station_name"] = Uri.EscapeUriString(ticket.from_station_name); // 票上的出发站名称
            parameters["to_station_name"] = Uri.EscapeUriString(ticket.to_station_name); // 票上的到达站名称
            parameters["ypInfoDetail"] = ticket.ypInfoDetail; // 票的验证码
            parameters["mmStr"] = ticket.mmStr; // 票的验证码2
            //parameters["ypInfoDetail"] ="1*****31964*****00001*****02323*****0000";
            //parameters["mmStr"] = "4344EE8C1729273E16C2FDAA443EB7A007F7604508B040E8687B0182";

            string content = _client.Post(
                "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=submutOrderRequest",
                parameters,
                "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=init");


            //content = _client.Get(
            //    "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init",
            //    "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=init");

            string token = string.Empty;
            string ticketStr = string.Empty;
            Regex tokenReg = new Regex("name=\"org.apache.struts.taglib.html.TOKEN\"[^<>]*value=\"(?<token>[^<>\"]*)");
            MatchCollection tokenMatchs = tokenReg.Matches(content);
            if (tokenMatchs.Count > 0)
            {
                token = tokenMatchs[0].Groups[1].Value;
            }
            Regex ticketStrReg = new Regex("name=\"leftTicketStr\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)");
            MatchCollection ticketStrMatchs = ticketStrReg.Matches(content);
            if (ticketStrMatchs.Count > 0)
            {
                ticketStr = ticketStrMatchs[0].Groups[1].Value;
            }
            ConfirmModel model = new ConfirmModel(token, ticketStr, randCode, "A");
            /*
            <input type="hidden" name="orderRequest.train_date" value="2012-10-24" id="start_date">
            <input type="hidden" name="orderRequest.train_no" value="24000000T90H" id="train_no">
            <input type="hidden" name="orderRequest.station_train_code" value="T9" id="station_train_code">
            <input type="hidden" name="orderRequest.from_station_telecode" value="BXP" id="from_station_telecode">
            <input type="hidden" name="orderRequest.to_station_telecode" value="CUW" id="to_station_telecode">
            <input type="hidden" name="orderRequest.seat_type_code" value="" id="seat_type_code">
            <input type="hidden" name="orderRequest.seat_detail_type_code" value="" id="seat_detail_type_code">
            <input type="hidden" name="orderRequest.ticket_type_order_num" value="" id="ticket_type_order_num">
            <input type="hidden" name="orderRequest.bed_level_order_num" value="000000000000000000000000000000" id="bed_level_order_num">

            <input type="hidden" name="orderRequest.start_time" value="15:20" id="orderRequest_start_time">

            <input type="hidden" name="orderRequest.end_time" value="15:45" id="orderRequest_end_time">
            <input type="hidden" name="orderRequest.from_station_name" value="北京西" id="orderRequest_from_station_name">
            <input type="hidden" name="orderRequest.to_station_name" value="重庆北" id="orderRequest_to_station_name">
            <input type="hidden" name="orderRequest.cancel_flag" value="1" id="cancel_flag">
            <input type="hidden" name="orderRequest.id_mode" value="Y" id="orderRequest_id_mode">
             */
            Regex reg = new Regex("name=\"orderRequest.train_date\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)");
            MatchCollection matchs = reg.Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_train_date = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.train_no\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_train_no = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.station_train_code\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_station_train_code = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.from_station_telecode\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_from_station_telecode = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.to_station_telecode\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_to_station_telecode = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.seat_type_code\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_seat_type_code = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.seat_detail_type_code\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_seat_detail_type_code = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.ticket_type_order_num\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_ticket_type_order_num = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.bed_level_order_num\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_bed_level_order_num = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.start_time\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_start_time = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.end_time\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_end_time = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.from_station_name\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_from_station_name = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.to_station_name\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_to_station_name = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.cancel_flag\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_cancel_flag = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
            matchs = new Regex("name=\"orderRequest.id_mode\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
            if (matchs.Count > 0)
            {
                model.orderRequest_id_mode = matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }

            //https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=getQueueCount&train_date=2012-11-03&station=T9&seat=3&from=BXP&to=CUW&ticket=1023103150406420000010231007083040500093
            //https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init
            Dictionary<string, string> parameters2 = new Dictionary<string, string>();
            parameters2["method"] = "getQueueCount";
            parameters2["train_date"] = ticket.StartDate;
            parameters2["station"] = ticket.station_train_code;
            parameters2["seat"] = "3";
            parameters2["from"] = ticket.from_station_telecode;
            parameters2["to"] = ticket.to_station_telecode;
            parameters2["ticket"] = model.leftTicketStr;
            content = _client.Get(
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do",
                parameters2,
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init");

            return model;
        }

        public ConfirmModel Submit2(QueryModel query, TicketModel ticket, string randCode, out string message)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //车号
            parameters["station_train_code"] = ticket.station_train_code;
            parameters["train_date"] = query.orderRequest_train_date; // 查询时填写的出发日期
            parameters["seattype_num"] = "";// train.seattype_num;
            parameters["from_station_telecode"] = ticket.from_station_telecode; // 票上的起始站编号
            parameters["to_station_telecode"] = ticket.to_station_telecode;     // 票上的到达站编号
            parameters["include_student"] = "00"; // train.include_student;
            parameters["from_station_telecode_name"] = Uri.EscapeUriString(query.from_station_telecode_name); // 查询时输入的起始时间
            parameters["to_station_telecode_name"] = Uri.EscapeUriString(query.to_station_telecode_name); // 查询时输入的到达站名称
            parameters["round_train_date"] = DateTime.Now.ToString("yyyy-MM-dd"); //查询时返程票的查询时间，默认当前时间加一天
            parameters["round_start_time_str"] = Uri.EscapeUriString("00:00-24:00"); // 查询时的返程票查询时间范围
            parameters["single_round_type"] = "1"; // 查询时的往返类型1单程,2往返
            parameters["train_pass_type"] = query.trainPassType; // 查询时填写的列车经过类型，始发或经过
            parameters["train_class_arr"] = query.trainClass; // 查询时填写的列车类型 动车，特快等
            parameters["start_time_str"] = Uri.EscapeUriString(query.orderRequest_start_time_str); //查询时填写的时间范围
            parameters["lishi"] = Uri.EscapeUriString(ticket.lishi); //票上的历时时间
            parameters["train_start_time"] = Uri.EscapeUriString(ticket.train_start_time); // 票上的出发时间
            parameters["trainno4"] = ticket.trainno4; // 票上的列车系统编号
            parameters["arrive_time"] = Uri.EscapeUriString(ticket.arrive_time); //票上的到达时间
            parameters["from_station_name"] = Uri.EscapeUriString(ticket.from_station_name); // 票上的出发站名称
            parameters["to_station_name"] = Uri.EscapeUriString(ticket.to_station_name); // 票上的到达站名称
            parameters["from_station_no"] = ticket.from_station_no;
            parameters["to_station_no"] = ticket.to_station_no;
            parameters["ypInfoDetail"] = ticket.ypInfoDetail; // 票的验证码
            parameters["mmStr"] = ticket.mmStr; // 票的验证码2
            parameters["localtionCode"] = ticket.locationCode; // 票的验证码2
            //parameters["ypInfoDetail"] ="1*****31964*****00001*****02323*****0000";
            //parameters["mmStr"] = "4344EE8C1729273E16C2FDAA443EB7A007F7604508B040E8687B0182";

            try
            {
                string content = _client.Post(
                    "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=submutOrderRequest",
                    parameters,
                    "https://dynamic.12306.cn/otsweb/order/querySingleAction.do?method=init");

                string token = string.Empty;
                string ticketStr = string.Empty;
                Regex tokenReg = new Regex("name=\"org.apache.struts.taglib.html.TOKEN\"[^<>]*value=\"(?<token>[^<>\"]*)");
                MatchCollection tokenMatchs = tokenReg.Matches(content);
                if (tokenMatchs.Count > 0)
                {
                    token = tokenMatchs[0].Groups[1].Value;
                }
                Regex ticketStrReg = new Regex("name=\"leftTicketStr\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)");
                MatchCollection ticketStrMatchs = ticketStrReg.Matches(content);
                if (ticketStrMatchs.Count > 0)
                {
                    ticketStr = ticketStrMatchs[0].Groups[1].Value;
                }

                ConfirmModel model = new ConfirmModel(token, ticketStr, randCode, "A");

                MatchCollection messageMatchs = new Regex("var message = \"(?<message>[^<>\"]*)\";").Matches(content);
                if (messageMatchs.Count > 0)
                {
                    message = messageMatchs[0].Groups[1].Value;
                }
                else
                {
                    message = string.Empty;
                }

                Regex reg = new Regex("name=\"orderRequest.train_date\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)");
                MatchCollection matchs = reg.Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_train_date = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.train_no\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_train_no = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.station_train_code\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_station_train_code = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.from_station_telecode\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_from_station_telecode = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.to_station_telecode\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_to_station_telecode = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.seat_type_code\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_seat_type_code = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.seat_detail_type_code\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_seat_detail_type_code = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.ticket_type_order_num\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_ticket_type_order_num = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.bed_level_order_num\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_bed_level_order_num = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.start_time\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_start_time = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.end_time\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_end_time = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.from_station_name\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_from_station_name = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.to_station_name\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_to_station_name = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.cancel_flag\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_cancel_flag = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                matchs = new Regex("name=\"orderRequest.id_mode\"[^<>]*value=\"(?<leftTicketStr>[^<>\"]*)").Matches(content);
                if (matchs.Count > 0)
                {
                    model.orderRequest_id_mode = matchs[0].Groups[1].Value;
                }
                else
                {
                    return null;
                }
                return model;
            }
            catch(Exception ex)
            {
                message = ex.Message;
                return null;
            }
        }

        public bool Confirm(ConfirmModel model, List<PassengerModel> passengers, SeatModel seat)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["org.apache.struts.taglib.html.TOKEN"] = model.org_apache_struts_taglib_html_TOKEN;
            parameters["leftTicketStr"] = model.leftTicketStr;
            parameters["textfield"] = Uri.EscapeUriString("中文或拼音首字母");
            //parameters["checkbox0"]=
            //parameters["checkbox2"]=
            parameters["orderRequest.train_date"] = model.orderRequest_train_date;
            parameters["orderRequest.train_no"] = model.orderRequest_train_no;
            parameters["orderRequest.station_train_code"] = model.orderRequest_station_train_code;
            parameters["orderRequest.from_station_telecode"] = model.orderRequest_from_station_telecode;
            parameters["orderRequest.to_station_telecode"] = model.orderRequest_to_station_telecode;
            parameters["orderRequest.seat_type_code"] = model.orderRequest_seat_type_code;
            parameters["orderRequest.seat_detail_type_code"] = model.orderRequest_seat_detail_type_code;
            parameters["orderRequest.ticket_type_order_num"] = model.orderRequest_ticket_type_order_num;
            parameters["orderRequest.bed_level_order_num"] = model.orderRequest_bed_level_order_num;
            parameters["orderRequest.start_time"] = model.orderRequest_start_time;
            parameters["orderRequest.end_time"] = model.orderRequest_end_time;
            parameters["orderRequest.from_station_name"] = Uri.EscapeUriString(model.orderRequest_from_station_name);
            parameters["orderRequest.to_station_name"] = Uri.EscapeUriString(model.orderRequest_to_station_name);
            parameters["orderRequest.cancel_flag"] = model.orderRequest_cancel_flag;
            parameters["orderRequest.id_mode"] = model.orderRequest_id_mode;

            //passengerTickets
            //oldPassengers
            //passenger_1_seat
            //passenger_1_seat_detail_select
            //passenger_1_seat_detail
            //passenger_1_ticket
            //passenger_1_name
            //passenger_1_cardtype
            //passenger_1_cardno
            //passenger_1_mobileno
            //checkbox9
            //passengerTickets
            //oldPassengers
            //passenger_2_seat
            //passenger_2_seat_detail_select
            //passenger_2_seat_detail
            //passenger_2_ticket
            //passenger_2_name
            //passenger_2_cardtype
            //passenger_2_cardno
            //passenger_2_mobileno
            //checkbox9
            //oldPassengers
            //checkbox9
            //oldPassengers
            //checkbox9
            //oldPassengers
            //checkbox9
            StringBuilder passengerTickets = new StringBuilder();
            for (int index = 0; index < 5; index++)
            {
                if (index < passengers.Count)
                {
                    if (index == 0)
                    {
                        passengerTickets.AppendFormat(
                            "{0},{1},{2},{3},{4},{5},{6},{7}",
                            seat.seat,
                            seat.seat_detail_select,
                            passengers[index].passenger_ticket,
                            Uri.EscapeUriString(passengers[index].passenger_name),
                            passengers[index].passenger_cardtype,
                            passengers[index].passenger_cardno,
                            passengers[index].passenger_mobileno,
                            passengers[index].checkbox9);
                    }
                    else
                    {
                        passengerTickets.AppendFormat(
                            "&passengerTickets={0},{1},{2},{3},{4},{5},{6},{7}",
                            seat.seat,
                            seat.seat_detail_select,
                            passengers[index].passenger_ticket,
                            Uri.EscapeUriString(passengers[index].passenger_name),
                            passengers[index].passenger_cardtype,
                            passengers[index].passenger_cardno,
                            passengers[index].passenger_mobileno,
                            passengers[index].checkbox9);
                    }
                    passengerTickets.AppendFormat(
                        "&oldPassengers={0},{1},{2}",
                        Uri.EscapeUriString(passengers[index].passenger_name),
                        passengers[index].passenger_cardtype,
                        passengers[index].passenger_cardno);
                    passengerTickets.AppendFormat(
                        "&passenger_{0}_seat={1}&passenger_{0}_seat_detail_select={2}&passenger_{0}_seat_detail={3}&passenger_{0}_ticket={4}&passenger_{0}_name={5}&passenger_{0}_cardtype={6}&passenger_{0}_cardno={7}&passenger_{0}_mobileno={8}&checkbox9={9}",
                        index+1,
                        seat.seat,
                        seat.seat_detail_select,
                        seat.seat_detail,
                        passengers[index].passenger_ticket,
                        Uri.EscapeUriString(passengers[index].passenger_name),
                        passengers[index].passenger_cardtype,
                        passengers[index].passenger_cardno,
                        passengers[index].passenger_mobileno,
                        passengers[index].checkbox9);
                }
                else
                {
                    passengerTickets.AppendFormat("&oldPassengers=&checkbox9=Y");
                }
            }
            parameters["passengerTickets"] = passengerTickets.ToString();
            parameters["randCode"] = model.randCode;
            parameters["orderRequest.reserve_flag"] = model.orderRequest_reserve_flag;


            string content = _client.Post(
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=confirmSingleForQueueOrder",
                parameters,
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init");
            if (content.Contains("Y"))
            {
                //{\"errMsg\":\"Y\"}
                return true;
            }
            else
            {
                return false;
            }
        }

        [Obsolete("使用Confirm2,Confirm已过期")]
        public bool Confirm(ConfirmModel model, List<PassengerModel> passengers, SeatModel seat, out string message)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["org.apache.struts.taglib.html.TOKEN"] = model.org_apache_struts_taglib_html_TOKEN;
            parameters["leftTicketStr"] = model.leftTicketStr;
            parameters["textfield"] = Uri.EscapeUriString("中文或拼音首字母");
            //parameters["checkbox0"]=
            //parameters["checkbox2"]=
            parameters["orderRequest.train_date"] = model.orderRequest_train_date;
            parameters["orderRequest.train_no"] = model.orderRequest_train_no;
            parameters["orderRequest.station_train_code"] = model.orderRequest_station_train_code;
            parameters["orderRequest.from_station_telecode"] = model.orderRequest_from_station_telecode;
            parameters["orderRequest.to_station_telecode"] = model.orderRequest_to_station_telecode;
            parameters["orderRequest.seat_type_code"] = model.orderRequest_seat_type_code;
            parameters["orderRequest.seat_detail_type_code"] = model.orderRequest_seat_detail_type_code;
            parameters["orderRequest.ticket_type_order_num"] = model.orderRequest_ticket_type_order_num;
            parameters["orderRequest.bed_level_order_num"] = model.orderRequest_bed_level_order_num;
            parameters["orderRequest.start_time"] = model.orderRequest_start_time;
            parameters["orderRequest.end_time"] = model.orderRequest_end_time;
            parameters["orderRequest.from_station_name"] = Uri.EscapeUriString(model.orderRequest_from_station_name);
            parameters["orderRequest.to_station_name"] = Uri.EscapeUriString(model.orderRequest_to_station_name);
            parameters["orderRequest.cancel_flag"] = model.orderRequest_cancel_flag;
            parameters["orderRequest.id_mode"] = model.orderRequest_id_mode;

            //passengerTickets
            //oldPassengers
            //passenger_1_seat
            //passenger_1_seat_detail_select
            //passenger_1_seat_detail
            //passenger_1_ticket
            //passenger_1_name
            //passenger_1_cardtype
            //passenger_1_cardno
            //passenger_1_mobileno
            //checkbox9
            //passengerTickets
            //oldPassengers
            //passenger_2_seat
            //passenger_2_seat_detail_select
            //passenger_2_seat_detail
            //passenger_2_ticket
            //passenger_2_name
            //passenger_2_cardtype
            //passenger_2_cardno
            //passenger_2_mobileno
            //checkbox9
            //oldPassengers
            //checkbox9
            //oldPassengers
            //checkbox9
            //oldPassengers
            //checkbox9
            StringBuilder passengerTickets = new StringBuilder();
            for (int index = 0; index < 5; index++)
            {
                if (index < passengers.Count)
                {
                    if (index == 0)
                    {
                        passengerTickets.AppendFormat(
                            "{0},{1},{2},{3},{4},{5},{6},{7}",
                            seat.seat,
                            seat.seat_detail_select,
                            passengers[index].passenger_ticket,
                            Uri.EscapeUriString(passengers[index].passenger_name),
                            passengers[index].passenger_cardtype,
                            passengers[index].passenger_cardno,
                            passengers[index].passenger_mobileno,
                            passengers[index].checkbox9);
                    }
                    else
                    {
                        passengerTickets.AppendFormat(
                            "&passengerTickets={0},{1},{2},{3},{4},{5},{6},{7}",
                            seat.seat,
                            seat.seat_detail_select,
                            passengers[index].passenger_ticket,
                            Uri.EscapeUriString(passengers[index].passenger_name),
                            passengers[index].passenger_cardtype,
                            passengers[index].passenger_cardno,
                            passengers[index].passenger_mobileno,
                            passengers[index].checkbox9);
                    }
                    passengerTickets.AppendFormat(
                        "&oldPassengers={0},{1},{2}",
                        Uri.EscapeUriString(passengers[index].passenger_name),
                        passengers[index].passenger_cardtype,
                        passengers[index].passenger_cardno);
                    passengerTickets.AppendFormat(
                        "&passenger_{0}_seat={1}&passenger_{0}_seat_detail_select={2}&passenger_{0}_seat_detail={3}&passenger_{0}_ticket={4}&passenger_{0}_name={5}&passenger_{0}_cardtype={6}&passenger_{0}_cardno={7}&passenger_{0}_mobileno={8}&checkbox9={9}",
                        index + 1,
                        seat.seat,
                        seat.seat_detail_select,
                        seat.seat_detail,
                        passengers[index].passenger_ticket,
                        Uri.EscapeUriString(passengers[index].passenger_name),
                        passengers[index].passenger_cardtype,
                        passengers[index].passenger_cardno,
                        passengers[index].passenger_mobileno,
                        passengers[index].checkbox9);
                }
                else
                {
                    passengerTickets.AppendFormat("&oldPassengers=&checkbox9=Y");
                }
            }
            parameters["passengerTickets"] = passengerTickets.ToString();
            parameters["randCode"] = model.randCode;
            parameters["orderRequest.reserve_flag"] = model.orderRequest_reserve_flag;


            string content = _client.Post(
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=confirmSingleForQueueOrder",
                parameters,
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init");
            if (content.Contains("Y"))
            {
                //{\"errMsg\":\"Y\"}
                message = content;
                return true;
            }
            else
            {
                message = content;
                return false;
            }
        }

        public bool Confirm2(ConfirmModel model, List<PassengerModel> passengers, SeatModel seat, out string message)
        {
            if (!Confirm2_1(model, passengers, seat, out message))
            {
                return false;
            }
            if (!Confirm2_2(model, passengers, seat, out message))
            {
                return false;
            }

            return Confirm2_3(model, passengers, seat, out message);
        }

        private bool Confirm2_1(ConfirmModel model, List<PassengerModel> passengers, SeatModel seat, out string message)
        {
            #region https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=checkOrderInfo&rand=x9he

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["org.apache.struts.taglib.html.TOKEN"] = model.org_apache_struts_taglib_html_TOKEN;
            parameters["leftTicketStr"] = model.leftTicketStr;
            parameters["textfield"] = Uri.EscapeUriString("中文或拼音首字母");
            //parameters["checkbox0"]=
            //parameters["checkbox2"]=
            parameters["orderRequest.train_date"] = model.orderRequest_train_date;
            parameters["orderRequest.train_no"] = model.orderRequest_train_no;
            parameters["orderRequest.station_train_code"] = model.orderRequest_station_train_code;
            parameters["orderRequest.from_station_telecode"] = model.orderRequest_from_station_telecode;
            parameters["orderRequest.to_station_telecode"] = model.orderRequest_to_station_telecode;
            parameters["orderRequest.seat_type_code"] = model.orderRequest_seat_type_code;
            parameters["orderRequest.seat_detail_type_code"] = model.orderRequest_seat_detail_type_code;
            parameters["orderRequest.ticket_type_order_num"] = model.orderRequest_ticket_type_order_num;
            parameters["orderRequest.bed_level_order_num"] = model.orderRequest_bed_level_order_num;
            parameters["orderRequest.start_time"] = model.orderRequest_start_time;
            parameters["orderRequest.end_time"] = model.orderRequest_end_time;
            parameters["orderRequest.from_station_name"] = Uri.EscapeUriString(model.orderRequest_from_station_name);
            parameters["orderRequest.to_station_name"] = Uri.EscapeUriString(model.orderRequest_to_station_name);
            parameters["orderRequest.cancel_flag"] = model.orderRequest_cancel_flag;
            parameters["orderRequest.id_mode"] = model.orderRequest_id_mode;

            //passengerTickets
            //oldPassengers
            //passenger_1_seat
            //passenger_1_seat_detail_select
            //passenger_1_seat_detail
            //passenger_1_ticket
            //passenger_1_name
            //passenger_1_cardtype
            //passenger_1_cardno
            //passenger_1_mobileno
            //checkbox9
            //passengerTickets
            //oldPassengers
            //passenger_2_seat
            //passenger_2_seat_detail_select
            //passenger_2_seat_detail
            //passenger_2_ticket
            //passenger_2_name
            //passenger_2_cardtype
            //passenger_2_cardno
            //passenger_2_mobileno
            //checkbox9
            //oldPassengers
            //checkbox9
            //oldPassengers
            //checkbox9
            //oldPassengers
            //checkbox9
            StringBuilder passengerTickets = new StringBuilder();
            for (int index = 0; index < 5; index++)
            {
                if (index < passengers.Count)
                {
                    if (index == 0)
                    {
                        passengerTickets.AppendFormat(
                            "{0},{1},{2},{3},{4},{5},{6},{7}",
                            seat.seat,
                            seat.seat_detail_select,
                            passengers[index].passenger_ticket,
                            Uri.EscapeUriString(passengers[index].passenger_name),
                            passengers[index].passenger_cardtype,
                            passengers[index].passenger_cardno,
                            passengers[index].passenger_mobileno,
                            passengers[index].checkbox9);
                    }
                    else
                    {
                        passengerTickets.AppendFormat(
                            "&passengerTickets={0},{1},{2},{3},{4},{5},{6},{7}",
                            seat.seat,
                            seat.seat_detail_select,
                            passengers[index].passenger_ticket,
                            Uri.EscapeUriString(passengers[index].passenger_name),
                            passengers[index].passenger_cardtype,
                            passengers[index].passenger_cardno,
                            passengers[index].passenger_mobileno,
                            passengers[index].checkbox9);
                    }
                    passengerTickets.AppendFormat(
                        "&oldPassengers={0},{1},{2}",
                        Uri.EscapeUriString(passengers[index].passenger_name),
                        passengers[index].passenger_cardtype,
                        passengers[index].passenger_cardno);
                    passengerTickets.AppendFormat(
                        "&passenger_{0}_seat={1}&passenger_{0}_seat_detail_select={2}&passenger_{0}_seat_detail={3}&passenger_{0}_ticket={4}&passenger_{0}_name={5}&passenger_{0}_cardtype={6}&passenger_{0}_cardno={7}&passenger_{0}_mobileno={8}&checkbox9={9}",
                        index + 1,
                        seat.seat,
                        seat.seat_detail_select,
                        seat.seat_detail,
                        passengers[index].passenger_ticket,
                        Uri.EscapeUriString(passengers[index].passenger_name),
                        passengers[index].passenger_cardtype,
                        passengers[index].passenger_cardno,
                        passengers[index].passenger_mobileno,
                        passengers[index].checkbox9);
                }
                else
                {
                    passengerTickets.AppendFormat("&oldPassengers=&checkbox9=Y");
                }
            }
            parameters["passengerTickets"] = passengerTickets.ToString();
            parameters["randCode"] = model.randCode;
            parameters["orderRequest.reserve_flag"] = model.orderRequest_reserve_flag;
            parameters["tFlag"] ="dc";


            string content = _client.Post(
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=checkOrderInfo&rand=" + model.randCode,
                parameters,
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init");
            if (content.Contains("Y"))
            {
                //{\"errMsg\":\"Y\"}
                message = content;
                return true;
            }
            else
            {
                message = content;
                return false;
            }
            #endregion
        }
        private bool Confirm2_2(ConfirmModel model, List<PassengerModel> passengers, SeatModel seat, out string message)
        {
            #region https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=getQueueCount
              
            //&train_date=2012-12-30&train_no=24000000T90I&station=T9&seat=1&from=BXP&to=CUW
            //&ticket=1023103293406420000010231004493040500000

            Dictionary<string, string> parameters2 = new Dictionary<string, string>();
            parameters2["method"] = "getQueueCount";
            parameters2["train_date"] = model.orderRequest_train_date;
            parameters2["station"] = model.orderRequest_station_train_code;
            parameters2["seat"] = seat.seat;
            parameters2["from"] =model.orderRequest_from_station_telecode;
            parameters2["to"] = model.orderRequest_to_station_telecode;
            parameters2["ticket"] = model.leftTicketStr;
            try
            {
                string content = _client.Get(
                    "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do",
                    parameters2,
                    "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init");
                message = content;
                return true;
            }
            catch(Exception ex)
            {
                message = ex.Message + "\r\n" + ex.StackTrace;
                return false;
            }
            #endregion
        }
        private bool Confirm2_3(ConfirmModel model, List<PassengerModel> passengers, SeatModel seat, out string message)
        {
            #region https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=confirmSingleForQueueOrder
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["org.apache.struts.taglib.html.TOKEN"] = model.org_apache_struts_taglib_html_TOKEN;
            parameters["leftTicketStr"] = model.leftTicketStr;
            parameters["textfield"] = Uri.EscapeUriString("中文或拼音首字母");
            //parameters["checkbox0"]=
            //parameters["checkbox2"]=
            parameters["orderRequest.train_date"] = model.orderRequest_train_date;
            parameters["orderRequest.train_no"] = model.orderRequest_train_no;
            parameters["orderRequest.station_train_code"] = model.orderRequest_station_train_code;
            parameters["orderRequest.from_station_telecode"] = model.orderRequest_from_station_telecode;
            parameters["orderRequest.to_station_telecode"] = model.orderRequest_to_station_telecode;
            parameters["orderRequest.seat_type_code"] = model.orderRequest_seat_type_code;
            parameters["orderRequest.seat_detail_type_code"] = model.orderRequest_seat_detail_type_code;
            parameters["orderRequest.ticket_type_order_num"] = model.orderRequest_ticket_type_order_num;
            parameters["orderRequest.bed_level_order_num"] = model.orderRequest_bed_level_order_num;
            parameters["orderRequest.start_time"] = model.orderRequest_start_time;
            parameters["orderRequest.end_time"] = model.orderRequest_end_time;
            parameters["orderRequest.from_station_name"] = Uri.EscapeUriString(model.orderRequest_from_station_name);
            parameters["orderRequest.to_station_name"] = Uri.EscapeUriString(model.orderRequest_to_station_name);
            parameters["orderRequest.cancel_flag"] = model.orderRequest_cancel_flag;
            parameters["orderRequest.id_mode"] = model.orderRequest_id_mode;

            //passengerTickets
            //oldPassengers
            //passenger_1_seat
            //passenger_1_seat_detail_select
            //passenger_1_seat_detail
            //passenger_1_ticket
            //passenger_1_name
            //passenger_1_cardtype
            //passenger_1_cardno
            //passenger_1_mobileno
            //checkbox9
            //passengerTickets
            //oldPassengers
            //passenger_2_seat
            //passenger_2_seat_detail_select
            //passenger_2_seat_detail
            //passenger_2_ticket
            //passenger_2_name
            //passenger_2_cardtype
            //passenger_2_cardno
            //passenger_2_mobileno
            //checkbox9
            //oldPassengers
            //checkbox9
            //oldPassengers
            //checkbox9
            //oldPassengers
            //checkbox9
            StringBuilder passengerTickets = new StringBuilder();
            for (int index = 0; index < 5; index++)
            {
                if (index < passengers.Count)
                {
                    if (index == 0)
                    {
                        passengerTickets.AppendFormat(
                            "{0},{1},{2},{3},{4},{5},{6},{7}",
                            seat.seat,
                            seat.seat_detail_select,
                            passengers[index].passenger_ticket,
                            Uri.EscapeUriString(passengers[index].passenger_name),
                            passengers[index].passenger_cardtype,
                            passengers[index].passenger_cardno,
                            passengers[index].passenger_mobileno,
                            passengers[index].checkbox9);
                    }
                    else
                    {
                        passengerTickets.AppendFormat(
                            "&passengerTickets={0},{1},{2},{3},{4},{5},{6},{7}",
                            seat.seat,
                            seat.seat_detail_select,
                            passengers[index].passenger_ticket,
                            Uri.EscapeUriString(passengers[index].passenger_name),
                            passengers[index].passenger_cardtype,
                            passengers[index].passenger_cardno,
                            passengers[index].passenger_mobileno,
                            passengers[index].checkbox9);
                    }
                    passengerTickets.AppendFormat(
                        "&oldPassengers={0},{1},{2}",
                        Uri.EscapeUriString(passengers[index].passenger_name),
                        passengers[index].passenger_cardtype,
                        passengers[index].passenger_cardno);
                    passengerTickets.AppendFormat(
                        "&passenger_{0}_seat={1}&passenger_{0}_seat_detail_select={2}&passenger_{0}_seat_detail={3}&passenger_{0}_ticket={4}&passenger_{0}_name={5}&passenger_{0}_cardtype={6}&passenger_{0}_cardno={7}&passenger_{0}_mobileno={8}&checkbox9={9}",
                        index + 1,
                        seat.seat,
                        seat.seat_detail_select,
                        seat.seat_detail,
                        passengers[index].passenger_ticket,
                        Uri.EscapeUriString(passengers[index].passenger_name),
                        passengers[index].passenger_cardtype,
                        passengers[index].passenger_cardno,
                        passengers[index].passenger_mobileno,
                        passengers[index].checkbox9);
                }
                else
                {
                    passengerTickets.AppendFormat("&oldPassengers=&checkbox9=Y");
                }
            }
            parameters["passengerTickets"] = passengerTickets.ToString();
            parameters["randCode"] = model.randCode;
            parameters["orderRequest.reserve_flag"] = model.orderRequest_reserve_flag;


            string content = _client.Post(
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=confirmSingleForQueueOrder",
                parameters,
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init");
            if (content.Contains("Y"))
            {
                //{\"errMsg\":\"Y\"}
                message = content;
                return true;
            }
            else
            {
                message = content;
                return false;
            }
            #endregion
        }

        /// <summary>
        /// 获取配置文件中的乘客信息
        /// </summary>
        /// <returns></returns>
        public List<PassengerModel> GetOfflinePassengers()
        {
            List<PassengerModel> passengers = new List<PassengerModel>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "./datas/passenger.xml");
            XmlNodeList nodes = xmlDoc.SelectNodes(".//item");
            foreach (XmlNode node in nodes)
            {
                PassengerModel pm = new PassengerModel(
                    "1",
                    node.Attributes["name"].Value,
                    node.Attributes["cardtype"].Value,
                    node.Attributes["cardno"].Value,
                    node.Attributes["mobileno"].Value,
                    "Y");
                passengers.Add(pm);
            }
            return passengers;
        }

        /// <summary>
        /// 获取配置文件中的乘客信息
        /// </summary>
        /// <returns></returns>
        public List<PassengerModel> GetPassengers()
        {
            List<PassengerModel> passengers = new List<PassengerModel>();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["pageIndex"] = "0";
            parameters["pageSize"] = "99";
            parameters["passenger_name"] = Uri.EscapeUriString("请输入汉字或拼音首字母");

            string content = _client.Post(
                "https://dynamic.12306.cn/otsweb/passengerAction.do?method=queryPagePassenger",
                parameters,
                "https://dynamic.12306.cn/otsweb/passengerAction.do?method=initUsualPassenger");

            //"passenger_name":"XXX"
            Regex nameReg = new Regex("\"passenger_name\":\"(?<value>[^\"]*)\"");
            MatchCollection nameMatchs = nameReg.Matches(content);
            //"passenger_id_no":"510232196510105021"
            Regex noReg = new Regex("\"passenger_id_no\":\"(?<value>[^\"]*)\"");
            MatchCollection noMatchs = noReg.Matches(content);
            //"mobile_no":"15808090998"
            Regex mobileReg = new Regex("\"mobile_no\":\"(?<value>[^\"]*)\"");
            MatchCollection mobileMatchs = mobileReg.Matches(content);
            Regex noTypeReg = new Regex("\"passenger_id_type_code\":\"(?<value>[^\"]*)\"");
            MatchCollection noTypeMatchs = noTypeReg.Matches(content);
            for (int i = 0; i < nameMatchs.Count; i++)
            {
                PassengerModel pm = new PassengerModel(
                    "1",
                    nameMatchs[i].Groups[1].Value,
                    noTypeMatchs[i].Groups[1].Value,
                    noMatchs[i].Groups[1].Value,
                     mobileMatchs[i].Groups[1].Value,
                    "Y");
                passengers.Add(pm);
            }
            return passengers;
        }

        public List<TrainInfo> GetOfflineTrains()
        {
            List<TrainInfo> trains = new List<TrainInfo>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "./datas/train.xml");
            XmlNodeList nodes = xmlDoc.SelectNodes(".//item");
            foreach (XmlNode node in nodes)
            {
                TrainInfo pm = new TrainInfo(
                    node.Attributes["name"].Value,
                    node.Attributes["no"].Value,
                    node.Attributes["start"].Value,
                    node.Attributes["startcode"].Value,
                    node.Attributes["arrive"].Value,
                    node.Attributes["arrivecode"].Value);
                trains.Add(pm);
            }
            return trains;
        }

        /// <summary>
        /// 确认订单之后需要排队回去订单号
        /// </summary>
        /// <returns></returns>
        public string GetOrderNo(out int waitCount, out string message)
        {
            string content = _client.Get(
                "https://dynamic.12306.cn/otsweb/order/myOrderAction.do?method=getOrderWaitTime&tourFlag=dc",
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init");


            Regex messageReg = new Regex(",\"msg\":\"(?<v>[^<>\"]*)");
            MatchCollection messageMatchs = messageReg.Matches(content);
            if (messageMatchs.Count > 0)
            {
                message = messageMatchs[0].Groups[1].Value;
            }
            else
            {
                message = string.Empty;
            }

            Regex creg = new Regex(",\"waitCount\":(?<v>[\\d]+)");
            MatchCollection cmatchs = creg.Matches(content);
            if (cmatchs.Count > 0)
            {
                waitCount = Convert.ToInt32(cmatchs[0].Groups[1].Value);
            }
            else
            {
                waitCount = -1;
            }

            Regex reg = new Regex("\"orderId\":\"(?<v>[^\"]*)\"");
            MatchCollection matchs = reg.Matches(content);
            if (matchs.Count > 0)
            {
                return matchs[0].Groups[1].Value;
            }
            else
            {
                return null;
            }
        }

        public bool GoPay(string orderNo, ConfirmModel model, List<PassengerModel> passengers, SeatModel seat)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["org.apache.struts.taglib.html.TOKEN"] = model.org_apache_struts_taglib_html_TOKEN;
            parameters["leftTicketStr"] = model.leftTicketStr;
            parameters["textfield"] = Uri.EscapeUriString("中文或拼音首字母");
            //parameters["checkbox0"]=
            //parameters["checkbox2"]=
            parameters["orderRequest.train_date"] = model.orderRequest_train_date;
            parameters["orderRequest.train_no"] = model.orderRequest_train_no;
            parameters["orderRequest.station_train_code"] = model.orderRequest_station_train_code;
            parameters["orderRequest.from_station_telecode"] = model.orderRequest_from_station_telecode;
            parameters["orderRequest.to_station_telecode"] = model.orderRequest_to_station_telecode;
            parameters["orderRequest.seat_type_code"] = model.orderRequest_seat_type_code;
            parameters["orderRequest.seat_detail_type_code"] = model.orderRequest_seat_detail_type_code;
            parameters["orderRequest.ticket_type_order_num"] = model.orderRequest_ticket_type_order_num;
            parameters["orderRequest.bed_level_order_num"] = model.orderRequest_bed_level_order_num;
            parameters["orderRequest.start_time"] = model.orderRequest_start_time;
            parameters["orderRequest.end_time"] = model.orderRequest_end_time;
            parameters["orderRequest.from_station_name"] = Uri.EscapeUriString(model.orderRequest_from_station_name);
            parameters["orderRequest.to_station_name"] = Uri.EscapeUriString(model.orderRequest_to_station_name);
            parameters["orderRequest.cancel_flag"] = model.orderRequest_cancel_flag;
            parameters["orderRequest.id_mode"] = model.orderRequest_id_mode;

            //passengerTickets
            //oldPassengers
            //passenger_1_seat
            //passenger_1_seat_detail_select
            //passenger_1_seat_detail
            //passenger_1_ticket
            //passenger_1_name
            //passenger_1_cardtype
            //passenger_1_cardno
            //passenger_1_mobileno
            //checkbox9
            //passengerTickets
            //oldPassengers
            //passenger_2_seat
            //passenger_2_seat_detail_select
            //passenger_2_seat_detail
            //passenger_2_ticket
            //passenger_2_name
            //passenger_2_cardtype
            //passenger_2_cardno
            //passenger_2_mobileno
            //checkbox9
            //oldPassengers
            //checkbox9
            //oldPassengers
            //checkbox9
            //oldPassengers
            //checkbox9
            StringBuilder passengerTickets = new StringBuilder();
            for (int index = 0; index < 5; index++)
            {
                if (index < passengers.Count)
                {
                    if (index == 0)
                    {
                        passengerTickets.AppendFormat(
                            "{0},{1},{2},{3},{4},{5},{6},{7}",
                            seat.seat,
                            seat.seat_detail_select,
                            passengers[index].passenger_ticket,
                            Uri.EscapeUriString(passengers[index].passenger_name),
                            passengers[index].passenger_cardtype,
                            passengers[index].passenger_cardno,
                            passengers[index].passenger_mobileno,
                            passengers[index].checkbox9);
                    }
                    else
                    {
                        passengerTickets.AppendFormat(
                            "&passengerTickets={0},{1},{2},{3},{4},{5},{6},{7}",
                            seat.seat,
                            seat.seat_detail_select,
                            passengers[index].passenger_ticket,
                            Uri.EscapeUriString(passengers[index].passenger_name),
                            passengers[index].passenger_cardtype,
                            passengers[index].passenger_cardno,
                            passengers[index].passenger_mobileno,
                            passengers[index].checkbox9);
                    }
                    passengerTickets.AppendFormat(
                        "&oldPassengers={0},{1},{2}",
                        Uri.EscapeUriString(passengers[index].passenger_name),
                        passengers[index].passenger_cardtype,
                        passengers[index].passenger_cardno);
                    passengerTickets.AppendFormat(
                        "&passenger_{0}_seat={1}&passenger_{0}_seat_detail_select={2}&passenger_{0}_seat_detail={3}&passenger_{0}_ticket={4}&passenger_{0}_name={5}&passenger_{0}_cardtype={6}&passenger_{0}_cardno={7}&passenger_{0}_mobileno={8}&checkbox9={9}",
                        index + 1,
                        seat.seat,
                        seat.seat_detail_select,
                        seat.seat_detail,
                        passengers[index].passenger_ticket,
                        Uri.EscapeUriString(passengers[index].passenger_name),
                        passengers[index].passenger_cardtype,
                        passengers[index].passenger_cardno,
                        passengers[index].passenger_mobileno,
                        passengers[index].checkbox9);
                }
                else
                {
                    passengerTickets.AppendFormat("&oldPassengers=&checkbox9=Y");
                }
            }
            parameters["passengerTickets"] = passengerTickets.ToString();
            parameters["randCode"] = model.randCode;
            parameters["orderRequest.reserve_flag"] = model.orderRequest_reserve_flag;


            string content = _client.Post(
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=payOrder&orderSequence_no=" + orderNo,
                parameters,
                "https://dynamic.12306.cn/otsweb/order/confirmPassengerAction.do?method=init");
            if (!string.IsNullOrEmpty(content))
            {
                //{\"errMsg\":\"Y\"}
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
