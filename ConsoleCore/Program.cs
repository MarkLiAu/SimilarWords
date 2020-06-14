using System;
using System.IO;

namespace ConsoleCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //WordSimilarityLib.SuperMemory2.test();
            testWordSimilarity();
        }


        static void testWordSimilarity()
        {
            WordSimilarityLib.WordStudyModel model = new WordSimilarityLib.WordStudyModel();
            WordSimilarityLib.WordDictionary wordDictionary = new WordSimilarityLib.WordDictionary();

            wordDictionary.test1(@"..\..\..\data");

        }


    }
}
