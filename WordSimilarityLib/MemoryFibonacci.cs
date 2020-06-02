using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WordSimilarityLib
{
    public class MemoryLogFibonacci
    {
        public string name { get; set; }
        public DateTime viewTime { get; set; }
        public int viewInterval { get; set; }
        public int easiness { get; set; }

        public MemoryLogFibonacci()
        {
            name = "";
        }
    }

    public class MemoryFibonacci
    {
        public string fileName { get; set; }
        public List<Word> memoryList { get; set; }
        public List<MemoryLogFibonacci> logList { get; set; }

        public MemoryFibonacci()
        {
            fileName = "memory_default.txt";
            memoryList = new List<Word>();
            logList = new List<MemoryLogFibonacci>();
        }

        public MemoryFibonacci(string file): this()
        {
            fileName = file;
        }


        public bool isNewItem(Word word)
        {
            return word.viewInterval == -1;
        }

        public bool isDue(Word word)
        {
            if (word.viewInterval<=-1) return false;     // not start yet, also exclude new word (==-1)
            //if (word.viewInterval==0 && word.totalViewed == 1 && word.viewTime.AddMinutes(5) > DateTime.Now) return true;     // first time, due after 5 minutes
            if (DateTime.Now.Hour >= 6 && word.viewTime.AddDays(word.viewInterval) <= DateTime.Today.AddHours(23)) return true; // in the morning, 
            return word.viewTime.AddDays(word.viewInterval) <= DateTime.Now;
        }

        // check is the word is 1st time viewed today
        public bool is1stViewedToday(Word word)
        {
            if (word.viewInterval <= -1) return false;     // not start yet, also exclude new word (==-1)
            return word.startTime >= DateTime.Today;
        }

        public int StartNewItem(string name)
        {
            WordDictionary wd = new WordDictionary();
            if (!WordDictionary.WordList.ContainsKey(name.ToLower())) return -1;
            Word w = WordDictionary.WordList[name.ToLower()];
            if (w.viewInterval!=int.MinValue) return -2;        // already started

            w.viewTime = DateTime.Now;
            w.viewInterval = -1;
            w.totalViewed = 0;
            w.startTime = DateTime.Now;

            wd.UpdateWord(w);
            AddMemoryLog(w);

            return w.viewInterval;
        }

        public int UpdateMemoryItem(Word wordNew)
        {
            //if (wordNew.viewInterval == -1) return StartNewItem(wordNew.name);
            if (wordNew.viewInterval<0) return -2;        // something wrong, this one not started yet

            WordDictionary wd = new WordDictionary();
            if (!WordDictionary.WordList.ContainsKey(wordNew.name.ToLower())) return -1;
            Word w = WordDictionary.WordList[wordNew.name.ToLower()];

            if(w.viewInterval<0)  w.startTime = DateTime.Now;   // this is 1st time view

            w.viewTime = DateTime.Now;
            w.viewInterval = wordNew.viewInterval;
            w.easiness = wordNew.easiness;
            w.totalViewed++;

//            if (w.startTime == DateTime.MinValue) w.startTime = w.viewTime;     // something wrong

            wd.UpdateWord(w);
            AddMemoryLog(w);

            return w.viewInterval;
        }

        public int AddMemoryLog(Word word, string delimeter = "\t")
        {
            string log = word.name + delimeter + word.viewTime.ToBinary() + delimeter + word.viewInterval + delimeter + word.easiness;

            File.AppendAllLines(fileName, new string[] { log });
            return 1;
        }

        public int ReadMemoryLog(string file = "")
        {
            logList.Clear();

            if (string.IsNullOrWhiteSpace(file)) file = fileName;
            if (string.IsNullOrWhiteSpace(file) || !File.Exists(file)) return -1;
            string[] lines = File.ReadAllLines(file);
            if (lines.Length <= 0) return 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] ss = lines[i].Split(new char[] { ',', ';', '\t' });
                if (ss.Length < 3) continue;

                MemoryLogFibonacci log = new MemoryLogFibonacci();
                log.name = ss[0].Trim();
                log.viewTime = DateTime.FromBinary(Convert.ToInt64( ss[1]));
                log.viewInterval = Convert.ToInt32(ss[2]);
                log.easiness = Convert.ToInt32(ss[3]);
                logList.Add(log);
            }
            return logList.Count;
        }

        public void ClearViewHistory()
        {
            if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName)) File.Delete(fileName);
            WordDictionary wd = new WordDictionary();
            Word defaultWord = new Word();
            foreach (var d in WordDictionary.WordList)
            {
                d.Value.viewTime = defaultWord.viewTime;
                d.Value.totalViewed = defaultWord.totalViewed;
                d.Value.viewInterval = defaultWord.viewInterval;
                d.Value.easiness = defaultWord.easiness;
                d.Value.startTime = defaultWord.startTime;
            }
            wd.SaveFile(WordDictionary.dataFile);
        }

        public List<Word> getViewList(int maxNewItem=10)
        {
            WordDictionary wd = new WordDictionary();
            if (WordDictionary.WordList.Count <= 0)
                wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

            //ClearViewHistory();

            List<Word> result = new List<Word>();
            Dictionary<string, int> namelist = new Dictionary<string, int>();

            int newItemCount = 0;
            foreach (var w in WordDictionary.WordList)
                if (is1stViewedToday(w.Value)) newItemCount++;

            foreach (var w in WordDictionary.WordList)
            {
                Word word = w.Value;
                if (isNewItem(word))
                {
                    if (newItemCount >= maxNewItem) continue;
                    result.Add(new Word(word));
                    newItemCount++;
                }
                else if(isDue(word))
                {
                    result.Add(new Word(word));
                }
            }

            // calculate next interval
            foreach (var w in result)
            {
                w.viewInterval = CalculateNextInterval(w);
            }
            return result;
        }

        public int CalculateNextInterval(Word word)
        {
            int newVal = word.viewInterval;
            if (word.viewInterval < 0)
            {
                newVal = 0;
            }
            else if (word.viewInterval == 0)
            {
                if (word.totalViewed <= 0) newVal = 0;  // first time viewed
                else newVal = 1;
            }
            else
            {
                double v = word.viewInterval * 1.618;
                newVal = Convert.ToInt32(v);
                if (newVal < 1) newVal = 1;
            }
            return newVal;
        }

        //////////////////////////////////////////////////////////////////////
    }
}
