using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CST_With_only_excel
{

    public partial class Form1 : Form
    {
        NewExcelHandler ExcelHandling = new NewExcelHandler();
        textFileHandler textHandling = new textFileHandler();
        List<Player> players = new List<Player>();
        List<Player> Division1 = new List<Player>();
        List<Player> Division2 = new List<Player>();
        List<Player> Division3 = new List<Player>();
        List<Player> Division4 = new List<Player>();
        List<Player> Division5 = new List<Player>();
        List<Player> Division6 = new List<Player>();
        int round = 1;
        public Form1()
        {
            InitializeComponent();
            textHandling.createFile();
            round = ExcelHandling.checkRound();
            players = ExcelHandling.ImportPlayers();
            ExcelHandling.fillRows(players);
            for (int i = 1; i <= 6; i++)
            {
                DivisionPicker.Items.Add(i);
            }
            DivisionPicker.SelectedIndex = 0;

            if (round == 1)
            {
                int i = 1;
                foreach (var p in players)
                {
                    p.fakeRank = i;
                    p.lastPlacement = i;
                }
            }

            players = players.OrderBy(x => x.lastPlacement).ToList();

            Report.Enabled = false;
            Finish_Button.Enabled = false;


            foreach (var p in players)
            {
                if (p.division == 1)
                {
                    Division1.Add(p);
                }
                if (p.division == 2)
                {
                    Division2.Add(p);
                }
                if (p.division == 3)
                {
                    Division3.Add(p);
                }
                if (p.division == 4)
                {
                    Division4.Add(p);
                }
                if (p.division == 5)
                {
                    Division5.Add(p);
                }
                if (p.division == 6)
                {
                    Division6.Add(p);
                }
            }

            players = players.OrderBy(x => x.row).ToList();
            ExcelHandling.GetDivisionRank(players);

        }

        private void ReportDivision1_Click(object sender, EventArgs e)
        {
            //player 1
            var player1 = players.Where(x => x.fullName.Contains(Player1.SelectedItem.ToString())).First();
            int scoreP1Game1 = Convert.ToInt32(ScorePlayer1Division1.Value);
            int scoreP1Game2 = Convert.ToInt32(ScorePlayer1Division1Game2.Value);
            //player 2
            var player2 = players.Where(x => x.fullName.Contains(Player2.SelectedItem.ToString())).First();
            int scoreP2Game1 = Convert.ToInt32(ScorePlayer2Division1.Value);
            int scoreP2Game2 = Convert.ToInt32(ScorePlayer2Division1Game2.Value);

            int diffGame1P1 = scoreP1Game1 - scoreP2Game1;
            int diffgame2P1 = scoreP1Game2 - scoreP2Game2;

            int diffGame1P2 = scoreP2Game1 - scoreP1Game1;
            int diffgame2p2 = scoreP2Game2 - scoreP1Game2;

            if (player1.playedVs[0] != player2.fullName && player1.playedVs[1] != player2.fullName)
            {
                if ((diffGame1P1 <= 11 && diffGame1P1 >= 2) && (diffgame2P1 <= 11 && diffgame2P1 >= 2)
                    || (diffGame1P2 <= 11 && diffGame1P2 >= 2) && (diffgame2p2 <= 11 && diffgame2p2 >= 2)
                    || (diffGame1P1 <= 11 && diffGame1P1 >= 2) && (diffgame2p2 <= 11 && diffgame2p2 >= 2)
                    || (diffGame1P2 <= 11 && diffGame1P2 >= 2) && (diffgame2P1 <= 11 && diffgame2P1 >= 2))
                {
                    player1.Rapport(scoreP1Game1, scoreP2Game1, scoreP1Game2, scoreP2Game2, player2.divisionRank);
                    player1.CalculateWins(scoreP1Game1, scoreP2Game1, scoreP1Game2, scoreP2Game2);
                    player2.Rapport(scoreP2Game1, scoreP1Game1, scoreP2Game2, scoreP1Game2, player1.divisionRank);
                    player2.CalculateWins(scoreP2Game1, scoreP1Game1, scoreP2Game2, scoreP1Game2);

                    player1.gamePlayed += 1;
                    player1.UpdateVs(player2);
                    player2.gamePlayed += 1;
                    player2.UpdateVs(player1);

                    textHandling.updateFile(player1.name, player2.name, scoreP1Game1, scoreP2Game1, player1.division);
                    textHandling.updateFile(player1.name, player2.name, scoreP1Game2, scoreP2Game2, player1.division);

                    Reported_Games.Clear();
                    string[] reportedGames = textHandling.readFile();
                    foreach(var l in reportedGames)
                    {
                        Reported_Games.Text += l;
                        Reported_Games.Text += Environment.NewLine;
                    }
                                      

                    MessageBox.Show("Matchen rapporterad");
                }
                else
                {
                    MessageBox.Show("Resultat mellan spelarna måste vara mellan 2 och 11");
                }

                foreach (var p in players)
                {
                    if (p.gamePlayed == 2)
                    {
                        Finish_Button.Enabled = true;
                    }
                    else
                    {
                        Finish_Button.Enabled = false;
                    }
                }

            }
            else
            {
                MessageBox.Show("Spelare har redan möts");
            }


        }

        private void Player1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Player1.SelectedItem != Player2.SelectedItem) && (Player1.SelectedItem != null && Player2.SelectedItem != null))
            {
                Report.Enabled = true;
            }
            else
            {
                Report.Enabled = false;
            }
        }

        private void Player2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Player1.SelectedItem != Player2.SelectedItem) && (Player1.SelectedItem != null && Player2.SelectedItem != null))
            {
                Report.Enabled = true;
            }
            else
            {
                Report.Enabled = false;
            }
        }

        private void DivisionPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedDivision = Convert.ToInt32(DivisionPicker.SelectedItem);
            Player1.Items.Clear();
            Player2.Items.Clear();
            Report.Enabled = false;
            
            foreach(var p in players)
            {
                if(p.division == selectedDivision)
                {
                    Player1.Items.Add(p.fullName);
                    Player2.Items.Add(p.fullName);
                }
            }
        }

        private void Finish_Button_Click(object sender, EventArgs e)
        {
            ExcelHandling.UpdateCPlacement(players);
            players = players.OrderBy(x => x.newPlacement).ToList();
            ExcelHandling.UpdatePlacement(players); //Adds the placement to the Excel file

            foreach(var p in players)
            {
                p.GetNewPlacement();
                p.calculateAverage();
            }

            ExcelHandling.UpdateNextWeek(round, players);

            players = players.OrderBy(x => x.average).ToList();
            ExcelHandling.UpdateRankTable(players);

            Finish_Button.Enabled = false;

        }
    }
}
