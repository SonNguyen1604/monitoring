﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestPage.Common
{
    [Serializable]
    public class UserLogin
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string GroupID { get; set; }
    }
}