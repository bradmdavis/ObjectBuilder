using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectBuilder
{
    public class Utility
    {

        public static string RemoveNonAlpha(string x)
        {
            return System.Text.RegularExpressions.Regex.Replace(x, "[^a-zA-Z0-9 ]", string.Empty);
        } 

    }
}
