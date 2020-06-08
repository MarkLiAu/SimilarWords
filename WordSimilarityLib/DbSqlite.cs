using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Data;
using System.IO;

namespace WordSimilarityLib
{
    public class DbSqlite
    {
        public string _connString { get; set; }

        private List<UserProfile> userList;
        private List<Word> wordList;
        private List<Deck> deckList;
        private List<MemoryLogFibonacci> logList;

        public delegate void MapData(SqliteDataReader dr);

        public DbSqlite()
        {
            _connString = "";
            userList = new List<UserProfile>();
            wordList = new List<Word>();
            deckList = new List<Deck>();
            logList = new List<MemoryLogFibonacci>();
        }

        public DbSqlite(string conn) : this()
        {
            _connString = conn;
        }

        public int ExecuteNonQuery(string cmdString)
        {
            //SqliteConnectionStringBuilder cb = new SqliteConnectionStringBuilder();
            //cb.DataSource = @"aaa_sqlite.db";
            //cb.Mode = SqliteOpenMode.ReadWriteCreate;
            //_connString = cb.ConnectionString;
            int result;
            
            using (SqliteConnection con = new SqliteConnection(_connString))
            {
                //if (!File.Exists(con.DataSource)) File.Create(con.DataSource);
                con.Open();

                SqliteCommand cmd = new SqliteCommand(cmdString, con);
                result = cmd.ExecuteNonQuery();

                con.Close();
            }
            return result;
        }


        public int GetData(string cmdString, MapData map)
        {
            List<List<Object>> result = new List<List<Object>>();

            int n = 0;
            using (SqliteConnection con = new SqliteConnection(_connString))
            {
                con.Open();

                using (SqliteCommand cmd = new SqliteCommand(cmdString, con))
                {
                    using (SqliteDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            map(dr);
                            n++;
                        }
                    }
                }

                con.Close();
            }
            return n;
        }

        public void MapUser(SqliteDataReader dr)
        {
            UserProfile user = new UserProfile();
            int idx = 0;
            user.Id = Convert.ToInt32(dr[idx++]);
            user.Email = Convert.ToString(dr[idx++]);
            user.Username = Convert.ToString(dr[idx++]);
            user.Password = Convert.ToString(dr[idx++]);
            user.FirstName = Convert.ToString(dr[idx++]);
            user.LastName = Convert.ToString(dr[idx++]);
            user.MaxNewWord = Convert.ToInt32(dr[idx++]);
            user.DeckId = Convert.ToInt32(dr[idx++]);
            userList.Add(user);
        }

        public UserProfile GetUserProfile(string userKey)
        {
            string cmdString = $"SELECT * FROM users WHERE email='{userKey}'";
            userList.Clear();
            int n = GetData(cmdString, MapUser);
            if (n<=0) return null;
            return userList[0];
        }


        public void MapDeck(SqliteDataReader dr)
        {
            Deck data = new Deck();
            int idx = 0;
            data.Id = Convert.ToInt32(dr[idx++]);
            data.Name = Convert.ToString(dr[idx++]);
            data.Owner = Convert.ToString(dr[idx++]);
            data.User = Convert.ToString(dr[idx++]);
            data.MaxNewWord = Convert.ToInt32(dr[idx++]);
            deckList.Add(data);
        }

        public List<Deck> GetUserDecks(int userid)
        {
            string cmdString = $"SELECT rowid, * FROM decks WHERE userid='${userid}'";
            deckList.Clear();
            int n = GetData(cmdString, MapDeck);
  //          if (n <= 0) return null;
            return deckList;
        }

        //cmdString = "CREATE TABLE IF NOT EXISTS words ( deckid INT, TEXT, name TEXT PRIMARY KEY,  frequency INTERGER ,   pronounciation TEXT 
        // , similarWords TEXT,  meaning TEXT, startTime TEXT, studyTime TEXT, interval INT, easiness INT ) ";

        public void MapWord(SqliteDataReader dr)
        {
            Word data = new Word();
            int idx = 0;
            idx++;  // userid
            idx++;  // deckid
            data.name = Convert.ToString(dr[idx++]);
            data.frequency = Convert.ToInt32(dr[idx++]);
            data.pronounciation = Convert.ToString(dr[idx++]);
            data.similarWords = Convert.ToString(dr[idx++]);
            data.meaningShort = Convert.ToString(dr[idx++]);
            data.startTime = Convert.ToDateTime(dr[idx++]);
            data.viewTime = Convert.ToDateTime(dr[idx++]);
            data.viewInterval = Convert.ToInt32(dr[idx++]);
            data.easiness = Convert.ToInt32(dr[idx++]);
            wordList.Add(data);
        }

        public List<Word> Getwords(int userid,int deckid)
        {
            string cmdString = $"SELECT rowid, * FROM decks WHERE userid='${userid}' AND deckid=${deckid}";
            wordList.Clear();
            int n = GetData(cmdString, MapWord);
//            if (n <= 0) return wordList;
            return wordList;
        }



        public bool CreateUser(UserProfile user)
        {
            string cmdString = "INSERT INTO users ( id ,  email , username , password , firstname, lastname, max_new_word ,deckid, create_time, last_login_time ) "
                            + $" VALUES ( 0, '{user.Email}', '{user.Username}', '{user.Password}', '{user.FirstName}', '{user.LastName}', '{user.MaxNewWord}', '{user.DeckId}', '{DateTime.UtcNow.ToString()}', '' ) ";
            ExecuteNonQuery(cmdString);
            return true;
        }

        public bool CreateDb()
        {
            // create User tabl
            string cmdString = "CREATE TABLE IF NOT EXISTS users ( Id INTERGER PRIMARY KEY,  email TEXT, username TEXT, password TEXT, firstname TEXT, lastname TEXT, deckid INT, create_time TEXT, last_login_time TEXT ) ";
            ExecuteNonQuery(cmdString);
            cmdString = "CREATE TABLE IF NOT EXISTS decks ( name TEXT PRIMARY KEY,  ownerid INTERGER ,   userid INT, max_new_word INT ) ";
            ExecuteNonQuery(cmdString);
            cmdString = "CREATE TABLE IF NOT EXISTS words (userid INT, deckid INT, TEXT, name TEXT PRIMARY KEY,  frequency INTERGER ,   pronounciation TEXT , similar_words TEXT,  meaning TEXT, start_time TEXT, study_time TEXT, interval INT, easiness INT ) ";
            ExecuteNonQuery(cmdString);
            cmdString = "CREATE TABLE IF NOT EXISTS logs ( deck TEXT, name TEXT PRIMARY KEY,  study_time TEXT, interval INT, easiness INT ) WITHOUT ROWID;";
            ExecuteNonQuery(cmdString);
            return true;
        }

    }
}
