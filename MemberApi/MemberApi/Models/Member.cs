using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberApi.Models
{
    public class Member
    {
        public string NAME { get; set; }

        public string GENDER { get; set; }
        
        public string BIRTHDAY { get; set; }

        public string EMAIL { get; set; }

        public int PHONE { get; set; }

        public string ADDRESS { get; set; }       
    
    }
}