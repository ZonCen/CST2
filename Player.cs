﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CST_With_only_excel
{
    public class Player
    {
        NewExcelHandler test = new NewExcelHandler();
        public string name;
        public string lastName;
        public string fullName;

        public int rank;
        public int division;
        public int newPlacement;
        public int lastPlacement = 1;
        public int fakeRank;
        public int round = 1;
        public double average = 0;
        public int divisionRank = 0;

        public int row = 0;
        public int rankingRow = 0;

        string[,] Columns = new string[10, 2];
        public string[] playedVs = new string[2];
        public List<int> rounds = new List<int>();
        public int gamePlayed = 0;

        //Score
        public int wins = 0;
        public int game = 0;
        public int score = 0;

        //Data for User
        public Player(string Fullname, int Round)
        {
            fullName = Fullname;

            string[] tokens = Fullname.Split(new[] { "\r\n" }, StringSplitOptions.None);
            name = tokens[0];
            lastName = tokens[1];
            round = test.checkRound();

            getRankingRow();
            GetRow();
            getRounds();

        }

        public void getRounds()
        {
            rounds.AddRange(test.GetRounds(rankingRow));
        }

        public void getRankingRow()
        {
            rankingRow = test.GetRankingRowNumber(fullName, round);
        }

        public void GetRow()
        {
            row = test.GetRowNumber(name, round);

            Columns[0, 0] = "B" + row;
            Columns[1, 0] = "D" + row;
            Columns[2, 0] = "E" + row;
            Columns[3, 0] = "G" + row;
            Columns[4, 0] = "H" + row;
            Columns[5, 0] = "J" + row;
            Columns[6, 0] = "L" + row;
            Columns[7, 0] = "M" + row;
            Columns[8, 0] = "N" + row;
            Columns[9, 0] = "O" + row;

            Columns[0, 1] = "B" + (row + 1);
            Columns[1, 1] = "D" + (row + 1);
            Columns[2, 1] = "E" + (row + 1);
            Columns[3, 1] = "G" + (row + 1);
            Columns[4, 1] = "H" + (row + 1);
            Columns[5, 1] = "J" + (row + 1);
            Columns[6, 1] = "L" + (row + 1);
            Columns[7, 1] = "M" + (row + 1);
            Columns[8, 1] = "N" + (row + 1);
            Columns[9, 1] = "O" + (row + 1);

            GetLastPlacement();

            switch (lastPlacement)
            {
                case 1:
                case 2:
                case 4:
                    division = 1;
                    break;
                case 3:
                case 5:
                case 7:
                    division = 2;
                    break;
                case 6:
                case 8:
                case 10:
                    division = 3;
                    break;
                case 9:
                case 11:
                case 13:
                    division = 4;
                    break;
                case 12:
                case 14:
                case 15:
                case 16:
                    division = 5;
                    break;
            }
        }

        public void GetLastPlacement()
        {
            if (round > 1)
                lastPlacement = test.CheckLastPlacement((round - 1), name);
        }

        public void GetNewPlacement()
        {
            newPlacement = test.CheckPlacement(Columns[9, 0], round);
            rounds.Add(newPlacement);
        }

        public void UpdateVs(Player player2)
        {
            if (playedVs[0] != player2.fullName && playedVs[1] != player2.fullName)
            {
                if (playedVs[0] == null)
                {
                    playedVs[0] = player2.fullName;
                }
                else if (playedVs[1] == null)
                {
                    playedVs[1] = player2.fullName;
                }
            }
        }

        public void calculateAverage()
        {
            foreach (var a in rounds)
            {
                average += a;
            }

            average = average / rounds.Count;
        }


        public void Rapport(int Player1Score1, int player2Score1, int player1Score2, int player2Score2, int player2DivisionRank)
        {

            string firstColum = "";
            string secondColumn = "";
            string lowerFirstColumn = "";
            string lowerSecondColumn = "";

            if ((divisionRank == 1 || divisionRank == 3) && player2DivisionRank == 2)
            {
                firstColum = Columns[2, 0];
                secondColumn = Columns[3, 0];
                lowerFirstColumn = Columns[2, 1];
                lowerSecondColumn = Columns[3, 1];
            }
            else if ((divisionRank == 1 || divisionRank == 2) && player2DivisionRank == 3)
            {
                firstColum = Columns[4, 0];
                secondColumn = Columns[5, 0];
                lowerFirstColumn = Columns[4, 1];
                lowerSecondColumn = Columns[5, 1];
            }
            else if ((divisionRank == 2 || divisionRank == 3) && player2DivisionRank == 1)
            {
                firstColum = Columns[0, 0];
                secondColumn = Columns[1, 0];
                lowerFirstColumn = Columns[0, 1];
                lowerSecondColumn = Columns[1, 1];
            }

            test.UpdateScoreTable(firstColum, secondColumn, lowerFirstColumn, lowerSecondColumn, Player1Score1, player2Score1, player1Score2, player2Score2);
        }

        public void CalculateWins(int ScorePlayer1Game1, int ScorePlayer2Game1, int ScorePlayer1Game2, int ScorePlayer2Game2)
        {
            if (ScorePlayer1Game1 > ScorePlayer2Game1 && ScorePlayer1Game2 > ScorePlayer2Game2)
            {
                wins++;
                game += 2;
            }
            else if ((ScorePlayer1Game1 > ScorePlayer2Game1) || (ScorePlayer1Game2 > ScorePlayer2Game2))
            {
                game += 1;
            }

            int gamediffGame1 = ScorePlayer1Game1 - ScorePlayer2Game1;
            int gamediffGame2 = ScorePlayer1Game2 - ScorePlayer2Game2;
            score += gamediffGame1 + gamediffGame2;

            test.UpdateWins(row, round, wins, game, score);
        }
    }
}
