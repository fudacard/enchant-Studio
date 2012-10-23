using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Sgry.Azuki;
using Sgry.Azuki.Highlighter;
using Sgry.Azuki.WinForms;
using System.Text;

namespace enchantStudio
{
    public  partial class Form1 
    {

        /// <summary>
        /// 設定を読み込みます。
        /// なかったら作ってくれます。
        /// 出来なかったら何もしません。
        /// </summary>
        /// <returns></returns>
        private ESConfig LoadConfig()
        {
            ESConfig ret = new ESConfig();
            XmlSerializer sel = new XmlSerializer(typeof(ESConfig));

            if (!File.Exists("Data/Config.xml"))
            {
                SaveConfig(ret);
            }
            else
            {
                FileStream str = new FileStream("Data/Config.xml", FileMode.Open);
                ret = (ESConfig)sel.Deserialize(str);
                str.Close();
            }
            return ret;
        }


        /// <summary>
        /// 設定を保存します。
        /// </summary>
        /// <param name="setc"></param>
        private void SaveConfig(ESConfig setc)
        {
            XmlSerializer sel = new XmlSerializer(typeof(ESConfig));
            FileStream str = new FileStream("Data/Config.xml", FileMode.Create);
            sel.Serialize(str, setc);
            str.Close();
            

        }
    }
}
