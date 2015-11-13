using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tool12306.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RandCode { get; set; }
        public string LoginRand { get; set; }
        public string RefundLogin { get; set; }
        public string RefundFlag { get; set; }
        public string NameErrorFocus { get; set; }
        public string PasswordErrorFocus { get; set; }
        public string RandErrorFocus { get; set; }

        //loginRand=392&refundLogin=N&refundFlag=Y&loginUser.user_name=ly.jaeho%40gmail.com&nameErrorFocus=&user.password=xk161087264&passwordErrorFocus=&randCode=fckv&randErrorFocus=
    }
}
