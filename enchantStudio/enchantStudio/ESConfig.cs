using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace enchantStudio
{
    public class ESConfig
    {
        public ESConfig() 
        {
            /*
            C_Keyword1 = Color.FromArgb(0, 255, 255);
            C_Keyword2 = Color.FromArgb(0, 255, 0);
            C_String = Color.FromArgb(255, 127, 0);
            C_Comment = Color.FromArgb(0, 127, 0);
            C_Normal = Color.FromArgb(255, 255, 255);
            C_Number = Color.FromArgb(255, 255, 200);
            C_Background = Color.FromArgb(0, 0, 0);
            */
            ProjectPath = "";
            FontName = "ＭＳ ゴシック";
            FontSize = 16;
        }
        /*
        [Category("javascript配色")]
        [Description("javascriptの予約語の色を設定します。")]
        public Color C_Keyword1
        {
            get;
            set;
        }

        [Category("javascript配色")]
        [Description("javascriptのオブジェクトののキーワードの色を設定します。")]
        public Color C_Keyword2
        {
            get;
            set;
        }

        [Category("javascript配色")]
        [Description("javascriptの文字列の色を設定します。")]
        public Color C_String
        {
            get;
            set;
        }

        [Category("javascript配色")]
        [Description("javascriptのコメントの色を設定します。")]
        public Color C_Comment
        {
            get;
            set;
        }

        [Category("javascript配色")]
        [Description("javascriptの通常の文字の色を設定します。")]
        public Color C_Normal
        {
            get;
            set;
        }

        [Category("javascript配色")]
        [Description("背景の色を設定します。")]
        public Color C_Background
        {
            get;
            set;
        }

        [Category("javascript配色")]
        [Description("数値の色を設定します。")]
        public Color C_Number
        {
            get;
            set;
        }
        */
        [Category("プロジェクト")]
        [Description("プロジェクトを作成するパスを指定します。")]
        public string ProjectPath
        {
            get;
            set;
        }


        [Category("フォント")]
        [Description("使用するフォントの名前を設定します。")]
        public string FontName
        {
            get;
            set;
        }

        [Category("フォント")]
        [Description("使用するフォントのサイズを設定します。")]
        public float FontSize
        {
            get;
            set;
        }
    }
}
