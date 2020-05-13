using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            testWordSimilarity();
        }

        private void testWordSimilarity()
        {
            WordSimilarityLib.WordDictionary wordDictionary = new WordSimilarityLib.WordDictionary();
            //wordDictionary.ReadCollins(Path.Combine(Directory.GetCurrentDirectory(), "data/CollinsL5E.txt"),1);
            //wordDictionary.ReadCollins(Path.Combine(Directory.GetCurrentDirectory(), "data/CollinsL4E.txt"), 1);
            //wordDictionary.ReadCollins(Path.Combine(Directory.GetCurrentDirectory(), "data/CollinsL3E.txt"), 1);
            //wordDictionary.ReadCollins(Path.Combine(Directory.GetCurrentDirectory(), "data/CollinsL2E.txt"), 1);
            //wordDictionary.ReadCollins(Path.Combine(Directory.GetCurrentDirectory(), "data/CollinsL1E.txt"), 1);
            wordDictionary.ReadCOCA20000Words(Path.Combine(Directory.GetCurrentDirectory(), "data/COCA-20000Words.txt"));
            wordDictionary.ReadCOCAFrequency(Path.Combine(Directory.GetCurrentDirectory(), "data/COCAFrequency20000.txt"));

            wordDictionary.SaveFile(Path.Combine(Directory.GetCurrentDirectory(), "data/WordSimilarityList.txt"));

            wordDictionary.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "data/WordSimilarityList.txt"));

            wordDictionary.FindSimilarWords("good");

        }

    }
}
