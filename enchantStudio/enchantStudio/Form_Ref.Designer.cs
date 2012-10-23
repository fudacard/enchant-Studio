namespace enchantStudio
{
    partial class Form_Ref
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Ref));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ページToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.トップページToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ブラウザToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.戻るToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.進むToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ページToolStripMenuItem,
            this.ブラウザToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1262, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ページToolStripMenuItem
            // 
            this.ページToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.トップページToolStripMenuItem});
            this.ページToolStripMenuItem.Name = "ページToolStripMenuItem";
            this.ページToolStripMenuItem.Size = new System.Drawing.Size(56, 22);
            this.ページToolStripMenuItem.Text = "ページ";
            // 
            // トップページToolStripMenuItem
            // 
            this.トップページToolStripMenuItem.Name = "トップページToolStripMenuItem";
            this.トップページToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.トップページToolStripMenuItem.Text = "トップページ";
            this.トップページToolStripMenuItem.Click += new System.EventHandler(this.トップページToolStripMenuItem_Click);
            // 
            // ブラウザToolStripMenuItem
            // 
            this.ブラウザToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.戻るToolStripMenuItem,
            this.進むToolStripMenuItem,
            this.更新ToolStripMenuItem});
            this.ブラウザToolStripMenuItem.Name = "ブラウザToolStripMenuItem";
            this.ブラウザToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            this.ブラウザToolStripMenuItem.Text = "ブラウザ";
            // 
            // 戻るToolStripMenuItem
            // 
            this.戻るToolStripMenuItem.Name = "戻るToolStripMenuItem";
            this.戻るToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.戻るToolStripMenuItem.Text = "戻る";
            this.戻るToolStripMenuItem.Click += new System.EventHandler(this.戻るToolStripMenuItem_Click);
            // 
            // 進むToolStripMenuItem
            // 
            this.進むToolStripMenuItem.Name = "進むToolStripMenuItem";
            this.進むToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.進むToolStripMenuItem.Text = "進む";
            this.進むToolStripMenuItem.Click += new System.EventHandler(this.進むToolStripMenuItem_Click);
            // 
            // 更新ToolStripMenuItem
            // 
            this.更新ToolStripMenuItem.Name = "更新ToolStripMenuItem";
            this.更新ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.更新ToolStripMenuItem.Text = "更新";
            this.更新ToolStripMenuItem.Click += new System.EventHandler(this.更新ToolStripMenuItem_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 26);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(1262, 614);
            this.webBrowser1.TabIndex = 1;
            this.webBrowser1.Url = new System.Uri("http://enchantjs.com/ja/reference.html", System.UriKind.Absolute);
            // 
            // Form_Ref
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 640);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_Ref";
            this.Text = "enchant.js Reference & Preview";
            this.Load += new System.EventHandler(this.Form_Ref_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ページToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem トップページToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ブラウザToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 戻るToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 進むToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更新ToolStripMenuItem;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}