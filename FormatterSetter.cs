using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlLargeFilePrettyPrint
{
    public class FormatterSetter
    {
        public bool Indent { get; set; }
        public char? IndentChar { get; set; }


        internal void SetOptions(System.Xml.XmlTextWriter writer)
        {
            writer.Formatting = Indent ? Formatting.Indented : Formatting.None;
            if (IndentChar.HasValue)
            {
                writer.IndentChar = IndentChar.Value;
            }
        }
    }
}
