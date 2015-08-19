using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace YGOPRO_CardManager
{
    public class BanlistEditor: Form
    {
        private System.ComponentModel.IContainer components = null;
        private Dictionary<string, List<BanListCard>> m_banlists;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

         private void InitializeComponent()
        {
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.SemiLimitedList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.LimitedList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.BannedList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.BanList = new System.Windows.Forms.ListBox();
            this.BanListInput = new System.Windows.Forms.TextBox();
            this.SearchBox = new YGOPRO_CardManager.Components.SearchBox();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Savebtn = new System.Windows.Forms.Button();
            this.BanAnimeCardsBtn = new System.Windows.Forms.Button();
            this.Clearbtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.02543F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 594F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.301158F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(800, 638);
            this.tableLayoutPanel8.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(209, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(588, 600);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.SemiLimitedList, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(395, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(190, 594);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // SemiLimitedList
            // 
            this.SemiLimitedList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SemiLimitedList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.SemiLimitedList.FormattingEnabled = true;
            this.SemiLimitedList.IntegralHeight = false;
            this.SemiLimitedList.Location = new System.Drawing.Point(3, 20);
            this.SemiLimitedList.Name = "SemiLimitedList";
            this.SemiLimitedList.Size = new System.Drawing.Size(184, 571);
            this.SemiLimitedList.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Semi Limited";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.LimitedList, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(199, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(190, 594);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // LimitedList
            // 
            this.LimitedList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LimitedList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LimitedList.FormattingEnabled = true;
            this.LimitedList.IntegralHeight = false;
            this.LimitedList.Location = new System.Drawing.Point(3, 19);
            this.LimitedList.Name = "LimitedList";
            this.LimitedList.Size = new System.Drawing.Size(184, 572);
            this.LimitedList.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Limited";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.BannedList, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(190, 594);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // BannedList
            // 
            this.BannedList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BannedList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.BannedList.FormattingEnabled = true;
            this.BannedList.IntegralHeight = false;
            this.BannedList.Location = new System.Drawing.Point(3, 18);
            this.BannedList.Name = "BannedList";
            this.BannedList.Size = new System.Drawing.Size(184, 573);
            this.BannedList.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Banned";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SearchBox, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.54176F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.45824F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 600);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 243);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "BanList Select";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.BanList, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.BanListInput, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.60714F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.39286F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(188, 224);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // BanList
            // 
            this.BanList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BanList.FormattingEnabled = true;
            this.BanList.IntegralHeight = false;
            this.BanList.Location = new System.Drawing.Point(3, 3);
            this.BanList.Name = "BanList";
            this.BanList.Size = new System.Drawing.Size(182, 187);
            this.BanList.TabIndex = 0;
            this.BanList.SelectedIndexChanged += new System.EventHandler(this.BanList_SelectedIndexChanged);
            // 
            // BanListInput
            // 
            this.BanListInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BanListInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.BanListInput.Location = new System.Drawing.Point(3, 196);
            this.BanListInput.Name = "BanListInput";
            this.BanListInput.Size = new System.Drawing.Size(182, 20);
            this.BanListInput.TabIndex = 2;
            this.BanListInput.Text = "Add BanList";
            this.BanListInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SearchBox
            // 
            this.SearchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchBox.Location = new System.Drawing.Point(3, 252);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(194, 345);
            this.SearchBox.TabIndex = 1;
            this.SearchBox.TabStop = false;
            this.SearchBox.Text = "Search";
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 606);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 32);
            this.label4.TabIndex = 8;
            this.label4.Text = "Drag and Drop to add items to the banlist";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.Savebtn);
            this.flowLayoutPanel1.Controls.Add(this.BanAnimeCardsBtn);
            this.flowLayoutPanel1.Controls.Add(this.Clearbtn);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(209, 609);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(588, 26);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // Savebtn
            // 
            this.Savebtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Savebtn.Location = new System.Drawing.Point(510, 3);
            this.Savebtn.Name = "Savebtn";
            this.Savebtn.Size = new System.Drawing.Size(75, 23);
            this.Savebtn.TabIndex = 9;
            this.Savebtn.Text = "Save";
            this.Savebtn.UseVisualStyleBackColor = true;
            this.Savebtn.Click += new System.EventHandler(this.Savebtn_Click_1);
            // 
            // BanAnimeCardsBtn
            // 
            this.BanAnimeCardsBtn.Location = new System.Drawing.Point(398, 3);
            this.BanAnimeCardsBtn.Name = "BanAnimeCardsBtn";
            this.BanAnimeCardsBtn.Size = new System.Drawing.Size(106, 23);
            this.BanAnimeCardsBtn.TabIndex = 10;
            this.BanAnimeCardsBtn.Text = "Ban Anime Cards";
            this.BanAnimeCardsBtn.UseVisualStyleBackColor = true;
            // 
            // Clearbtn
            // 
            this.Clearbtn.Location = new System.Drawing.Point(317, 3);
            this.Clearbtn.Name = "Clearbtn";
            this.Clearbtn.Size = new System.Drawing.Size(75, 23);
            this.Clearbtn.TabIndex = 11;
            this.Clearbtn.Text = "Clear";
            this.Clearbtn.UseVisualStyleBackColor = true;
            // 
            // BanlistEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 638);
            this.Controls.Add(this.tableLayoutPanel8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BanlistEditor";
            this.Text = "BanListEditor";
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public BanlistEditor ()
		{
            InitializeComponent();
            TopLevel = false;
            Dock = DockStyle.Fill;
            Visible = true;
            LoadBanList();

            BanList.SelectedIndexChanged += BanList_SelectedIndexChanged;
            if (BanList.Items.Count > 0)
                BanList.SelectedIndex = 0;

            BannedList.AllowDrop = true;
            LimitedList.AllowDrop = true;
            SemiLimitedList.AllowDrop = true;

            SearchBox.List.MouseDown += SearchList_MouseDown;
            BannedList.DragEnter += List_DragEnter;
            LimitedList.DragEnter += List_DragEnter;
            SemiLimitedList.DragEnter += List_DragEnter;
            BannedList.DragDrop += List_DragDrop;
            LimitedList.DragDrop += List_DragDrop;
            SemiLimitedList.DragDrop += List_DragDrop;
            BannedList.DrawItem += List_DrawItem;
            LimitedList.DrawItem += List_DrawItem;
            SemiLimitedList.DrawItem += List_DrawItem;

            BanListInput.Enter += BanListInput_Enter;
            BanListInput.Leave += BanListInput_Leave;
            BanListInput.KeyDown += BanListInput_KeyDown;

            LimitedList.KeyDown += DeleteItem;
            SemiLimitedList.KeyDown += DeleteItem;
            BannedList.KeyDown += DeleteItem;
            BanList.KeyDown += DeleteBanList;
        }

        

        private void LoadBanList()
        {
            m_banlists = new Dictionary<string, List<BanListCard>>();
            if (!File.Exists("lflist.conf"))
                return;

            var reader = new StreamReader(File.OpenRead("lflist.conf"));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;
                if (line.StartsWith("!"))
                {
                    if (!BanList.Items.Contains(line.Substring(1)))
                        BanList.Items.Add(line.Substring(1));
                }
                else
                {
                    string[] parts = line.Split(' ');
                    if (!Program.CardData.ContainsKey(Int32.Parse(parts[0])))
                        continue;

                    if (Program.CardData[Int32.Parse(parts[0])].Name == "")
                        continue;

                    BanListCard card = new BanListCard();
                    card.ID = Int32.Parse(parts[0]);
                    if (parts[1] != "")
                        card.Banvalue = Int32.Parse(parts[1]);
                    else if (parts[2] != "")
                        card.Banvalue = Int32.Parse(parts[2]);
                    else
                        card.Banvalue = Int32.Parse(parts[3]);
                    card.Name = Program.CardData[Int32.Parse(parts[0])].Name;


                    if (!m_banlists.ContainsKey(BanList.Items[BanList.Items.Count - 1].ToString()))
                    {

                        m_banlists.Add(BanList.Items[BanList.Items.Count - 1].ToString(), new List<BanListCard>());
                        m_banlists[BanList.Items[BanList.Items.Count - 1].ToString()].Add(card);
                    }
                    else
                    {
                        if (!m_banlists[BanList.Items[BanList.Items.Count - 1].ToString()].Exists(banListCard => banListCard.ID == Int32.Parse(parts[0])))
                            m_banlists[BanList.Items[BanList.Items.Count - 1].ToString()].Add(card);
                    }
                }
            }
            reader.Close();

           
        }

        private void SaveBanList()
        {
            using (var writer = new StreamWriter("lflist.conf", false))
            {
                writer.WriteLine("#Built using DevPro card editor.");
                foreach (object t in BanList.Items)
                {
                    writer.WriteLine("!{0}", t);
                    try
                    {
                        var forbidden = m_banlists[t.ToString()].FindAll(x => x.Banvalue == 0);
                        var limited = m_banlists[t.ToString()].FindAll(x => x.Banvalue == 1);
                        var semiLimited = m_banlists[t.ToString()].FindAll(x => x.Banvalue == 2);

                        writer.WriteLine("#forbidden");
                        foreach (var banListCard in forbidden)
                        {
                            writer.WriteLine("{0} {1}", banListCard.ID, banListCard.Banvalue);
                        }

                        writer.WriteLine("#limit");
                        foreach (var banListCard in limited)
                        {
                            writer.WriteLine("{0} {1}", banListCard.ID, banListCard.Banvalue);
                        }

                        writer.WriteLine("#semi limit");
                        foreach (var banListCard in semiLimited)
                        {
                            writer.WriteLine("{0} {1}", banListCard.ID, banListCard.Banvalue);
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        MessageBox.Show("Unlimited was probably hit, good idea to check it out.");
                    }
                }
            }

            MessageBox.Show("Save Complete");

        }
        


        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ListBox SemiLimitedList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListBox LimitedList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.ListBox BannedList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.ListBox BanList;
        private System.Windows.Forms.TextBox BanListInput;
        private Components.SearchBox SearchBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button Savebtn;
        private System.Windows.Forms.Button BanAnimeCardsBtn;
        private System.Windows.Forms.Button Clearbtn;


        private void BanList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BanList.SelectedItem == null) return;
            BannedList.Items.Clear();
            LimitedList.Items.Clear();
            SemiLimitedList.Items.Clear();
            if (m_banlists.ContainsKey(BanList.SelectedItem.ToString()))
            {
                foreach (BanListCard card in m_banlists[BanList.SelectedItem.ToString()])
                {
                    if (card.Banvalue == 0)
                        BannedList.Items.Add(card);
                    else if (card.Banvalue == 1)
                        LimitedList.Items.Add(card);
                    else if (card.Banvalue == 2)
                        SemiLimitedList.Items.Add(card);
                }
            }
        }

        private void List_DrawItem(object sender, DrawItemEventArgs e)
        {
            var list = (ListBox)sender;
            e.DrawBackground();

            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            int index = e.Index;
            if (index >= 0 && index < list.Items.Count)
            {
                BanListCard card = (BanListCard)list.Items[index];
                Graphics g = e.Graphics;
                if (!Program.CardData.ContainsKey(card.ID))
                    list.Items.Remove(card);
                else
                {
                    g.FillRectangle((selected) ? new SolidBrush(Color.Blue) : new SolidBrush(Color.White), e.Bounds);

                    // Print text
                    g.DrawString((card.Name == "" ? card.ID.ToString() : card.Name), e.Font,
                                 (selected) ? Brushes.White : Brushes.Black,
                                 list.GetItemRectangle(index).Location);
                }
            }

            e.DrawFocusRectangle();
        }

        private void BanAnimeCardsBtn_Click(object sender, EventArgs e)
        {
            foreach (int id in Program.CardData.Keys)
            {
                if (Program.CardData[id].Ot == 4)
                {
                    if (GetBanListCard(id) == null)
                    {
                        BanListCard card = new BanListCard { ID = id, Banvalue = 0, Name = Program.CardData[id].Name };
                        BannedList.Items.Add(card);
                        m_banlists[BanList.SelectedItem.ToString()].Add(card);
                    }
                }
            }
        }

        private BanListCard GetBanListCard(int id)
        {
            foreach (BanListCard card in m_banlists[BanList.SelectedItem.ToString()])
            {
                if (card.ID == id)
                    return card;
            }
            return null;
        }

        private void SearchList_MouseDown(object sender, MouseEventArgs e)
        {
            var list = (ListBox)sender;
            int indexOfItem = list.IndexFromPoint(e.X, e.Y);
            if (indexOfItem >= 0 && indexOfItem < list.Items.Count)
            {
                list.DoDragDrop(list.Items[indexOfItem], DragDropEffects.Copy);
            }
        }

        private void List_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void List_DragDrop(object sender, DragEventArgs e)
        {
            var list = (ListBox)sender;
            int indexOfItemUnderMouseToDrop = list.IndexFromPoint(list.PointToClient(new Point(e.X, e.Y)));
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                int id = Int32.Parse(e.Data.GetData(DataFormats.Text).ToString());
                CardInfo cardinfo = Program.CardData[id];
                BanListCard bancard = new BanListCard() { ID = id, Name = cardinfo.Name, Banvalue = (list == BannedList ? 0 : list == LimitedList ? 1 : 2) };


                if (GetBanListCard(id) == null)
                {
                    if (indexOfItemUnderMouseToDrop >= 0 && indexOfItemUnderMouseToDrop < list.Items.Count)
                        list.Items.Insert(indexOfItemUnderMouseToDrop, bancard);
                    else
                        list.Items.Add(bancard);

                    m_banlists[BanList.SelectedItem.ToString()].Add(bancard);
                }
                else
                {
                    BanListCard foundcard = GetBanListCard(id);

                    if (foundcard.Banvalue == 0)
                        MessageBox.Show(foundcard.Name + " is already contained in the Banned list.");
                    else if (foundcard.Banvalue == 1)
                        MessageBox.Show(foundcard.Name + " is already contained in the Limited list.");
                    else if (foundcard.Banvalue == 2)
                        MessageBox.Show(foundcard.Name + " is already contained in the SemiLimited list.");
                }
            }
        }



        private void Savebtn_Click(object sender, EventArgs e)
        {
            SaveBanList();
        }

        private void Clearbtn_Click(object sender, EventArgs e)
        {
            BannedList.Items.Clear();
            LimitedList.Items.Clear();
            SemiLimitedList.Items.Clear();
            m_banlists[BanList.SelectedItem.ToString()].Clear();
        }
        private void BanListInput_Enter(object sender, EventArgs e)
        {
            if (BanListInput.Text == "Add BanList")
            {
                BanListInput.Text = "";
                BanListInput.ForeColor = SystemColors.WindowText;
            }
        }

        private void BanListInput_Leave(object sender, EventArgs e)
        {
            if (BanListInput.Text == "")
            {
                BanListInput.Text = "Add BanList";
                BanListInput.ForeColor = SystemColors.WindowFrame;
            }
        }
        private void BanListInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(BanListInput.Text))
                    return;

                if (m_banlists.ContainsKey(BanListInput.Text))
                {
                    BanList.SelectedItem = BanListInput.Text;
                    return;
                }

                m_banlists.Add(BanListInput.Text, new List<BanListCard>());
                BanList.Items.Add(BanListInput.Text);
                BanList.SelectedItem = BanListInput.Text;
                BanListInput.Clear();
            }
        }

        private void DeleteItem(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                var list = (ListBox)sender;
                if (list.SelectedIndex != -1)
                {
                    BanListCard card = (BanListCard)list.SelectedItem;
                    m_banlists[BanList.SelectedItem.ToString()].Remove(card);
                    list.Items.Remove(card);

                }
            }
        }

        private void DeleteBanList(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                var list = (ListBox)sender;
                if (list.SelectedIndex != -1)
                {
                    Clearbtn_Click(null, null);
                    m_banlists.Remove(list.SelectedItem.ToString());
                    list.Items.RemoveAt(list.SelectedIndex);
                }
            }
        }

        private void Savebtn_Click_1(object sender, EventArgs e)
        {
            SaveBanList();
        }



    }

    public class BanListCard
    {
        public int ID;
        public string Name;
        public int Banvalue;
    }
}
