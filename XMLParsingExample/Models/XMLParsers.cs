using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace XMLParsingExample.Models
{
    public static class XMLParsers
    {
        private static string xmlUrl = "http://localhost:7467/Student.xml";

        // Parse the xml using XMLDocument class.
        public static StudentsInformation ParseByXMLDocument()
        {
            var students = new StudentsInformation();

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);

            XmlNode GeneralInformationNode =
                doc.SelectSingleNode("/StudentsInformation/GeneralInformation");
            students.School =
                GeneralInformationNode.SelectSingleNode("School").InnerText;
            students.Department =
                GeneralInformationNode.SelectSingleNode("Department").InnerText;

            XmlNode StudentListNode =
                doc.SelectSingleNode("/StudentsInformation/Studentlist");
            XmlNodeList StudentNodeList =
                StudentListNode.SelectNodes("Student");
            foreach (XmlNode node in StudentNodeList)
            {
                Student aStudent = new Student();
                aStudent.id = Convert.ToInt16(node.Attributes
                    .GetNamedItem("id").Value);
                aStudent.name = node.InnerText;
                aStudent.score = Convert.ToInt16(node.Attributes
                    .GetNamedItem("score").Value);
                aStudent.enrollment =
                    node.Attributes.GetNamedItem("enrollment").Value;
                aStudent.comment =
                    node.Attributes.GetNamedItem("comment").Value;

                students.Studentlist.Add(aStudent);
            }

            return students;
        }

        // Parse the xml using XDocument class.
        public static StudentsInformation ParseByXDocument()
        {
            var students = new StudentsInformation();

            XDocument doc = XDocument.Load(xmlUrl);
            XElement generalElement = doc
                    .Element("StudentsInformation")
                    .Element("GeneralInformation");
            students.School = generalElement.Element("School").Value;
            students.Department = generalElement.Element("Department").Value;

            students.Studentlist = (from c in doc.Descendants("Student")
                           select new Student()
                           {
                               id = Convert.ToInt16(c.Attribute("id").Value),
                               name = c.Value,
                               score = Convert.ToInt16(c.Attribute("score").Value),
                               enrollment = c.Attribute("enrollment").Value,
                               comment = c.Attribute("comment").Value
                           }).ToList<Student>();

            return students;
        }
    }
}
