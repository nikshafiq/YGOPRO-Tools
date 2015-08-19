using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;
using System.Drawing;
using System.Globalization;

namespace YGOPRO_CardManager
{
    public class IDConverter: Form
    {
        private System.ComponentModel.IContainer components = null;
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
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.AddButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.patchchk = new System.Windows.Forms.CheckBox();
            this.imagechk = new System.Windows.Forms.CheckBox();
            this.cdbchk = new System.Windows.Forms.CheckBox();
            this.UpdateCardsList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.NewId = new System.Windows.Forms.TextBox();
            this.SearchBox = new YGOPRO_CardManager.Components.SearchBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ConvertButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel8.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(733, 509);
            this.tableLayoutPanel8.TabIndex = 2;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.AddButton);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 480);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(360, 26);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(282, 3);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 0;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(369, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.60493F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(361, 471);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(355, 465);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Convert List";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.groupBox4, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.UpdateCardsList, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.08072F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.91928F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(349, 446);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel9);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 378);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(343, 65);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Convert Options";
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Controls.Add(this.checkBox1, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.patchchk, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.imagechk, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.cdbchk, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(337, 46);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(55, 29);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(58, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Scripts";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // patchchk
            // 
            this.patchchk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.patchchk.AutoSize = true;
            this.patchchk.Checked = true;
            this.patchchk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.patchchk.Location = new System.Drawing.Point(208, 29);
            this.patchchk.Name = "patchchk";
            this.patchchk.Size = new System.Drawing.Size(88, 17);
            this.patchchk.TabIndex = 1;
            this.patchchk.Text = "Create Patch";
            this.patchchk.UseVisualStyleBackColor = true;
            // 
            // imagechk
            // 
            this.imagechk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.imagechk.AutoSize = true;
            this.imagechk.Checked = true;
            this.imagechk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.imagechk.Location = new System.Drawing.Point(54, 4);
            this.imagechk.Name = "imagechk";
            this.imagechk.Size = new System.Drawing.Size(60, 17);
            this.imagechk.TabIndex = 0;
            this.imagechk.Text = "Images";
            this.imagechk.UseVisualStyleBackColor = true;
            // 
            // cdbchk
            // 
            this.cdbchk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cdbchk.AutoSize = true;
            this.cdbchk.Checked = true;
            this.cdbchk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cdbchk.Location = new System.Drawing.Point(228, 4);
            this.cdbchk.Name = "cdbchk";
            this.cdbchk.Size = new System.Drawing.Size(48, 17);
            this.cdbchk.TabIndex = 2;
            this.cdbchk.Text = "CDB";
            this.cdbchk.UseVisualStyleBackColor = true;
            // 
            // UpdateCardsList
            // 
            this.UpdateCardsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpdateCardsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.UpdateCardsList.FormattingEnabled = true;
            this.UpdateCardsList.IntegralHeight = false;
            this.UpdateCardsList.Location = new System.Drawing.Point(3, 3);
            this.UpdateCardsList.Name = "UpdateCardsList";
            this.UpdateCardsList.Size = new System.Drawing.Size(343, 369);
            this.UpdateCardsList.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.SearchBox, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.60493F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.395061F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(360, 471);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.NewId, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 434);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(354, 34);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // NewId
            // 
            this.NewId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NewId.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.NewId.Location = new System.Drawing.Point(3, 3);
            this.NewId.Name = "NewId";
            this.NewId.Size = new System.Drawing.Size(348, 20);
            this.NewId.TabIndex = 2;
            this.NewId.Text = "New ID";
            this.NewId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SearchBox
            // 
            this.SearchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchBox.Location = new System.Drawing.Point(3, 3);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(354, 425);
            this.SearchBox.TabIndex = 4;
            this.SearchBox.TabStop = false;
            this.SearchBox.Text = "Search";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ConvertButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(369, 480);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(361, 26);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // ConvertButton
            // 
            this.ConvertButton.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.ConvertButton.Location = new System.Drawing.Point(283, 3);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new System.Drawing.Size(75, 23);
            this.ConvertButton.TabIndex = 0;
            this.ConvertButton.Text = "Convert";
            this.ConvertButton.UseVisualStyleBackColor = true;
            this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_Click);
            // 
            // IDConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(733, 509);
            this.Controls.Add(this.tableLayoutPanel8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IDConverter";
            this.Text = "IDConverter";
            this.tableLayoutPanel8.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public IDConverter ()
		{
            InitializeComponent();
            TopLevel = false;
            Dock = DockStyle.Fill;
            Visible = true;

            NewId.Enter += NewIDInput_Enter;
            NewId.Leave += NewIDInput_Leave;

            UpdateCardsList.DrawItem += NewIDList_DrawItem;
            UpdateCardsList.KeyDown += DeleteItem;
        }

        private void DeleteItem(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                var list = (ListBox)sender;
                if (list.SelectedIndex != -1)
                    list.Items.RemoveAt(list.SelectedIndex);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            List<string[]> updateCards = new List<string[]>();
            if (SearchBox.List.SelectedIndex == -1)
            {
                MessageBox.Show("No card selected.", "Error!", MessageBoxButtons.OK);
                return;
            }

            int newId;
            if (!Int32.TryParse(NewId.Text, out newId))
            {
                MessageBox.Show("New Id is invalid.", "Error!", MessageBoxButtons.OK);
                return;
            }

            if (Program.CardData.ContainsKey(newId) || updateCards.Exists(x => x[1] == newId.ToString(CultureInfo.InvariantCulture)))
            {
                MessageBox.Show("New Id is already been used.", "Error!", MessageBoxButtons.OK);
                return;
            }

            int selectedCardId = Convert.ToInt32(SearchBox.List.SelectedItem.ToString());
            if (updateCards.Exists(x => x[0] == selectedCardId.ToString(CultureInfo.InvariantCulture)))
            {
                MessageBox.Show("Card already in list to be changed", "Error!", MessageBoxButtons.OK);
                return;
            }


            var cardToUpdate = new string[2];
            cardToUpdate[0] = selectedCardId.ToString(CultureInfo.InvariantCulture);
            cardToUpdate[1] = newId.ToString(CultureInfo.InvariantCulture);

            UpdateCardsList.Items.Add(cardToUpdate);
        }


        private void ConvertButton_Click(object sender, EventArgs e)
        {
            bool updateCdb = cdbchk.Checked;
            bool updateScript = patchchk.Checked;
            bool updateImage = imagechk.Checked;
            List<string[]> updateCards = new List<string[]>();

            if (patchchk.Checked)
            {
                if (!Directory.Exists("DevPatch"))
                    Directory.CreateDirectory("DevPatch");
                if (!Directory.Exists("DevPatch\\script"))
                    Directory.CreateDirectory("DevPatch\\script");
                if (!Directory.Exists("DevPatch\\pics"))
                    Directory.CreateDirectory("DevPatch\\pics");
                if (!Directory.Exists("DevPatch\\pics\\thumbnail"))
                    Directory.CreateDirectory("DevPatch\\pics\\thumbnail");
            }

            foreach (var updateCard in updateCards)
            {
                if (updateCdb)
                {
                    string str = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) ?? "";
                    string str2 = Path.Combine(str, "cards.cdb");
                    if (!File.Exists(str2))
                    {
                        MessageBox.Show("cards.cdb not found.");
                        return;
                    }

                    var connection = new SqliteConnection("Data Source=" + str2);
                    connection.Open();

                    SQLiteCommands.UpdateCardId(updateCard[0], updateCard[1], connection);

                    connection.Close();

                    if (patchchk.Checked)
                        File.Copy(str2, "Patch\\cards.cdb", true);

                }

                if (updateImage)
                {
                    string mainDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) ?? "";
                    const string picFolderName = "pics";
                    const string tumbnailFolderName = "pics\\thumbnail";
                    string picName = updateCard[0] + ".jpg";
                    string newPicName = updateCard[1] + ".jpg";

                    string imagePath = Path.Combine(mainDir, picFolderName, picName);
                    string newImagePath = Path.Combine(mainDir, picFolderName, newPicName);
                    string thumbnailImagePath = Path.Combine(mainDir, tumbnailFolderName, picName);
                    string newthumbnailImagePath = Path.Combine(mainDir, tumbnailFolderName, newPicName);

                    if (File.Exists(imagePath) && !File.Exists(newImagePath))
                        File.Move(imagePath, newImagePath);
                    if (File.Exists(thumbnailImagePath) && !File.Exists(newthumbnailImagePath))
                        File.Move(thumbnailImagePath, newthumbnailImagePath);
                    if (patchchk.Checked)
                    {
                        if (File.Exists(newImagePath))
                            File.Copy(newImagePath, Path.Combine("Patch\\pics", newPicName), true);
                        if (File.Exists(newthumbnailImagePath))
                            File.Copy(newthumbnailImagePath, Path.Combine("Patch\\pics\\thumbnail", newPicName), true);
                    }
                }

                if (updateScript)
                {
                    string mainDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) ?? "";
                    const string scriptFolderName = "script";
                    string scriptName = "c" + updateCard[0] + ".lua";
                    string newScriptName = "c" + updateCard[1] + ".lua";

                    string scriptPath = Path.Combine(mainDir, scriptFolderName, scriptName);
                    string newScriptPath = Path.Combine(mainDir, scriptFolderName, newScriptName);

                    if (File.Exists(scriptPath))
                    {
                        File.Move(scriptPath, newScriptPath);

                        //needs testing id replacing
                        string scriptFile = File.ReadAllText(newScriptPath);
                        scriptFile = scriptFile.Replace(updateCard[0], updateCard[1]);
                        File.WriteAllText(newScriptPath, scriptFile);

                        if (patchchk.Checked)
                            if (File.Exists(newScriptPath))
                                File.Copy(newScriptPath, Path.Combine("Patch\\script", newScriptName), true);
                    }
                }

                Program.CardData.RenameKey(Convert.ToInt32(updateCard[0]), Convert.ToInt32(updateCard[1]));

            }
            UpdateCardsList.Items.Clear();
            MessageBox.Show("Complete.");
        }

        private void NewIDInput_Enter(object sender, EventArgs e)
        {
            if (NewId.Text == "New ID")
            {
                NewId.Text = "";
                NewId.ForeColor = SystemColors.WindowText;
            }
        }

        private void NewIDInput_Leave(object sender, EventArgs e)
        {
            if (NewId.Text == "")
            {
                NewId.Text = "New ID";
                NewId.ForeColor = SystemColors.WindowFrame;
            }
        }

        private void NewIDList_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            int index = e.Index;
            if (index >= 0 && index < UpdateCardsList.Items.Count)
            {
                var data = (string[])UpdateCardsList.Items[index];
                Graphics g = e.Graphics;

                CardInfo card = Program.CardData[Int32.Parse(data[0])];

                g.FillRectangle((selected) ? new SolidBrush(Color.Blue) : new SolidBrush(Color.White), e.Bounds);

                // Print text
                g.DrawString((card.Name == "" ? "???" : card.Name) + "   -  " + data[0] + " > " + data[1], e.Font, (selected) ? Brushes.White : Brushes.Black,
                    UpdateCardsList.GetItemRectangle(index).Location);
            }

            e.DrawFocusRectangle();
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox NewId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ListBox UpdateCardsList;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.CheckBox patchchk;
        private System.Windows.Forms.CheckBox imagechk;
        private System.Windows.Forms.CheckBox cdbchk;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button ConvertButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button AddButton;
        private Components.SearchBox SearchBox;
        private System.Windows.Forms.CheckBox checkBox1;





    }
}
