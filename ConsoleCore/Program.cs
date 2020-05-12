using System;
using System.IO;

namespace ConsoleCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            testWordSimilarity();
        }


        static void testWordSimilarity()
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
