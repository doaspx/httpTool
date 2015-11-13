using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool12306.Models
{
    public class TrainType
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public TrainType(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
