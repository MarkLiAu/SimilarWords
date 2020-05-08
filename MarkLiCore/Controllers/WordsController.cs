using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

        private Word ErrorInfo(string name, Exception ex)
        {
            Word w = new Word(name);
            w.explanationShort=w.explanationLong = ex.Message + ";" + ex.StackTrace;
            return w;
        }

        // GET api/<controller>/5
        [HttpGet("{name}")]
        public Word Get(string name)
        {
            try
            {
                Words words = new Words();
                Word word = words.search(name);

                return word;
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
