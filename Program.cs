using System;
using System.Collections.Generic;

namespace odin2txt
{
    class Program
    {
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.Write("Invoke using: odn2txt [[odn_filename> [txt_filename]] [option=-f list_filename]\n");
			}
			else
			{
				ParserOdin2Text parser = new ParserOdin2Text();
				parser.Main(args);
			}
		}
	}
}
