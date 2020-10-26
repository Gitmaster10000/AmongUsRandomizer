using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AmongUsRandom
{
    public partial class AmongUsRandomizer : Form
    {
        Player p1, p2, p3, p4, p5, p6, p7, p8, p9, p10;
        Player[] allPlayers = new Player[10];
        int playerCnt = 10;
        public AmongUsRandomizer()
        {
            InitializeComponent();

            p1 = new Player(tableLayoutPanel4, textBox1, textBox2);
            p2 = new Player(tableLayoutPanel5, textBox4, textBox3);
            p3 = new Player(tableLayoutPanel6, textBox6, textBox5);
            p4 = new Player(tableLayoutPanel7, textBox8, textBox7);
            p5 = new Player(tableLayoutPanel8, textBox10, textBox9);
            p6 = new Player(tableLayoutPanel9, textBox12, textBox11);
            p7 = new Player(tableLayoutPanel10, textBox14, textBox13);
            p8 = new Player(tableLayoutPanel11, textBox16, textBox15);
            p9 = new Player(tableLayoutPanel12, textBox18, textBox17);
            p10 = new Player(tableLayoutPanel13, textBox20, textBox19);
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
        }


        #region events
        

        private void buttonRandomize_Click(object sender, EventArgs e)
        {
            //randomize
            List<Player> players = new List<Player>();
            foreach(Player p in allPlayers)
            {
                if (p.IsVisible)
                {
                    p.Name = p.NameBox.Text;
                    p.IngameName = string.Empty;
                    p.IngameNameBox.Text = string.Empty;
                    players.Add(p);
                    if (string.IsNullOrWhiteSpace(p.Name))
                    {
                        throw (new Exception("Every player needs to have a name set!"));
                        return;
                    }
                }
            }
            Player[] playerList = new Player[playerCnt];
            int curPlayer = 0;
            while (players.Count > 0)
            {
                Random r = new Random();
                int randomNumber = r.Next(1, players.Count) - 1;
                playerList[curPlayer] = players[randomNumber];
                players.RemoveAt(randomNumber);
                curPlayer++;
            }

            //The last player is the first player in the list
            playerList[playerCnt - 1].IngameName = playerList[0].Name;
            //set ingame names for all the other players to the next in the list
            for(int i = 0; i<playerCnt-1; i++)
            {
                playerList[i].IngameName = playerList[i+1].Name;
            }

            //copy to clipboard
            string clipboardString = string.Empty;
            foreach (Player p in allPlayers)
            {
                if (p.IsVisible)
                {
                    int spaceCnt = 15 - p.IngameName.Length;
                    clipboardString += p.Name + " is ||" + p.IngameName + new string(' ', spaceCnt) + "||\n";
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
            Clipboard.SetText(clipboardString);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            string clipboardString = string.Empty;
            foreach(Player p in allPlayers)
            {
                if (p.IsVisible)
                {
                    int spaceCnt = 30 - p.IngameName.Length;
                    clipboardString += p.Name + " is ||"+ p.IngameName + new string(' ', spaceCnt)+"||\n";

                }
            }
            Clipboard.SetText(clipboardString);
        }
        private void buttonReset_Click(object sender, EventArgs e)
        {
            foreach(Player p in allPlayers)
            {
                p.Reset();
            }
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
            if (playerCnt == 1)
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
            if(playerCnt == 10)
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

        #endregion
    }

    public class Player
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
        }

        public void Reset()
        {
            IngameName = string.Empty;
            IngameNameBox.Text = string.Empty;
        }

        public void ToggleVisible()
        {
            IsVisible = !IsVisible;
            panel.Visible = IsVisible;
        }

        public void SetIngameName(string name)
        {
            IngameName = name;
            if (IsTextVisible)
            {
                IngameNameBox.Text = name;
            }
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
}
