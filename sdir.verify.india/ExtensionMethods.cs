using System;
using System.Collections.Generic;
using System.Text;

namespace sdir.verify.india
{

        public static class MyExtensions
        {
            public static string Clean(this string str)
            {
                return str.Replace("&nbsp;", "").Replace("\n", "").Replace("\t", "").Trim();
            }
        }
    
}
