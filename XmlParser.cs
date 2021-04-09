using System;
using System.Collections.Generic;
using System.Xml;

namespace ConsoleApp3
{
    class XmlParser
    {
        private XmlReader reader;
        private void readNames(XmlReader reader, List<string> names, string endNode)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement() && (reader.Name.ToString()).Equals("name"))
                {
                    reader.ReadStartElement("name");
                    string s = reader.ReadString();
                    char i = '0';
                    if (names.Contains(s))
                    {
                        i++;
                        s += "[";
                        s += i;
                        s += "]";
                    }
                    while (names.Contains(s))
                    {
                        char[] sb = s.ToCharArray();
                        sb[s.Length - 1] = i;
                        s = sb.ToString();
                    }
                    names.Add(s);
                    reader.ReadEndElement();
                    continue;
                }
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == endNode)
                {
                    break;
                }
            }
        }

        //put all the nodes in names
        public void buildingTS(List<string> names, string startNode)
        {
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name.ToString().Equals(startNode))
                    {
                        readNames(reader, names, startNode);
                    }
                }
            }
        }

            
        public XmlParser(string xmlFile)
        {
            try
            {
                reader = XmlReader.Create(xmlFile);
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

    }
}
