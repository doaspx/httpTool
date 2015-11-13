using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool12306.Models
{
    public class TrainInfo
    {
        public string Name { get; set; }
        public string No { get; set; }
        public string Start { get; set; }
        public string StartCode { get; set; }
        public string Arrive { get; set; }
        public string ArriveCode { get; set; }

        public TrainInfo(
            string name,
            string no,
            string start,
            string startcode,
            string arrive,
            string arrivecode)
        {
            Name = name;
            No = no;
            Start = start;
            StartCode = startcode;
            Arrive = arrive;
            ArriveCode = arrivecode;
        }
    }
}
