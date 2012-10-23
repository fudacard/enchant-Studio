using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace enchantStudio
{
    public class ESProject
    {
        List<string> addfiles;

        public ESProject()
        {
            addfiles = new List<string>();
            ProjectName = "";
            ProjectPath = "";
        }

        /// <summary>
        /// プロジェクト名です。
        /// </summary>
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// プロジェクトのあるパスです。
        /// 最後の\や/は入らないので、ご注意ぐださい。
        /// </summary>
        public string ProjectPath
        {
            get;
            set;
        }

        /// <summary>
        /// デフォルトで追加されていくファイルの
        /// リストです。
        /// </summary>
        public List<string> DefaultFiles
        {
            get
            {
                return addfiles;
            }
            set
            {
                addfiles = value;
            }
        }

        public string Execute
        {
            get;
            set;
        }

        public bool IsCreated
        {
            get;
            set;
        }

    }
}
