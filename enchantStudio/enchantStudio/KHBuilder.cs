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
    public partial class Form1
    {

        public void setKeywordColor()
        {
            
            //khl = new KeywordHighlighter();
            //csh = new ColorScheme();

            //khl.AddKeywordSet(keywords1, CharClass.Keyword);
            //khl.AddKeywordSet(keywords2, CharClass.Keyword2);
            //khl.AddEnclosure("\"", "\"", CharClass.String, false, '\\');
            //khl.AddEnclosure("'", "'", CharClass.String, false, '\"');
            //khl.AddEnclosure("/*", "*/", CharClass.Comment, true);
            //khl.AddLineHighlight("//", CharClass.Comment);
            /*
            csh.SetColor(CharClass.Keyword, config.C_Keyword1, config.C_Background);
            csh.SetColor(CharClass.Keyword2, config.C_Keyword2, config.C_Background);
            csh.SetColor(CharClass.Comment, config.C_Comment, config.C_Background);
            csh.SetColor(CharClass.String, config.C_String, config.C_Background);
            csh.SetColor(CharClass.Normal, config.C_Normal, config.C_Background);
            csh.SetColor(CharClass.Number, config.C_Number, config.C_Background);
             * */
            string[] exts = File.ReadAllLines("Data/Extension Setting/Extensions.txt", Encoding.GetEncoding(932));
            string filter = File.ReadAllText("Data/Extension Setting/FileList.txt", Encoding.GetEncoding(932));
            KeywordHighlighter setkh;
            ColorScheme setcs;
            //フィルターを設定
            openFileDialog1.Filter = filter;
            openFileDialog1.FilterIndex = 0;
            saveFileDialog1.Filter = filter;
            saveFileDialog1.FilterIndex = 0;

            GetBackColors();

            for (int i = 0; i < exts.Length; i++)
            {
                if (exts[i].IndexOf("$") != -1)
                {
                    string[] setexts = exts[i].Split('$');
                    Highlighters[setexts[0].ToLower()] = Highlighters[setexts[1]];
                    ColorSchemes[setexts[0].ToLower()] = ColorSchemes[setexts[1]];
                }
                else
                {
                    setkh = GetExtensionKeywords("Data/Extension Setting/" + exts[i]);
                    setcs = GetExtensionScheme("Data/Extension Setting/" + exts[i]);
                    Highlighters[exts[i].ToLower()] = setkh;
                    ColorSchemes[exts[i].ToLower()] = setcs;
                }
            }
            Highlighters["$DEFAULT$"] = Highlighters["txt"];
            ColorSchemes["$DEFAULT$"] = ColorSchemes["txt"];
        }

        /// <summary>
        /// 背景色を読み込んで設定します。
        /// </summary>
        private void GetBackColors()
        {
            string[] args;
            string[] lbuf = File.ReadAllLines("Data/Extension Setting/BackColor.txt",Encoding.GetEncoding(932));
            Color sc;
            for (int i = 0; i < lbuf.Length; i++)
            {
                args = lbuf[i].Split(',');
                if (args.Length < 4) return;
                sc = Color.FromArgb(int.Parse(args[1]), int.Parse(args[2]), int.Parse(args[3]));
                BackColors[args[0].ToLower()] = sc;
            }
            BackColors["$DEFAULT$"] = BackColors["txt"];
        }

        /// <summary>
        /// KeywordHi(ryに拡張子の設定をぶっこんで生成します。
        /// </summary>
        /// <param name="path">パス(最後のアレはナクておｋ)</param>
        /// <returns>いろいろキーワードが設定されたKeywordHighlighter</returns>
        private KeywordHighlighter GetExtensionKeywords(string path)
        {
            KeywordHighlighter setkhl = new KeywordHighlighter();
            //普通のキーワード
            if (File.Exists(path + "/Keyword1.txt"))
            {
                setkhl.AddKeywordSet(File.ReadAllLines(path + "/Keyword1.txt", Encoding.GetEncoding(932)), CharClass.Keyword);
            }
            if (File.Exists(path + "/Keyword2.txt"))
            {
                setkhl.AddKeywordSet(File.ReadAllLines(path + "/Keyword2.txt", Encoding.GetEncoding(932)), CharClass.Keyword2);
            }
            if (File.Exists(path + "/Keyword3.txt"))
            {
                setkhl.AddKeywordSet(File.ReadAllLines(path + "/Keyword3.txt", Encoding.GetEncoding(932)), CharClass.Keyword3);
            }
            //エンクロージャ
            CharClass setclass;
            string[] encs;// = File.ReadAllLines(path + "/Enclosure.txt", Encoding.GetEncoding(932));
            string[] args;
            if (File.Exists(path + "/Enclosure.txt"))
            {
                encs = File.ReadAllLines(path + "/Enclosure.txt", Encoding.GetEncoding(932));
                for (int i = 0; i < encs.Length; i++)
                {
                    args = encs[i].Split(',');
                    if (args.Length < 5) continue;
                    setclass = GetCharClassFromString(args[0]);
                    if (args[3].Length >= 1)
                    {
                        setkhl.AddEnclosure(args[1], args[2], setclass, bool.Parse(args[4]), (char)(args[3].ToCharArray())[0]);
                    }
                    else
                    {
                        setkhl.AddEnclosure(args[1], args[2], setclass, bool.Parse(args[4]));
                    }
                }
            }
            
            //行タイプ
            if (File.Exists(path + "/Line.txt"))
            {
                string[] lhs = File.ReadAllLines(path + "/Line.txt", Encoding.GetEncoding(932));
                for (int i = 0; i < lhs.Length; i++)
                {
                    args = lhs[i].Split(',');
                    if (args.Length < 2) continue;
                    setclass = GetCharClassFromString(args[0]);
                    setkhl.AddLineHighlight(args[1], setclass);
                }
            }

            return setkhl;
        }

        /// <summary>
        /// 指定されたクラスを継承して、いろいろ追加ぶっこんで返します。
        /// </summary>
        /// <param name="path">パス</param>
        /// <param name="extpar">元のKeywordHi(ry</param>
        /// <returns></returns>
        private KeywordHighlighter GetExtensionKeywords(string path,KeywordHighlighter extpar)
        {
            KeywordHighlighter setkhl = extpar;
            //普通のキーワード
            if (File.Exists(path + "/Keyword1.txt"))
            {
                setkhl.AddKeywordSet(File.ReadAllLines(path + "/Keyword1.txt", Encoding.GetEncoding(932)), CharClass.Keyword);
            }
            if (File.Exists(path + "/Keyword2.txt"))
            {
                setkhl.AddKeywordSet(File.ReadAllLines(path + "/Keyword2.txt", Encoding.GetEncoding(932)), CharClass.Keyword2);
            }
            if (File.Exists(path + "/Keyword3.txt"))
            {
                setkhl.AddKeywordSet(File.ReadAllLines(path + "/Keyword3.txt", Encoding.GetEncoding(932)), CharClass.Keyword3);
            }
            //エンクロージャ
            CharClass setclass;
            string[] encs;// = File.ReadAllLines(path + "/Enclosure.txt", Encoding.GetEncoding(932));
            string[] args;
            if (File.Exists(path + "/Enclosure.txt"))
            {
                encs = File.ReadAllLines(path + "/Enclosure.txt", Encoding.GetEncoding(932));
                for (int i = 0; i < encs.Length; i++)
                {
                    args = encs[i].Split(',');
                    if (args.Length < 5) continue;
                    setclass = GetCharClassFromString(args[0]);
                    if (args[3].Length >= 1)
                    {
                        setkhl.AddEnclosure(args[1], args[2], setclass, bool.Parse(args[4]), (char)(args[3].ToCharArray())[0]);
                    }
                    else
                    {
                        setkhl.AddEnclosure(args[1], args[2], setclass, bool.Parse(args[4]));
                    }
                }
            }
            
            //行タイプ
            if (File.Exists(path + "/Line.txt"))
            {
                string[] lhs = File.ReadAllLines(path + "/Line.txt", Encoding.GetEncoding(932));
                for (int i = 0; i < lhs.Length; i++)
                {
                    args = lhs[i].Split(',');
                    if (args.Length < 2) continue;
                    setclass = GetCharClassFromString(args[0]);
                    setkhl.AddLineHighlight(args[1], setclass);
                }
            }

            return setkhl;
        }

        /// <summary>
        /// ColorSc(ryに拡張子の設定をぶっこんで生成します。
        /// </summary>
        /// <param name="path">パス（最後はナクておｋ）</param>
        /// <returns></returns>
        private ColorScheme GetExtensionScheme(string path)
        {
            ColorScheme setcsh=new ColorScheme();
            string[] args;
            string[] setstr;
            CharClass setclass;
            if (File.Exists(path + "/Color.txt"))
            {
                args = File.ReadAllLines(path + "/Color.txt", Encoding.GetEncoding(932));
                for (int i = 0; i < args.Length; i++)
                {
                    setstr = args[i].Split(',');
                    if (setstr.Length < 7) continue;
                    setclass = GetCharClassFromString(setstr[0]);
                    setcsh.SetColor(setclass, Color.FromArgb(int.Parse(setstr[1]), int.Parse(setstr[2]), int.Parse(setstr[3])), Color.FromArgb(int.Parse(setstr[4]), int.Parse(setstr[5]), int.Parse(setstr[6])));
                }
            }
            return setcsh;
        }

        /// <summary>
        /// 指定した文字列に対応したCharClassを返します。
        /// </summary>
        /// <param name="clnstr"></param>
        /// <returns></returns>
        private CharClass GetCharClassFromString(string clnstr)
        {
            switch (clnstr)
            {
                case "Normal":
                    return CharClass.Normal;
                case "Number":
                    return CharClass.Number;
                case "String":
                    return CharClass.String;
                case "Keyword":
                    return CharClass.Keyword;
                case "Keyword2":
                    return CharClass.Keyword2;
                case "Keyword3":
                    return CharClass.Keyword3;
                case "Macro":
                    return CharClass.Macro;
                case "Attribute":
                    return CharClass.Attribute;
                case "Comment":
                    return CharClass.Comment;
                case "Regex":
                    return CharClass.Regex;
                case "Annotation":
                    return CharClass.Annotation;
                case "AttributeValue":
                    return CharClass.AttributeValue;
                case "CDataSection":
                    return CharClass.CDataSection;
                case "Character":
                    return CharClass.Character;
                case "Delimiter":
                    return CharClass.Delimiter;
                case "DocComment":
                    return CharClass.DocComment;
                case "ElementName":
                    return CharClass.ElementName;
                case "EmbededScript":
                    return CharClass.EmbededScript;
                case "Entity":
                    return CharClass.Entity;
                case "Heading1":
                    return CharClass.Heading1;
                case "Heading2":
                    return CharClass.Heading2;
                case "Heading3":
                    return CharClass.Heading3;
                case "Heading4":
                    return CharClass.Heading4;
                case "Heading5":
                    return CharClass.Heading5;
                case "Heading6":
                    return CharClass.Heading6;
                case "Property":
                    return CharClass.Property;
                case "Selecter":
                    return CharClass.Selecter;
                case "Type":
                    return CharClass.Type;
                case "Value":
                    return CharClass.Value;
                default:
                    return CharClass.Normal;
            }
        }
    
    }
}
