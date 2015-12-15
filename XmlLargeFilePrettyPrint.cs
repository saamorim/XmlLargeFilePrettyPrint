using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XmlLargeFilePrettyPrint
{
    public class XmlLargeFilePrettyPrint
    {
        private StreamReader inputStream;
        private TextWriter outputStream;
        private XmlTextWriter writer;
        private FormatterSetter formatterSetter;

        public XmlLargeFilePrettyPrint(FormatterSetter formatterSetter)
        {
            this.formatterSetter = formatterSetter;
        }

        public void PrettyPrint(StreamReader inputStream, TextWriter outputStream)
        {
            this.inputStream = inputStream;
            this.outputStream = outputStream;

            EnsureMinimumWriter();

            using (XmlReader xmlReader = new XmlTextReader(inputStream))
            {
                TreatInputDocument(xmlReader);
            }

        }

        private void TreatInputDocument(XmlReader xmlReader)
        {
            while (xmlReader.Read())
            {
                writer = TreatNode(writer, xmlReader);
            }
        }

        private XmlTextWriter TreatNode(XmlTextWriter writer, XmlReader node)
        {

            switch (node.NodeType)
            {
                case XmlNodeType.None:
                    throw new InvalidOperationException();
                case XmlNodeType.Element:
                    if (node.IsEmptyElement)
                    {
                        writer.WriteStartElement(node.Name);
                        writer.WriteAttributes(node, false);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteStartElement(node.Name);
                        writer.WriteAttributes(node, false);
                    }
                    break;

                case XmlNodeType.Attribute:
                    writer.WriteAttributeString(node.Name, node.Value);
                    break;
                case XmlNodeType.Text:
                    writer.WriteValue(node.Value);
                    break;
                case XmlNodeType.CDATA:
                    writer.WriteCData(node.Value);
                    break;
                case XmlNodeType.EntityReference:
                    writer.WriteEntityRef(node.Value);
                    throw new InvalidOperationException();
                case XmlNodeType.Entity:
                    writer.WriteValue(node.Value);
                    throw new InvalidOperationException();
                case XmlNodeType.ProcessingInstruction:
                    writer.WriteProcessingInstruction(node.Name, node.Value);
                    throw new InvalidOperationException();
                case XmlNodeType.Comment:
                    writer.WriteComment(node.Value);
                    break;
                case XmlNodeType.Document:
                    throw new InvalidOperationException();
                case XmlNodeType.DocumentType:
                    throw new InvalidOperationException();
                case XmlNodeType.DocumentFragment:
                    throw new InvalidOperationException();
                case XmlNodeType.Notation:
                    throw new InvalidOperationException();
                case XmlNodeType.Whitespace:
                    //writer.WriteWhitespace(node.Value);
                    break;
                case XmlNodeType.SignificantWhitespace:
                    throw new InvalidOperationException();
                case XmlNodeType.EndElement:
                    writer.WriteEndElement();
                    break;
                case XmlNodeType.EndEntity:
                    throw new InvalidOperationException();
                case XmlNodeType.XmlDeclaration:

                    CreateCorrectWriter(node);

                    break;
            }
            return writer;
        }

        private XmlTextWriter CreateCorrectWriter(XmlReader inputDeclarationNode)
        {
            string xmlEncoding = inputDeclarationNode.GetAttribute("encoding") ?? Encoding.UTF8.WebName;
            writer = new XmlTextWriter(outputStream);

            SetWriterOptions();

            WriteDeclarationNode(inputDeclarationNode);

            return writer;
        }

        private void WriteDeclarationNode(XmlReader inputDeclarationNode)
        {
            string standaloneRaw = inputDeclarationNode.GetAttribute("standalone");
            bool standalone = (standaloneRaw ?? "no") == "yes";
            writer.WriteStartDocument(standalone);
            writer.WriteWhitespace(Environment.NewLine);
        }


        private void EnsureMinimumWriter()
        {

            writer = new XmlTextWriter(outputStream);
            SetWriterOptions();
        }

        private void SetWriterOptions()
        {
            formatterSetter.SetOptions(writer);
        }

    }
}
