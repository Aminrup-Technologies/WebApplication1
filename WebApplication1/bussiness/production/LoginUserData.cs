using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.bussiness.production
{
    [Serializable] // Marking the class as serializable for session storage
    public class LoginUserData
    {
        public string State { get; set; }
        public string Value { get; set; }
        public string CompValue { get; set; }
        public string Datalock { get; set; }
    }
}