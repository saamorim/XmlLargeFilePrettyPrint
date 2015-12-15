using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlLargeFilePrettyPrint
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader strReader = new StreamReader(args[0]))
            {
                FormatterSetter formatterSetter = new FormatterSetter()
                {
                    Indent = true
                };
                XmlLargeFilePrettyPrint pp = new XmlLargeFilePrettyPrint(formatterSetter);
                pp.PrettyPrint(strReader, Console.Out);
            }

        }
    }
}
