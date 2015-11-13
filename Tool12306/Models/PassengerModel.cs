using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool12306.Models
{
    public class PassengerModel
    {
        public string passenger_ticket { get; set; }
        public string passenger_name { get; set; }
        public string passenger_cardtype { get; set; }
        public string passenger_cardno { get; set; }
        public string passenger_mobileno { get; set; }
        public string checkbox9 { get; set; }

        public PassengerModel(
            string passenger_ticket,
            string passenger_name,
            string passenger_cardtype,
            string passenger_cardno,
            string passenger_mobileno,
            string checkbox9)
        {
            this.passenger_ticket = passenger_ticket;
            this.passenger_name = passenger_name;
            this.passenger_cardtype = passenger_cardtype;
            this.passenger_cardno = passenger_cardno;
            this.passenger_mobileno = passenger_mobileno;
            this.checkbox9 = checkbox9;
        }
    }

    public static class Passengers
    {
        private static PassengerModel[] _data = new PassengerModel[]{
            new PassengerModel("1","刘洋","1","500227198509095010","13552664526","Y")
        };

        public static PassengerModel[] Data
        {
            get
            {
                return _data;
            }
        }
    }
}
