namespace ApplicationCore.WordStudy;
public static class WordSimilarityProcess
{
    public static List<Word> CalculateSimilarWords(this IList<Word> wordList, string name, int maxFrequency = 10000)
    {
        List<Word> result = [];

        SortedList<string, Word> matchList = [];

        // find the word first
        Word w1st = wordList.FirstOrDefault(x => x.Name == name) 
                ?? new(name) { MeaningShort = "(not found)" };

        // search the list
        foreach (var w in wordList.Where(x=>x.Frequency<=maxFrequency && string.Compare(x.Name, name, true) != 0))
        {
            double val = WordSimilarity.CalculateSpellingSimilarity(name.ToLower(), w.Name!.ToLower());
            var val2 = WordSimilarity.CalculatePronounciationSimilarity(w1st.Pronunciation, w.Pronunciation);
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

    public static void UpdateAllSimilarWords(this IList<Word> WordList, int maxFrequency = 10000)
    {
        foreach (var w in WordList)
        {
            if(w.Name == null) continue;
            List<Word> list = WordList.CalculateSimilarWords(w.Name, maxFrequency)
                .Where(x=>string.Compare(x.Name, w.Name, true) != 0).ToList();
            w.SimilarWords = list.Count < 1 ? "" : JoinSimilarWords(list);
            Console.WriteLine($"word: {w.Name}, frequency: {w.Frequency}, similar words: {w.SimilarWords}");
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
