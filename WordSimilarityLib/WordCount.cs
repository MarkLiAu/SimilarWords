using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// count words from a file or string
/// </summary>
namespace WordSimilarityLib
{
    public class WordCount
    {
        public static Dictionary<string,int> CountWords(string text, Dictionary<string, int> result)
        {
            if (result == null) result = new Dictionary<string, int>();
            for(int i=0; i<text.Length;)
            {
                int nexti = FindNextWord(text, i);
                string word = text.Substring(i, nexti - i);
                i = nexti;
                word = PureWord(word).ToLower();
                if (word == "") continue;
                if (result.ContainsKey(word)) result[word]++;
                else result[word] = 1;
            }

            return result;
        }

        /// <summary>
        /// return next position, we're counted as one word
        /// </summary>
        /// <param name="test"></param>
        /// <param name="start"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int FindNextWord(string text, int start)
        {
            bool found = false;
            for(int i=start;i<text.Length;i++)
            {
                if (char.IsLetter(text[i])) found = true;
                else if (text[i] != '\'' && found) return i;
            }
            return text.Length;
        }

        /// <summary>
        /// pick actual a-z A-Z and ' from a string, 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string PureWord(string text)
        {
            string word = "";
            foreach (var c in text)
                if (char.IsLetter(c) || c == '\'') word += c;
            return word;
        }
    }
}
