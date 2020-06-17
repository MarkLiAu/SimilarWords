using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Security.Claims;

namespace WordSimilarityLib
{
    // for dashboard
    public class WordInfo
    {
        public string group { get; set; }
        public string name { get; set; }
        public string value { get; set; }

        public WordInfo()
        {
            group = name = value = "";
        }
        public WordInfo(string g, string n, string v)
        {
            group = g;
            name = n;
            value = v;
        }
    }


    public class StudyLog
    {
        public int userid { get; set; }
        public string deckname { get; set; }
        public string name { get; set; }    // name of the word
        public DateTime viewTime { get; set; }
        public int viewInterval { get; set; }
        public int easiness { get; set; }

        public StudyLog()
        {
            name = "";
        }
    }


    public class WordStudyModel 
    {
        const string dbname = "SimilarWords";

        public string _workPath { get; set; }
        public DbSqlite _db { get; set; }

        public UserProfile _user { get; set; }

        //public WordStudyModel():this(Directory.GetCurrentDirectory())
        //{
        //}

        public WordStudyModel(string workPath=null, string connString=null)
        {
            _workPath = string.IsNullOrWhiteSpace(workPath) ? Directory.GetCurrentDirectory() : workPath;
            _db = string.IsNullOrWhiteSpace(connString) ?
                new DbSqlite("DataSource="+Path.Combine(_workPath, $@"data\{dbname}.db"))
                :
                new DbSqlite(connString);
            _db.Open();
        }

        public List<Word> FindSimilarWords(string name)
        {
            List<Word> result = new List<Word>();


            List<Word> deckList = _user==null ? _db.Getwords(1,1) : _db.Getwords(_user.Id, _user.DeckId);

            if (name == "randomword")
            {
                Random rnd = new Random();
                int n = rnd.Next(0, deckList.Count - 1);
                name = deckList[n].name;
            }

            SortedList<string, Word> matchList = new SortedList<string, Word>();


            // find the word first
            Word w1st = new Word(name);
            w1st.meaningShort = "(not found)";

            string nameLowcase = name.ToLower();
            foreach (var w in deckList)
            {
                if (w.name == nameLowcase) { w1st = w; break; }
            }
                // search the list
            foreach (var w in deckList)
            {
                if (w.name == nameLowcase) continue; 
                double val =Word.WordCompare(nameLowcase, w.name);
                if (w1st.pronounciation.Length > 0 && w.pronounciation == w1st.pronounciation) val = 0.9; // same pronoun
                if (val < 0.7) continue;
                matchList.Add((1 - val).ToString("0.000000") + w.frequency.ToString("00000"), w);     // sort by compare Val and frequency
            }

            result.Add(w1st);

            foreach (var m in matchList) result.Add(m.Value);
            return result;
        }

        public Word UpdateWordPart(Word word, string part = "memory")
        {
            List<Word> originList = _db.Getwords(_user.Id, _user.DeckId, word.name);

            if (originList.Count <= 0) return _db.UpdateWord(_user.Id, _user.DeckId, word);

            Word originWord = originList[0];
            if (part == "memory"||part=="both"||part=="all")
            {
                originWord.easiness = word.easiness;
                originWord.startTime = word.startTime;
                originWord.totalViewed = word.totalViewed;
                originWord.viewInterval = word.viewInterval;
                originWord.viewTime = word.viewTime;
            }
            if (part == "both" || part == "all"|| part!="memory" )
            {
                originWord.name = word.name;
                originWord.pronounciation = word.pronounciation;
                originWord.frequency = word.frequency;
                originWord.similarWords = word.similarWords;
                originWord.meaningShort = word.meaningShort;
                originWord.meaningLong = word.meaningLong;
                originWord.meaningOther = word.meaningOther;
                originWord.soundUrl = word.soundUrl;
                originWord.exampleSoundUrl = word.exampleSoundUrl;
            }
            
            return _db.UpdateWord(_user.Id, _user.DeckId, originWord);
        }


        public UserProfile GetAuthorizedUser(ClaimsIdentity identity)
        {
            UserProfile userProfile = new UserProfile();

            IEnumerable<Claim> claims = identity.Claims;
            foreach (var c in claims)
            {
                if (c.Type == ClaimTypes.NameIdentifier) userProfile.Id = Convert.ToInt32(c.Value);
                else if (c.Type == ClaimTypes.Name) userProfile.Email = c.Value;
                else if (c.Type == ClaimTypes.GivenName) userProfile.FirstName = c.Value;
                else if (c.Type == ClaimTypes.Surname) userProfile.LastName = c.Value;
            }
            if (userProfile.DeckId < 1) userProfile.DeckId = 1;
            _user = userProfile;
            return userProfile;
        }


        public UserProfile GetUserProfile(string userKey)
        {
            UserProfile userProfile = _db.GetUserProfile(userKey);
            List<Deck> deckList = _db.GetUserDecks(userProfile.Id);
            if (deckList.Count > 0) userProfile.DeckId = deckList[0].Id;
            return userProfile;
        }

        public bool CreateUser(UserProfile user)
        {
            return _db.CreateUser(user);
        }

        public bool CreateDb()
        {
            return _db.CreateDb();
        }

        public string ProcessAdmin(string cmd)
        {
            return $"OK: now={DateTime.Now}, UTCNow={DateTime.UtcNow}";
        }


        public void ResetDb(bool resetUsers = false)
        {

            _db.ExecuteNonQuery("DELETE FROM decks; DELETE FROM logs; DELETE FROM words");
            if(resetUsers) _db.ExecuteNonQuery("DELETE FROM users");
        }

        public bool CreateDeck(Dictionary<string,Word> wordList, int shared=0)
        {
            _user.DeckId = -1;
            _user.DeckName = "Top 10000";
            return _db.CreateDeck(_user, wordList, shared);
        }

        // convert study log from file to sqlite
        public void ConvertLog()
        {
            WordDictionary wd = new WordDictionary();
            string userId = "markli";
            string memoryMethod = "fib";
            MemoryFibonacci memoryFib = new MemoryFibonacci(@"data\menory_" + memoryMethod + userId + ".txt");

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            memoryFib.ReadMemoryLog();
            foreach (var log in memoryFib.logList)
            {
                Word w = new Word(log.name);

                w.viewTime = log.viewTime;
                w.easiness = log.easiness;
                w.viewInterval = log.viewInterval;

                AddStudyLog(w);
            }
            sw.Stop();
            long t = sw.ElapsedMilliseconds / 1000;

        }



        /// <summary>
        /// study
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        public string StartNewItem(string name)
        {
            List<Word> originList = _db.Getwords(_user.Id, _user.DeckId, name);
            if (originList.Count <= 0) return "Error: can't find word: " + name;
            Word w = originList[0];
            if (w.viewInterval != int.MinValue) return "Error: already started word:"+name;        // already started

            w.viewTime = DateTime.UtcNow;
            w.viewInterval = -1;
            w.totalViewed = 0;
            w.startTime = DateTime.UtcNow;

            UpdateWordPart(w,"memory");
            AddStudyLog(w);

            return "OK";

        }

        public string UpdateMemoryItem(Word wordNew)
        {
            if (wordNew.viewInterval == -1) return StartNewItem(wordNew.name);

            List<Word> originList = _db.Getwords(_user.Id, _user.DeckId, wordNew.name);
            if (originList.Count <= 0) return "Error: can't fine word:"+wordNew.name;
            Word w = originList[0];

            if (w.viewInterval < 0) w.startTime = DateTime.UtcNow;   // this is 1st time view

            w.viewTime = DateTime.UtcNow;
            w.viewInterval = wordNew.viewInterval;
            w.easiness = wordNew.easiness;
            if (w.totalViewed < 0) w.totalViewed = 1;
            else w.totalViewed++;

            //            if (w.startTime == DateTime.MinValue) w.startTime = w.viewTime;     // something wrong

            UpdateWordPart(w,"memory");
            AddStudyLog(w);

            return "OK";
        }

        public List<Word> getViewList()
        {

            List<Word> result = new List<Word>();

            List<Word> originList = _db.Getwords(_user.Id, _user.DeckId);
            if (originList.Count <= 0) return result;

            int newItemCount = 0;
            foreach (var w in originList)
                if (w.is1stViewedToday()) newItemCount++;

            foreach (var word in originList)
            {
                if (word.isNewItem())
                {
                    if (newItemCount >= _user.MaxNewWord) continue;
                    result.Add(new Word(word));
                    newItemCount++;
                }
                else if (word.isDue())
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


        // calculate next interval
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

        ////////////
        /// get dashboard data        
        public List<WordInfo> GetDashboard()
        {
            List<WordInfo> result = new List<WordInfo>();

            List<Word> wordList = _db.Getwords(_user.Id, _user.DeckId);

            int[] counts = new int[10];

            DateTime start_date = DateTime.UtcNow;
            foreach (var w in wordList)
            {
                Word word = w;
                if (word.viewInterval < -1) continue;   // not started yet


                if (word.isNewItem())
                {
                    counts[1]++;    // new added items, ready for memorying
                    continue;
                }

                counts[3]++;    // all other viewed items
                if(word.totalViewed>0) counts[6] += word.totalViewed;       // total viewed including same word multiple times

                if (word.viewTime.Year > 2000 && word.viewTime < start_date) start_date = word.viewTime;

                if (word.isDue()) counts[2]++;    // already started items, and due to be viewed
                if (word.is1stViewedToday()) counts[4]++;  // 1st viewed today
                if (word.viewTime >= DateTime.Today) counts[5]++;   // all words viewed today
            }

            result.Add(new WordInfo("memory", "New words to view", (counts[1]).ToString()));
            result.Add(new WordInfo("memory", "old words due", (counts[2]).ToString()));
            result.Add(new WordInfo("memory", "other viewed words", (counts[3]).ToString()));
            result.Add(new WordInfo("memory", "new words today", (counts[4]).ToString()));
            result.Add(new WordInfo("memory", "reviewed today", (counts[5]).ToString()));
            result.Add(new WordInfo("word", "total words", (wordList.Count).ToString()));
            start_date = new DateTime(start_date.Year, start_date.Month, start_date.Day);
            result.Add(new WordInfo("memory", "total days", ((DateTime.UtcNow - start_date).Days + 1).ToString()));
            result.Add(new WordInfo("memory", "reviewed today", (counts[6]).ToString()));

            return result;
        }


        /// strudy log
        public int AddStudyLog(Word word)
        {
            string cmdString = "INSERT INTO logs (userid,deckid, name, study_time, interval, easiness) "
                             + $" VALUES ({_user.Id},{_user.DeckId}, '{word.name}','{word.viewTime.ToString("o")}',{word.viewInterval},{word.easiness}  )";
            int rc= _db.ExecuteNonQuery(cmdString);
            return rc;
        }

        public List<StudyLog> GetStudyLog(string count, string name=null)
        {
            List<StudyLog> logList = new List<StudyLog>();
            //cmdString = "CREATE TABLE IF NOT EXISTS decks ( id INTEGER PRIMARY KEY, name TEXT ,  ownerid INT , userid INT, max_new_word INT, shared INT ) ";
            //cmdString = "CREATE TABLE IF NOT EXISTS logs ( userid INT, deckid INT, name TEXT PRIMARY KEY,  study_time TEXT, interval INT, easiness INT ) WITHOUT ROWID;";

            string timeBack = DateTime.UtcNow.AddDays(-Convert.ToInt32(count)).ToString("o");
            string cmdString = $"SELECT logs.*, decks.name FROM logs LEFT OUTER JOIN decks ON logs.deckid = decks.id WHERE logs.userid={_user.Id} AND logs.deckid={_user.DeckId} ";
            cmdString += $" AND study_time>='{timeBack}'";
            if (!string.IsNullOrWhiteSpace(name)) cmdString += " AND name = '{name} ";
            cmdString += " ORDER BY logs.id DESC ";
            List<List<object>> data = _db.GetData(cmdString);
            foreach(var row in data)
            {
                if (row.Count < 8) continue;
                StudyLog log = new StudyLog();
                log.name = row[3].ToString();
                log.viewTime = Convert.ToDateTime(row[4]);
                log.viewInterval = Convert.ToInt32(row[5]);
                log.easiness = Convert.ToInt32(row[6]);
                log.deckname = row[7] != null ? row[7].ToString() : "" ;
                logList.Add(log); 
            }

            // group by viewTime
            var group = from d in logList
                        group d by new DateTime(d.viewTime.Year, d.viewTime.Month, d.viewTime.Day) into g
                        orderby g.Key
                        select new StudyLog { name = "0", viewTime = g.Key, easiness = g.Count() }
                        ;
            logList.AddRange(group);
            return logList;
        }

    }
}
