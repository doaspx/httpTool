using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hl_17xy_cn
{
    public class ServerModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Index { get; set; }
        public string Caption
        {
            get
            {
                return string.Format("s{0}:{1}", Index, Name);
            }
        }
    }
}
