using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool12306.Models
{
    public class City
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public City(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }

    public static class Cities
    {
        private static City[] _cities = new City[]{
            new City("BJP", "北京"),
            new City("CQW", "重庆"),
            new City("JNK", "济南")
        };

        public static City[] Data
        {
            get
            {
                return _cities;
            }
        }
    }
}
