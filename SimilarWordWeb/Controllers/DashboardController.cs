using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using WordSimilarityLib;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimilarWordWeb.Controllers
{

    public class WordInfo
    {
        public string group { get; set; }
        public string name { get; set; }
        public string value { get; set; }

        public WordInfo()
        {
            group = name = value = "";
        }
        public WordInfo(string g, string n,string v)
        {
            group = g;
            name = n;
            value = v;
        }
    }

    [Authorize]
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        string userId = "markli";
        string memoryMethod = "fib";

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<WordInfo> Get()
        {
            List<WordInfo> result = new List<WordInfo>();

            WordDictionary wd = new WordDictionary();
            if (WordDictionary.WordList.Count <= 0)
                wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

            MemoryFibonacci memoryFib = new MemoryFibonacci(@"data\menory_" + memoryMethod + userId + ".txt");

            int[] counts = new int[10];

            DateTime start_date = DateTime.Now;
            foreach (var w in WordDictionary.WordList)
            {
                Word word = w.Value;
                if (word.viewInterval < -1) continue;   // not started yet


                if (memoryFib.isNewItem(word))
                {
                    counts[1]++;    // new added items, ready for memorying
                    continue;
                }

                counts[3]++;    // all other viewed items

                if (word.viewTime < start_date) start_date = word.viewTime;

                if (memoryFib.isDue(word)) counts[2]++;    // already started items, and due to be viewed
                if (memoryFib.is1stViewedToday(word)) counts[4]++;  // 1st viewed today
                if (word.viewTime >= DateTime.Today) counts[5]++;   // all words viewed today
            }

            result.Add(new WordInfo("memory", "New words to view", (counts[1]).ToString()));
            result.Add(new WordInfo("memory", "old words due", (counts[2]).ToString()));
            result.Add(new WordInfo("memory", "other viewed words", (counts[3]).ToString()));
            result.Add(new WordInfo("memory", "new words today", (counts[4]).ToString()));
            result.Add(new WordInfo("memory", "reviewed today", (counts[5]).ToString()));
            result.Add(new WordInfo("word", "total words", (WordDictionary.WordList.Count).ToString()));
            result.Add(new WordInfo("memory", "total days", (DateTime.Now-start_date).Days.ToString()));

            return result;
        }

        // GET api/<controller>/5
        [HttpGet("{name}")]
        public List<Word> Get(string name)
        {
            try
            {
                WordDictionary wd = new WordDictionary();
                if (WordDictionary.WordList.Count() <= 0)
                    wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

                List<Word> result = wd.FindSimilarWords(name);


                return result;
            }
            catch (Exception ex)
            {
                return new List<Word>() { new Word(name, -1, ex.Message + ex.StackTrace) };
            }
        }


        // POST api/<controller>
        [HttpPost]
        public string Post([FromBody] Word w)
        {
            return "Got ";
        }



        [HttpPut("{name}")]
        public string Put(string name, [FromBody]Word word)
        {
            try
            {
                int interval = word.viewInterval;
                MemoryFibonacci memoryList = new MemoryFibonacci(@"data\menory_" + memoryMethod + userId + ".txt");
                int rt;
                if (interval <= -1) rt = memoryList.StartNewItem(name);
                else rt = memoryList.UpdateMemoryItem(word);
                if (rt < 0) return "ERROR:" + name + " has already record";
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message + ex.StackTrace;
            }

        }

        // DELETE api/<controller>/5
        [HttpDelete("{name}")]
        public string Delete(string name)
        {
            try
            {
                WordDictionary wd = new WordDictionary();
                if (WordDictionary.WordList.Count() <= 0)
                    wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

                if (wd.DeleteWord(name)) return "OK";

                return "ERROR:" + "failed to update";

            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message + ex.StackTrace;
            }
        }



        ////////////////////////////////////////////////////////////// end
    }
}
