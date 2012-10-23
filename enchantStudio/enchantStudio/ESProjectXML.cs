using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace enchantStudio
{
    [XmlRoot("ESProject")]
    public class ESProjectXML
    {
        public ESProjectXML()
        {
            ProjectName = "無題";
            ExecuteHTML = "main.html";
        }

        public string ProjectFileVersion
        {
            get
            {
                return "2.0";
            }
        }

        [XmlElement("ProjectName")]
        public string ProjectName
        {
            get;
            set;
        }

        [XmlElement("ExecuteHTML")]
        public string ExecuteHTML
        {
            get;
            set;
        }
    }
}