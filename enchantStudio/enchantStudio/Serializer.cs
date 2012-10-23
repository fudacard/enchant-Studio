using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace enchantStudio
{
    public class Serializer
    {
        public object LoadObject(Type selt,string file)
        {
            object ret;
            XmlSerializer sel = new XmlSerializer(selt);
            if (File.Exists(file))
            {
                FileStream str = new FileStream(file, FileMode.Open);
                ret = sel.Deserialize(str);
            }
            else
            {
                ret = new object();
            }

            return ret;
        }

        public void SaveObject(Type selt, object obj, string file)
        {
            XmlSerializer sel = new XmlSerializer(selt);
            FileStream str = new FileStream(file, FileMode.Create);
            sel.Serialize(str, obj);
            str.Close();
        }

    }
}
