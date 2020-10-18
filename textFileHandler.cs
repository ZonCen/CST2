using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CST_With_only_excel
{
    public class textFileHandler
    {
        string fileName;
        string pathString;

        public  void createFile()
        {
            string folderName = @"c:\CST backup";
            pathString = System.IO.Path.Combine(folderName, "textFiler");
            DateTime thisday = DateTime.Today;
            System.IO.Directory.CreateDirectory(pathString);
            fileName = thisday.ToString("D") + ".txt";

            pathString = System.IO.Path.Combine(pathString, fileName);

            if (!File.Exists(pathString))
            {
                using(StreamWriter fs = new StreamWriter(pathString))
                {
                    fs.WriteLine("Start på filen");
                }
            }

        }

        public void updateFile(string player1Name, string player2Name, int ScorePlayer1, int ScorePlayer2, int division)
        {
            if (!System.IO.File.Exists(pathString))
            {
                using (StreamWriter fs = new StreamWriter(pathString))
                {
                    fs.WriteLine("Start på filen");
                }
            }
            else
            {
                using (StreamWriter fs = File.AppendText(pathString))
                {
                    fs.WriteLine(player1Name + " " + ScorePlayer1 + " - " + ScorePlayer2 + " " + player2Name + " Division" + division);
                }
            }
        }

        public string[] readFile()
        {
            string[] lines = File.ReadAllLines(pathString);
            return lines;
        }
    }
}
