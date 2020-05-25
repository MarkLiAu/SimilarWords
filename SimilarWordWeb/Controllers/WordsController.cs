using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

                List<Word> result = wd.FindSimilarWords(name);

                if (name == "bjfindallsimilarwords") wd.FindAllSimilarWords();
                if (name == "bjloadfile")   wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));


                return result;
            }
            catch (Exception ex)
            {
                return new List<Word>() { new Word(name,-1,ex.Message+ex.StackTrace) };
            }
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

        public Word PostAA([FromBody]Word word)
        {
            try
            {
                WordDictionary wd = new WordDictionary();
                if (WordDictionary.WordList.Count <= 0)
                    wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

                if (wd.UpdateWord(word)) return word;

                return new Word("ERROR", -1, "failed to update");

            }
            catch (Exception ex)
            {
                return new Word("ERROR", -1, ex.Message + ex.StackTrace);
            }

        }
        

        // PUT api/<controller>/5
        [HttpPut]
        public string PutAAA([FromBody]Word w)
        {
                Word word = new Word("test");
            return "done";

        }

        [HttpPut("{name}")]
        public Word Put(string name, [FromBody]Word word)
        {
            try
            {
                WordDictionary wd = new WordDictionary();
                if (WordDictionary.WordList.Count() <= 0)
                    wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

                //foreach (var d in WordDictionary.WordList) d.Value.meaningLong = "";
                //wd.SaveFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

                if (wd.UpdateWord(word)) return word;

                return new Word("ERROR", -1, "failed to update");

            }
            catch (Exception ex)
            {
                return new Word("ERROR", -1, ex.Message + ex.StackTrace);
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
