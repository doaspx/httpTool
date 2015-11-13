using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hl_17xy_cn
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VerifyNumber { get; set; }
        public string Url { get; set; }
        public string Gid { get; set; }
        public string Act { get; set; }
        public string Verify { get; set; }

        public List<ServerModel> Servers { get; set; }
        public ServerModel SelectedServer { get; set; }

        public LoginModel()
        {
            Servers = new List<ServerModel>();
        }
    }
}
