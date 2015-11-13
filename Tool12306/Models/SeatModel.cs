using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool12306.Models
{
    /// <summary>
    /// 1,0,0-硬座 3,0,0-硬卧
    /// </summary>
    public class SeatModel
    {
        public string caption { get; set; }
        public string seat { get; set; }
        public string seat_detail_select { get; set; }
        public string seat_detail { get; set; }
    }
}
