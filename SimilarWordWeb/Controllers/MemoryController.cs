using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WordSimilarityLib;
using System.IO;
using System.Text;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimilarWordWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MemoryController : Controller
    {
        string userId = "markli";
        string memoryMethod = "fib";

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Word> Get()
        {
            WordStudyModel wsModel = new WordStudyModel();
            wsModel.GetAuthorizedUser((ClaimsIdentity)User.Identity);
            if (wsModel._db != null) return wsModel.getViewList();


            WordDictionary wd = new WordDictionary();
            if (WordDictionary.WordList.Count() <= 0)
                wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));
            MemoryFibonacci memoryFib = new MemoryFibonacci(@"data\menory_" + memoryMethod + userId + ".txt");
            memoryFib.ReadMemoryLog();

            List<Word> result = memoryFib.getViewList(10);
            return result;
        }


        // GET api/<controller>/5
        [HttpGet("{count}")]
        public List<StudyLog> Get(string count)
        {
            try
            {
                WordStudyModel wsModel = new WordStudyModel();
                wsModel.GetAuthorizedUser((ClaimsIdentity)User.Identity);

                return wsModel.GetStudyLog(count) ;
            }
            catch (Exception ex)
            {
                StudyLog err = new StudyLog();
                err.name = "ERROR:" + ex.Message + ex.StackTrace;
                return new List<StudyLog>() { err };
            }
        }

        public List<MemoryLogFibonacci> GetForFile(string count)
        {
            try
            {
                WordDictionary wd = new WordDictionary();
                if (WordDictionary.WordList.Count() <= 0)
                    wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

                MemoryFibonacci memoryFib = new MemoryFibonacci(@"data\menory_" + memoryMethod + userId + ".txt");
                memoryFib.ReadMemoryLog();

                List<MemoryLogFibonacci> result = new List<MemoryLogFibonacci>();
                int maxCount = Convert.ToInt32(count);
                for (int i = 0; i < maxCount && i < memoryFib.logList.Count; i++)
                    result.Add(memoryFib.logList[memoryFib.logList.Count - 1 - i]);

                return result;
            }
            catch (Exception ex)
            {
                MemoryLogFibonacci err = new MemoryLogFibonacci();
                err.name = "ERROR:" + ex.Message + ex.StackTrace;
                return new List<MemoryLogFibonacci>() { err };
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
                WordStudyModel wsModel = new WordStudyModel();
                wsModel.GetAuthorizedUser((ClaimsIdentity)User.Identity);
                if (wsModel._db != null) return wsModel.UpdateMemoryItem(word);

                int interval = word.viewInterval;
                MemoryFibonacci memoryList = new MemoryFibonacci(@"data\menory_"+memoryMethod+userId+".txt");
                int rt;
                if (interval <= -1) rt = memoryList.StartNewItem(name);
                else rt = memoryList.UpdateMemoryItem(word);
                if (rt < 0) return "ERROR:" + name + " has already record";
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR:"+ ex.Message + ex.StackTrace;
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

        [HttpPost("csv")]
        public IActionResult MemoryLogCsv(string cmd)
        {
            StringBuilder sb = new StringBuilder();

            WordDictionary wd = new WordDictionary();
            if (WordDictionary.WordList.Count() <= 0)
                wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

            MemoryFibonacci memoryFib = new MemoryFibonacci(@"data\menory_" + memoryMethod + userId + ".txt");
            memoryFib.ReadMemoryLog();

            sb.AppendLine("name,time,interval,easiness");
            foreach( var log in memoryFib.logList)
            {
                sb.AppendLine(log.name+",");
                sb.AppendLine(log.viewTime.ToLocalTime() + ",");
                sb.AppendLine(log.viewInterval.ToString() + ",");
                sb.AppendLine(log.easiness.ToString() + ",");
            }

            //sb.AppendLine("1;2;3;");
            return File(System.Text.Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "WordMemoryLog.csv");
        }

        ////////////////////////////////////////////////////////////// end
    }
}
