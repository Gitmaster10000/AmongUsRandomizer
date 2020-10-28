using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AURandomizer
{
    public partial class AURandomizer : Form
    {
        private static Random rng = new Random();
        private Player[] allPlayers = new Player[10];
        private int playerCnt = 10;
        private const int MAX_PLAYER_COUNT = 10;
        private const int MIN_PLAYER_COUNT = 1;
        public AURandomizer()
        {
            InitializeComponent();

            Player p1 = new Player(tableLayoutPanel4, textBox1, textBox2);
            Player p2 = new Player(tableLayoutPanel5, textBox4, textBox3);
            Player p3 = new Player(tableLayoutPanel6, textBox6, textBox5);
            Player p4 = new Player(tableLayoutPanel7, textBox8, textBox7);
            Player p5 = new Player(tableLayoutPanel8, textBox10, textBox9);
            Player p6 = new Player(tableLayoutPanel9, textBox12, textBox11);
            Player p7 = new Player(tableLayoutPanel10, textBox14, textBox13);
            Player p8 = new Player(tableLayoutPanel11, textBox16, textBox15);
            Player p9 = new Player(tableLayoutPanel12, textBox18, textBox17);
            Player p10 = new Player(tableLayoutPanel13, textBox20, textBox19);
            allPlayers[0] = p1;
            allPlayers[1] = p2;
            allPlayers[2] = p3;
            allPlayers[3] = p4;
            allPlayers[4] = p5;
            allPlayers[5] = p6;
            allPlayers[6] = p7;
            allPlayers[7] = p8;
            allPlayers[8] = p9;
            allPlayers[9] = p10;

            CenterToScreen();
        }

        #region private method
        private void CopyToClipboard(bool setIngameBox = false, int maxNameLength = 15)
        {
            string clipboardString = string.Empty;
            foreach (Player p in allPlayers)
            {
                if (p.IsVisible)
                {
                    int spaceCnt = maxNameLength - p.IngameName.Length;
                    clipboardString += p.Name + " is ||" + p.IngameName + new string(' ', spaceCnt) + "||\n";
                    if (setIngameBox)
                    {
                        if (p.IsTextVisible)
                        {
                            p.IngameNameBox.Text = p.IngameName;
                        }
                        else
                        {
                            p.IngameNameBox.Text = "???";
                        }
                    }
                }
            }
            Clipboard.SetText(clipboardString);
        }
        #endregion

        #region events

        private void buttonRandomize_Click(object sender, EventArgs e)
        {
            //randomize
            List<Player> visiblePlayers = new List<Player>();
            foreach(Player p in allPlayers)
            {
                if (p.IsVisible)
                {
                    p.Name = p.NameBox.Text;
                    p.IngameName = string.Empty;
                    p.IngameNameBox.Text = string.Empty;
                    visiblePlayers.Add(p);
                    if (string.IsNullOrWhiteSpace(p.Name))
                    {
                        throw new Exception("Every player needs to have a name set!");
                    }
                }
            }
            Player[] randomizedPlayerList = new Player[playerCnt];
            int iPlayer = 0;
            while (visiblePlayers.Count > 0)
            {
                int randomNumber = rng.Next(1, visiblePlayers.Count + 1) - 1;
                randomizedPlayerList[iPlayer] = visiblePlayers[randomNumber];
                visiblePlayers.RemoveAt(randomNumber);
                iPlayer++;
            }

            //The last player is the first player in the list
            randomizedPlayerList[playerCnt - 1].IngameName = randomizedPlayerList[0].Name;
            //set ingame names for all the other players to the next in the list
            for(int i = 0; i<playerCnt-1; i++)
            {
                randomizedPlayerList[i].IngameName = randomizedPlayerList[i+1].Name;
            }

            CopyToClipboard(true);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }
        private void buttonReset_Click(object sender, EventArgs e)
        {
            //set all players visible and reset player count to 10
            foreach(Player p in allPlayers)
            {
                p.Reset();
            }
            playerCnt = MAX_PLAYER_COUNT;
        }
        private void buttonToggle_Click(object sender, EventArgs e)
        {
            foreach (Player p in allPlayers)
            {
                p.ToggleTextVisible();
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (playerCnt == MIN_PLAYER_COUNT)
            {
                return;
            }
            playerCnt -= 1;
            for(int i = 9; i>= 0; i--)
            {
                if (allPlayers[i].IsVisible)
                {
                    allPlayers[i].ToggleVisible();
                    break;
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if(playerCnt == MAX_PLAYER_COUNT)
            {
                return;
            }
            playerCnt += 1;
            for (int i = 0; i <= 9; i++)
            {
                if (!allPlayers[i].IsVisible)
                {
                    allPlayers[i].ToggleVisible();
                    break;
                }
            }
        }
        private void AURandomizer_FormClosed(object sender, FormClosedEventArgs e)
        {
            //do cleanup stuff here if necessary
        }
        #endregion

        #region Private Player class
        private class Player
        {
            public string Name { get; set; }
            public string IngameName { get; set; }
            public TextBox NameBox { get; set; }
            public TextBox IngameNameBox { get; set; }
            public bool IsVisible { get; set; }
            public bool IsTextVisible { get; set; }

            private TableLayoutPanel panel;

            public Player(TableLayoutPanel p, TextBox name, TextBox ingame)
            {
                panel = p;
                NameBox = name;
                IngameNameBox = ingame;
                Name = NameBox.Text;
                IsVisible = true;
                IsTextVisible = false;
                NameBox.Enter -= NameBox_Enter;
                NameBox.Enter += NameBox_Enter;
            }

            private void NameBox_Enter(object sender, EventArgs e)
            {
                NameBox.SelectAll();
            }

            public void Reset()
            {
                IngameName = string.Empty;
                IngameNameBox.Text = string.Empty;
                IsVisible = true;
                IsTextVisible = false;
                panel.Visible = IsVisible;
            }

            public void ToggleVisible()
            {
                IsVisible = !IsVisible;
                panel.Visible = IsVisible;
            }

            public void ToggleTextVisible()
            {
                IsTextVisible = !IsTextVisible;
                if (!string.IsNullOrEmpty(IngameName) && IsTextVisible)
                {
                    IngameNameBox.Text = IngameName;
                }
                else
                {
                    IngameNameBox.Text = "???";
                }
            }
        }
        #endregion
    }
}
