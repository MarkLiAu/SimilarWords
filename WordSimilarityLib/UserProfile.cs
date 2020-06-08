using System;
using System.Collections.Generic;
using System.Text;

namespace WordSimilarityLib
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        // public string Token { get; set; }

        public string Email { get; set; }
        public int DeckId { get; set; }         // current deck ID

        public string DeckName { get; set; }
        public int MaxNewWord { get; set; }         // maximum new word to study every day

        public UserProfile()
        {
            MaxNewWord = 10;
        }
    }


    public class Deck
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string User { get; set; }
        public int MaxNewWord { get; set; }

        public Deck()
        {

        }
    }

}
