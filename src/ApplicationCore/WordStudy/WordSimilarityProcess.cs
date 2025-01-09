namespace ApplicationCore.WordStudy;
public static class WordSimilarityProcess
{
    public static List<Word> FindSimilarWords(this IList<Word> wordList, string name)
    {
        List<Word> result = [];

        SortedList<string, Word> matchList = [];

        // find the word first
        Word w1st = new(name)
        {
            MeaningShort = "(not found)"
        };

        string nameLowcase = name.ToLower();
        foreach (var w in wordList)
        {
            if (w.Name == nameLowcase) { w1st = w; break; }
        }
            // search the list
        foreach (var w in wordList)
        {
            if (w.Name == nameLowcase) continue; 
            double val = WordSimilarity.CalculateSpellingSimilarity(nameLowcase, w.Name);
            var val2 = WordSimilarity.CalculatePronounciationSimilarity(w1st.Pronounciation, w.Pronounciation);
            if (val2 > val) val = val2;
            if (val < 0.7) continue;
            matchList.Add((1 - val).ToString("0.000000") + w.Frequency.ToString("00000"), w);     // sort by compare Val and frequency
        }

        result.Add(w1st);

        foreach (var m in matchList) result.Add(m.Value);
        return result;
    }

    public static string GetRandomWord(this IList<Word> wordList)
    {
            Random rnd = new Random();
            int n = rnd.Next(0, wordList.Count - 1);
            return wordList[n].Name!;
    }

        public static void UpdateAllSimilarWords(this IList<Word> WordList)
        {
            foreach (var w in WordList)
            {
                if(w.Name == null) continue;
                List<Word> list = WordList.FindSimilarWords(w.Name);
                if (list.Count <= 1) continue;
                w.SimilarWords = JoinSimilarWords(list);
            }
        }

        public static string JoinSimilarWords(IList<Word> wordList)
        {
            return string.Join(" ", wordList.Select(w => w.Name));
        }

        public static string[] SplitSimilarWords(string? similarWords)
        {
            return similarWords?.Split(' ') ?? [];
        }
}
