using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //WordSimilarityLib.SuperMemory2.test();
            // testWordSimilarity();
            //WordCount();
            WordSimilarityLib.MergeSubtitles.merge(@"C:\temp\Friends subtitles");
            Console.ReadKey();
        }

        static void WordCount()
        {
            Dictionary<string, int> wordlist = new Dictionary<string, int>();
            string[] files = Directory.GetFiles(@"C:\temp\", "*.srt", SearchOption.AllDirectories);
            foreach(string f in files)
            {
                Dictionary<string, int> list = WordSimilarityLib.WordCount.CountWords(File.ReadAllText(f), null);
                foreach (var d in list) wordlist[d.Key] = d.Value + (wordlist.ContainsKey(d.Key) ? wordlist[d.Key] : 0);
                Console.WriteLine($"word count:{list.Count}, total:{wordlist.Count}, f:{f}" );
            }

            File.WriteAllLines(@"c:\temp\WordList.csv", wordlist.OrderBy(x => x.Key).Select(x => x.Key + ", " + x.Value));
        }

        static void testWordSimilarity()
        {
            WordSimilarityLib.WordStudyModel model = new WordSimilarityLib.WordStudyModel();
            WordSimilarityLib.WordDictionary wordDictionary = new WordSimilarityLib.WordDictionary();

            wordDictionary.test1(@"..\..\..\data");

        }


    }
}
