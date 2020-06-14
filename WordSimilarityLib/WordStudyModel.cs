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
            return (_db.GetUserProfile(userKey));
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
            return _db.CreateDeck(_user, wordList, shared);
        }

    }
}
