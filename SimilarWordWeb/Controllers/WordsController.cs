using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WordSimilarityLib;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimilarWordWeb.Controllers
{
    [Route("api/[controller]")]
    public class WordsController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/<controller>/5
        [HttpGet("{name}")]
        public List<Word> Get(string name)
        {
            try
            {
                WordDictionary wd = new WordDictionary();
                if (WordDictionary.WordList.Count <= 0)
                    wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

                if (WordDictionary.WordList.Count <= 0)
                    return new List<Word>() { new Word("error", -1, "no words in dictionary list") };

                if (name == "randomword") name = getRandomWord();
                List<Word> result = wd.FindSimilarWords(name);

                return result;
            }
            catch (Exception ex)
            {
                return new List<Word>() { new Word(name,-1,ex.Message+ex.StackTrace) };
            }
        }

        public string getRandomWord()
        {
            if (WordDictionary.WordList.Count <= 0) return "";
            List<string> list = WordDictionary.WordList.Where(x => x.Value.frequency > 2000 && x.Value.frequency < 5000).Select(x=>x.Value.name).ToList();
            if (list.Count <= 0) return "";
            Random rnd = new Random();
            int n = rnd.Next(0, list.Count - 1);

            return list[n];

        }


        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        [HttpPost]
        public string Post([FromBody] Word w)
        {
            return "Got ";
        }

        [Authorize]
        [HttpPut("{name}")]
        public Word Put(string name, [FromBody]Word word)
        {
            try
            {
                WordDictionary wd = new WordDictionary();
                if (WordDictionary.WordList.Count() <= 0)
                    wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

                if (word.similarWords == "") word.similarWords = wd.FindSimilarWord(word.name) + " ";   // put a space at the end, so no need to search next time
                Word originWord = WordDictionary.WordList[word.name.ToString().Trim()];
                if(originWord!=null)
                {
                    word.viewInterval = originWord.viewInterval;        // because when editing during memory, interval is changed to next interval
                }
                if (wd.UpdateWord(word)) return word;

                return new Word("ERROR", -1, "failed to update");

            }
            catch (Exception ex)
            {
                return new Word("ERROR", -1, ex.Message + ex.StackTrace);
            }

        }

        // DELETE api/<controller>/5
        [Authorize]
        [HttpDelete("{name}")]
        public string Delete(string name)
        {
            try
            {
                WordDictionary wd = new WordDictionary();
                if (WordDictionary.WordList.Count() <= 0)
                    wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

                if (wd.DeleteWord(name)) return "OK";

                return "ERROR:"+ "failed to update";

            }
            catch (Exception ex)
            {
                return "ERROR:"+ ex.Message + ex.StackTrace;
            }
        }



        ////////////////////////////////////////////////////////////// end
    }
}
