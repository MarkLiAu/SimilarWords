using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using WordSimilarityLib;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimilarWordWeb.Controllers
{

    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        string userId = "markli";
        string memoryMethod = "fib";

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new List<string>(new string[] { "ok1", "ok2" });
        }


        // POST api/<controller>
        [HttpPost]
        public string Post([FromBody] Word w)
        {
            return "Got ";
        }

        [Authorize]
        [HttpPut("{cmd}")]
        public string Put(string cmd, [FromBody]string note)
        {
            try
            {
                //var user = User.ToString();

                WordStudyModel wsModel = new WordStudyModel();
                wsModel.GetAuthorizedUser((ClaimsIdentity)User.Identity);

                WordDictionary wd = new WordDictionary();
                string userId = "markli";
                string memoryMethod = "fib";
                MemoryFibonacci memoryFib = new MemoryFibonacci(@"data\menory_" + memoryMethod + userId + ".txt");

                if (cmd.ToLower() == "reload")
                {
                    wd.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), @"data\WordSimilarityList.txt"));

                    return "OK:" + DateTime.Now.ToString();
                }
                else if (cmd.ToLower() == "resetmemory")
                {
                    memoryFib.ClearViewHistory();
                    return "OK:" + DateTime.Now.ToString();
                }
                else if (cmd.ToLower() == "fixmemory")
                {
                    wsModel._user.DeckId = -1;
                    wsModel.CreateDeck(WordDictionary.WordList);
                    return "OK:" + DateTime.Now.ToString();
                }
                else if (cmd.ToLower() == "fixmemory_OLD")
                {
                    foreach (var d in WordDictionary.WordList) d.Value.totalViewed = int.MinValue;
                    memoryFib.ReadMemoryLog();
                    foreach (var log in memoryFib.logList)
                    {
                        if (!WordDictionary.WordList.ContainsKey(log.name)) continue;
                        Word w = WordDictionary.WordList[log.name];
                        if (w.totalViewed < 0) w.totalViewed = 0;
                        if (log.viewInterval >= 0) w.totalViewed++;
                        if (w.startTime.Year < 2020 && log.viewInterval >= 0) w.startTime = log.viewTime;
                        if (w.viewTime.Year < 2020 && log.viewInterval >= 0) w.viewTime = log.viewTime;
                        if (w.easiness < -99 && log.easiness > 99) w.easiness = log.easiness;
                        if (w.viewInterval < -99 && log.viewInterval > 99)
                            w.viewInterval = log.viewInterval;
                    }
                    wd.SaveFile(WordDictionary.dataFile);
                    return "OK";
                }
                else
                {
                    return "OK: not supported command:" + cmd;
                }
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
