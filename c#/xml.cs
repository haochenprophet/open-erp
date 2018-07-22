using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace n_xml
{
    public class Cxml
    {
        public bool debug = true;
        public XmlDocument xmlDoc = new XmlDocument();
        public string xml_file = "Cxml.xml";
        public string version = "1.0";
        public string encoding = "UTF-8";
        public string s_root = "RootNode";
        public string s_node = "Node";
        public string s_element = "Element";
        public string s_text = "InnerText";
        public string s_attr = "Attribute";
        public string s_attr_value = "attr_value";


        public Cxml(string xml_file=null)
        {
            if (xml_file != null) this.xml_file = xml_file;
        }

        ~Cxml()
        {

        }


        public bool create()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                //Dec
                XmlDeclaration dec = xmlDoc.CreateXmlDeclaration(this.version, this.encoding, null);
                xmlDoc.AppendChild(dec);
                //Root
                XmlElement root = xmlDoc.CreateElement(this.s_root);
                xmlDoc.AppendChild(root);
                //Node 
                XmlNode node = xmlDoc.CreateElement(this.s_node);
                //Element
                XmlElement element = xmlDoc.CreateElement(this.s_element);
                element.InnerText = this.s_text;
                element.SetAttribute(this.s_attr, this.s_attr_value);
                //add
                node.AppendChild(element);
                root.AppendChild(node);
                //save
                xmlDoc.Save(this.xml_file);
                return true;
            }
            catch (Exception e) { if (this.debug) Console.WriteLine(e.ToString()); return false; }
        }

        public void insert()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Books.xml");
            XmlNode root = xmlDoc.SelectSingleNode("Books");
            XmlElement book = xmlDoc.CreateElement("Book");
            XmlElement title = xmlDoc.CreateElement("Title");
            title.InnerText = "XML";
            book.AppendChild(title);
            XmlElement isbn = xmlDoc.CreateElement("ISBN");
            isbn.InnerText = "333333";
            book.AppendChild(isbn);
            XmlElement author = xmlDoc.CreateElement("Author");
            author.InnerText = "snow";
            book.AppendChild(author);
            XmlElement price = xmlDoc.CreateElement("Price");
            price.InnerText = "120";
            price.SetAttribute("Unit", "___FCKpd___0quot");
            book.AppendChild(price);
            root.AppendChild(book);
            xmlDoc.Save("Books.xml");
        }


        public bool select(string xml_file, string descendant, string s_value = null)//descendant style  "descendant::Node[subNode1/SubNode[n]='value']"
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xml_file);

                XmlNodeList nodeList;
                XmlNode root = doc.DocumentElement;

                nodeList = root.SelectNodes(descendant);

                foreach (XmlNode node in nodeList)
                {
                    if (s_value == null) break;
                    node.LastChild.InnerText = s_value;
                }
                //  Console.WriteLine("Display the modified XML document....");
                if (this.debug) doc.Save(Console.Out);
                return true;
            }
            catch (Exception e) { if (this.debug) Console.WriteLine(e.ToString()); return false; }
        }

        public bool update()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Books.xml");
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("Books//Book").ChildNodes;
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Name == "Author")
                {
                    xe.InnerText = "amandag";
                }
                if (xe.GetAttribute("Unit") == "___FCKpd___0quot")
                {
                    xe.SetAttribute("Unit", "гд");
                }
            }
            xmlDoc.Save("Books.xml");
            return true;
        }

        private bool delete()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Books.xml");
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("Books//Book").ChildNodes;
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Name == "Author")
                {
                    xe.RemoveAll();
                }
                if (xe.GetAttribute("Unit") == "гд")
                {
                    xe.RemoveAttribute("Unit");
                }
            }
            xmlDoc.Save("Books.xml");
            return true;
        }


        public  void child(string xml_file)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(xml_file);

            XmlNode lastNode = doc.DocumentElement.LastChild;
            Console.WriteLine("LastChild:"+ lastNode.OuterXml);

            XmlNode prevNode = lastNode.PreviousSibling;
            Console.WriteLine("\r\nPrevious...");
            Console.WriteLine(prevNode.OuterXml);
        }

        public  void display(string xml_file,string tag_name)
        {
            if (!this.debug) return;
            try
            {
                //Create the XmlDocument.
                XmlDocument doc = new XmlDocument();
                doc.Load(xml_file);

                //Display all the tag.
                XmlNodeList elemList = doc.GetElementsByTagName(tag_name);
                for (int i = 0; i < elemList.Count; i++)
                {
                    Console.WriteLine(elemList[i].InnerXml);
                }

            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        public void display_child(string xml_file)
        {
            if (!this.debug) return;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xml_file);
                XmlNode root = doc.FirstChild;

                //Display the contents of the child nodes.
                if (root.HasChildNodes)
                {
                  for (int i = 0; i<root.ChildNodes.Count; i++)
                  {
                    Console.WriteLine(root.ChildNodes[i].InnerText);
                  }
               }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}