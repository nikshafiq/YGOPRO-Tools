using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using YGOPRO_CardManager.Enums;
using Mono.Data.Sqlite;
using System.Text.RegularExpressions;

namespace YGOPRO_CardManager
{
    public partial class DeckManager : Form
    {


        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel2;
        private ListBox DeckList;
        private Components.SearchBox searchBox1;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel4;
        private ListBox CardList;
        private const string Cdbdir = "cards.cdb";
        string createdBy = string.Empty;

        List<int> PlayerMainDeck = new List<int>();
        List<int> PlayerExtraDeck = new List<int>();
        List<int> PlayerSideDeck = new List<int>();

        List<string> PlayerMainDeckText = new List<string>();
        List<string> PlayerExtraDeckText = new List<string>();
        List<string> PlayerSideDeckText = new List<string>();


        List<CardData> mainDeck = new List<CardData>();
        List<CardData> extraDeck = new List<CardData>();
        List<CardData> sideDeck = new List<CardData>();
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private ListBox ExtraList;
        private ListBox SideList;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private TableLayoutPanel tableLayoutPanel5;
        private Button button1;
        private Button button2;
        private Label label1;
        private TextBox textBox1;
        private RichTextBox richTextBox2;
        private RichTextBox richTextBox1;
        private Button button3;
        List<CardData> allCard = new List<CardData>();



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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.searchBox1 = new YGOPRO_CardManager.Components.SearchBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.DeckList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.SideList = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.CardList = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ExtraList = new System.Windows.Forms.ListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.46041F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.53959F));
            this.tableLayoutPanel1.Controls.Add(this.searchBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.13126F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.86874F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(159, 579);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // searchBox1
            // 
            this.searchBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchBox1.Location = new System.Drawing.Point(3, 327);
            this.searchBox1.Name = "searchBox1";
            this.searchBox1.Size = new System.Drawing.Size(153, 249);
            this.searchBox1.TabIndex = 7;
            this.searchBox1.TabStop = false;
            this.searchBox1.Text = "Search";
            this.searchBox1.Click += new System.EventHandler(this.searchBox1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 312);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Deck List";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.DeckList, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(8, 19);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.125F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.875F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(139, 287);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // DeckList
            // 
            this.DeckList.FormattingEnabled = true;
            this.DeckList.Location = new System.Drawing.Point(3, 3);
            this.DeckList.Name = "DeckList";
            this.DeckList.Size = new System.Drawing.Size(133, 277);
            this.DeckList.TabIndex = 4;
            this.DeckList.SelectedIndexChanged += new System.EventHandler(this.DeckList_SelectedIndexChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.04819F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.95181F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 225F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox6, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.groupBox4, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox3, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox5, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(163, 1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.6337F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.3663F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(612, 546);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.richTextBox2);
            this.groupBox6.Location = new System.Drawing.Point(389, 274);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 269);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "List to Extra Deck";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(6, 34);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(188, 223);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.SideList);
            this.groupBox4.Location = new System.Drawing.Point(204, 274);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(179, 269);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Side Deck List";
            // 
            // SideList
            // 
            this.SideList.FormattingEnabled = true;
            this.SideList.Location = new System.Drawing.Point(6, 32);
            this.SideList.Name = "SideList";
            this.SideList.Size = new System.Drawing.Size(175, 225);
            this.SideList.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel4);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.tableLayoutPanel3.SetRowSpan(this.groupBox2, 2);
            this.groupBox2.Size = new System.Drawing.Size(195, 540);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Main Deck List";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.CardList, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 22);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.72727F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.272727F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(199, 495);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // CardList
            // 
            this.CardList.FormattingEnabled = true;
            this.CardList.Location = new System.Drawing.Point(3, 3);
            this.CardList.Name = "CardList";
            this.CardList.Size = new System.Drawing.Size(190, 485);
            this.CardList.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ExtraList);
            this.groupBox3.Location = new System.Drawing.Point(204, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(179, 254);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Extra Deck List";
            // 
            // ExtraList
            // 
            this.ExtraList.FormattingEnabled = true;
            this.ExtraList.Location = new System.Drawing.Point(6, 25);
            this.ExtraList.Name = "ExtraList";
            this.ExtraList.Size = new System.Drawing.Size(175, 212);
            this.ExtraList.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.richTextBox1);
            this.groupBox5.Location = new System.Drawing.Point(389, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(203, 258);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "List to Main Deck";
            // 
            // richTextBox1
            // 
            this.richTextBox1.AllowDrop = true;
            this.richTextBox1.Location = new System.Drawing.Point(6, 25);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(188, 212);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 5;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.18615F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.81385F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 199F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.Controls.Add(this.button3, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.textBox1, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.button1, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.button2, 4, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(166, 550);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(606, 30);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(3, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "New";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(223, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(193, 20);
            this.textBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(107, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Deck Name";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(422, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Export";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(508, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Create";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // DeckManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(779, 584);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DeckManager";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);

        }

        public DeckManager ()
		{
			InitializeComponent();
            TopLevel = false;
            Dock = DockStyle.Fill;
            Visible = true;
            LoadDecks();

        }

        private void LoadDecks()
        {

            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/deck/");

            foreach (FileInfo file in dir.GetFiles())
                if (file.Extension == ".ydk")
                    DeckList.Items.Add(file.Name);


        }

        private void ClearDeck()
        {
            PlayerMainDeckText.Clear(); mainDeck.Clear();
            PlayerExtraDeckText.Clear(); extraDeck.Clear();
            PlayerSideDeckText.Clear(); sideDeck.Clear();

            PlayerMainDeck.Clear();
            PlayerExtraDeck.Clear();
            PlayerSideDeck.Clear();

            CardList.DataSource = null;
            CardList.Items.Clear();

            ExtraList.DataSource = null;
            ExtraList.Items.Clear();

            SideList.DataSource = null;
            SideList.Items.Clear();

        }

        private string GetCardName(int CardID, string cardcdb)
        {
            using (var conn = new SqliteConnection(@"Data Source= " + cardcdb))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT name FROM texts WHERE id LIKE @CardID";
                cmd.Parameters.Add(new SqliteParameter("@CardID", CardID));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string CardName = reader.GetString(reader.GetOrdinal("name"));
                        return CardName;
                    }
                }
            }
            return string.Empty;
        }

        private int GetCardID(string CardName, string cardcdb)
        {
             int CardID = 0;

            using (var conn = new SqliteConnection(@"Data Source= " + cardcdb))
              {
                SqliteCommand sql = new SqliteCommand("SELECT id, name FROM texts WHERE name LIKE @CardName", conn);
                
                conn.Open();
                sql.Parameters.Add(new SqliteParameter("@CardName", CardName));
                SqliteDataAdapter DataAdapter1 = new SqliteDataAdapter();
                DataSet ds = new DataSet();
                DataAdapter1.SelectCommand = sql;
                DataAdapter1.Fill(ds, "texts");

                if (ds.Tables[0].Rows.Count > 0)
                    CardID = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                else
                    CardID = 0;
                     return CardID;

              }
           
        }

        private void getCardText(List<int> Deck, List<string> DeckText, List<CardData> CardData)
        {
            string CardName = "";

            for (int i = 0; i < Deck.Count; i++)
            {
                string CardID = Deck[i].ToString();
                CardName = GetCardName(Convert.ToInt32(CardID), "cards.cdb");
                if (CardName == "")
                {

                    
                    try 
                        {

                            string expansioncdb = AppDomain.CurrentDomain.BaseDirectory + "/expansions/cards-tf.cdb";
                            CardName = GetCardName(Convert.ToInt32(CardID), expansioncdb);
                        }

                    catch (Exception ex) 
                        {
                          ex = new Exception();


                         }
                    MessageBox.Show(String.Format("Cannot find card from ID.{0}Card ID : {1}", Environment.NewLine, CardID.ToString()
                        ), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                //add number of card
                int index = DeckText.FindIndex(x => x.Contains(CardName));

                if (index > -1)
                {
                    int CardNumber = Convert.ToInt32(DeckText[index].Substring(DeckText[index].Length - 1, 1));

                    CardNumber++;
                    DeckText[index] = (CardName + " x" + CardNumber.ToString());

                    CardData[index] = (new CardData(Convert.ToInt32(CardID), (CardName + " x" + CardNumber.ToString())));
                    //CardData.Add(new cardData(Convert.ToInt32(CardID), (CardName + " x" + CardNumber.ToString())));
                }
                else
                {
                    DeckText.Add(CardName + " x1");
                    CardData.Add(new CardData(Convert.ToInt32(CardID), CardName + " x1"));
                }

            }
        }

        private void getCardIdentifier(List<string> DeckText, List<CardData> ListData)
        {
            int CardID = 0;

            for (int i = 0; i < DeckText.Count; i++)
            {
                string CardName = DeckText[i].ToString();
                CardID = GetCardID(CardName, "cards.cdb");
                if (CardID == 0)
                {


                    try
                    {

                        string expansioncdb = AppDomain.CurrentDomain.BaseDirectory + "/expansions/cards-tf.cdb";
                        CardID = GetCardID(CardName, expansioncdb);
                    }

                    catch (Exception ex)
                    {
                        ex = new Exception();

                        MessageBox.Show(String.Format("Cannot find card from ID.{0}Card ID : {1}", Environment.NewLine, CardID.ToString()
                        ), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                   
                }

                //add number of card
                int index = DeckText.FindIndex(x => x.Contains(CardName));

                if (index > 1)
                {
                    
                    
                    ListData.Add(new CardData(Convert.ToInt32(CardID), CardName));
                    
                }
                else
                {
                    //DeckText.Add(CardName + " x1");
                    ListData.Add(new CardData(Convert.ToInt32(CardID), CardName));
                }

            }
        }

        private void GetCardList()
        {

            string[] lines = richTextBox1.Lines;
            int count = 0;
            string name = "";
            foreach (string line in lines)
            {

                if (line.Contains("x2"))
                {

                    name = line.Substring(0, line.Length - 3).Trim();
                    count = 2;
                    

                    
                    PlayerMainDeckText.Add(name);
                }
                else if (line.Contains("x3"))
                {

                    name = line.Substring(0, line.Length - 3).Trim();
                    count = 3;
                    

                    PlayerMainDeckText.Add(name);
                    PlayerMainDeckText.Add(name);
                }

                else if (line.Contains("1 "))
                {

                    name = line.Substring(2, line.Length - 2).Trim();
                    count = 1;
                    


                    //PlayerMainDeckText.Add(name);
                }

                else if (line.Contains("2 "))
                {

                    name = line.Substring(2, line.Length - 2).Trim();
                    count = 2;
                    

                    
                    PlayerMainDeckText.Add(name);
                }

                else if (line.Contains("3 "))
                {

                    name = line.Substring(2, line.Length - 2).Trim();
                    count = 3;
                   

                    PlayerMainDeckText.Add(name);
                    PlayerMainDeckText.Add(name);
                }

                else

                    name = line.Trim();
                    PlayerMainDeckText.Add(name);

            }

            


            getCardIdentifier(PlayerMainDeckText, mainDeck);

            CardList.DataSource = mainDeck;
            CardList.DisplayMember = "Name";
            CardList.ValueMember = "ID";

           

        }

        private void GetExtraList()
        {

            string[] lines = richTextBox2.Lines;
            int count = 0;
            string name = "";
            foreach (string line in lines)
            {

                if (line.Contains("x2"))
                {

                    name = line.Substring(0, line.Length - 3).Trim();
                    count = 2;
                   


                    PlayerExtraDeckText.Add(name);
                }
                else if (line.Contains("x3"))
                {

                    name = line.Substring(0, line.Length - 3).Trim();
                    count = 3;
                    

                    PlayerExtraDeckText.Add(name);
                    PlayerExtraDeckText.Add(name);
                }

                else if (line.Contains("1 "))
                {

                    name = line.Substring(2, line.Length - 2).Trim();
                    count = 1;
                    


                    //PlayerExtraDeckText.Add(name);
                }

                else if (line.Contains("2 "))
                {

                    name = line.Substring(2, line.Length - 2).Trim();
                    count = 2;
                   


                    PlayerExtraDeckText.Add(name);
                }

                else if (line.Contains("3 "))
                {

                    name = line.Substring(2, line.Length - 2).Trim();
                    count = 3;
                    

                    PlayerExtraDeckText.Add(name);
                    PlayerExtraDeckText.Add(name);
                }

                else

                    name = line.Trim();
                PlayerExtraDeckText.Add(name);

            }




            getCardIdentifier(PlayerExtraDeckText, extraDeck);

            ExtraList.DataSource = extraDeck;
            ExtraList.DisplayMember = "Name";
            ExtraList.ValueMember = "ID";



        }

        private void WriteDeck(string path)
        {

            StreamWriter writer = new StreamWriter(path);

            
            writer.WriteLine("#created by ...");
            writer.WriteLine("#main");
            if (mainDeck.Count > 0)

                for (int i = 0; i <= mainDeck.Count - 1; i++ )
                    writer.WriteLine(mainDeck[i].ID);

            writer.WriteLine("#extra");

            if (extraDeck.Count > 0)
                for (int i= 0; i <= extraDeck.Count - 1; i++)
                    writer.WriteLine(extraDeck[i].ID);

            writer.WriteLine("!side");

            writer.Close();

        }

        private void WriteList(string path)
        {

            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine("Main Deck=" + mainDeck.Count);

            if (mainDeck.Count > 0)

                for (int i = 0; i <= mainDeck.Count - 1; i++)
                    writer.WriteLine(mainDeck[i].Name);

            writer.WriteLine("Extra Deck=" + extraDeck.Count);
            if (extraDeck.Count > 0)
                for (int i = 0; i <= extraDeck.Count - 1; i++)
                    writer.WriteLine(extraDeck[i].Name);

            writer.Close();
        
        
        }

        private void printList()
        {
            mainDeck.Insert(0, new CardData(-1, String.Format("Main Deck : {0}", PlayerMainDeck.Count.ToString())));
            extraDeck.Insert(0, new CardData(-2, String.Format("Extra Deck : {0}", PlayerExtraDeck.Count.ToString())));
            sideDeck.Insert(0, new CardData(-3, String.Format("Side Deck : {0}", PlayerSideDeck.Count.ToString())));

            allCard = new List<CardData>();
            allCard.AddRange(mainDeck);
            allCard.AddRange(extraDeck);
            allCard.AddRange(sideDeck);

            CardList.DataSource = mainDeck;
            CardList.DisplayMember = "Name";
            CardList.ValueMember = "ID";

            ExtraList.DataSource = extraDeck;
            ExtraList.DisplayMember = "Name";
            ExtraList.ValueMember = "ID";

            SideList.DataSource = sideDeck;
            SideList.DisplayMember = "Name";
            SideList.ValueMember = "ID";

            
        }

        private void DeckList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearDeck();
            

            string current = DeckList.SelectedItem.ToString();

            string DeckPath = AppDomain.CurrentDomain.BaseDirectory + "/deck/" + current;

            string[] lines = System.IO.File.ReadAllLines(DeckPath);
            createdBy = lines[0];
            int flag = 0; // 0 = Main Deck, 1 = Extra Deck, 2 = Side Deck
            int resultParse = 0;
            foreach (string line in lines)
            {
                switch (line)
                {
                    case "#main": flag = 0; break;
                    case "#extra": flag = 1; break;
                    case "!side": flag = 2; break;
                    default: if (int.TryParse(line, out resultParse))
                        {
                            switch (flag)
                            {
                                case 0: PlayerMainDeck.Add(resultParse); break;
                                case 1: PlayerExtraDeck.Add(resultParse); break;
                                case 2: PlayerSideDeck.Add(resultParse); break;
                            }
                        }
                        break;
                }
            }

            if (File.Exists(DeckPath))
                 {

                getCardText(PlayerMainDeck, PlayerMainDeckText, mainDeck);
                getCardText(PlayerExtraDeck, PlayerExtraDeckText, extraDeck);
                getCardText(PlayerSideDeck, PlayerSideDeckText, sideDeck);
                printList();

                 }
            
            //foreach (int val in PlayerMainDeck)
                //CardList.Items.Add(val.ToString());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearDeck();
            
            string deck = textBox1.Text.Trim();

            if (deck == String.Empty)
                deck = "test";

            if (richTextBox1.Lines.Count() > 0)
                GetCardList();

            if (richTextBox1.Lines.Count() > 0)
                GetExtraList();

            if (mainDeck != null)
                {
                WriteDeck(AppDomain.CurrentDomain.BaseDirectory + "/deck/" + deck + ".ydk");

                MessageBox.Show("Deck created: " + deck);

                }
            else

                MessageBox.Show("There is no decklist to create");
            

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (mainDeck != null)
                 {
                WriteList("list.txt");
                MessageBox.Show("Card list created");
                 }
            else

            MessageBox.Show("There is no cardlist to create");
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (mainDeck != null)
                ClearDeck();


        }

        private void List_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void List_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void List_DragDrop(object sender, DragEventArgs e)
        {
            string CardName = "";

            var list = (RichTextBox)sender;
            
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                int id = Int32.Parse(e.Data.GetData(DataFormats.Text).ToString());
                CardName = GetCardName(id, "cards.cdb");
               
                if (CardName == "")
                {

                    
                    try 
                        {

                            string expansioncdb = AppDomain.CurrentDomain.BaseDirectory + "/expansions/cards-tf.cdb";
                            CardName = GetCardName(id, expansioncdb);
                        }

                    catch (Exception ex) 
                        {
                          ex = new Exception();


                         }

                    

                }

                list.AppendText(CardName);
                
                  
           }



        }

        private void searchBox1_Click(object sender, EventArgs e)
        {
            string CardName = "";

            if (searchBox1.List.SelectedItem != null)
                 {
                     string current = searchBox1.List.SelectedItem.ToString();
                     int id = Int32.Parse(current);
                     CardName = GetCardName(id, "cards.cdb");

                     if (CardName == "")
                     {


                         try
                         {

                             string expansioncdb = AppDomain.CurrentDomain.BaseDirectory + "/expansions/cards-tf.cdb";
                             CardName = GetCardName(id, expansioncdb);
                         }

                         catch (Exception ex)
                         {
                             ex = new Exception();


                         }



                     }

                     richTextBox1.AppendText(CardName + Environment.NewLine);

                 }
        }


    }
}
