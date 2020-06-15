using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Claims;

namespace WordSimilarityLib
{

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

        public bool CreateDeck(Dictionary<string,Word> wordList, int shared=0)
        {
            _user.DeckId = -1;
            _user.DeckName = "Top 10000 Word";
            return _db.CreateDeck(_user, wordList, shared);
        }

    }
}
