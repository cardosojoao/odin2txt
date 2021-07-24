using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace odin2txt
{
    public class StringOptions
    {
        public StringOptions()
        {
            option[0] = option[1] = "";
        }
        public StringOptions(string str1)
        {
            option[0] = str1;
            option[1] = "";
        }
        public StringOptions(string str1, string str2)
        {
            option[0] = str1;
            option[1] = str2;
        }

        public string[] option = new string[2];
    }
}
