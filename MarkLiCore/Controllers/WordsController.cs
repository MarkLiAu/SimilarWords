using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordSimilarityLib;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestWebCore.api
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

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        private List<Word> ErrorInfo(string name, Exception ex)
        {
            Word w = new Word(name);
            w.meaningShort= ex.Message + ";" + ex.StackTrace;
            return new List<Word>() { w };
        }

        // GET api/<controller>/5
        [HttpGet("{name}")]
        public List<Word> Get(string name)
        {
            try
            {
                WordDictionary wd = new WordDictionary();
                if(WordDictionary.WordList.Count()<=0) 
                    wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/WordSimilarityList.txt"));

                List<Word> result =wd.FindSimilarWords(name);


                return result;
            }
            catch(Exception ex) 
            {
                return ErrorInfo(name, ex);
            }
        }


        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
