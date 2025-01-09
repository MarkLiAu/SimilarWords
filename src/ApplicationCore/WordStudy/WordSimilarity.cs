namespace ApplicationCore.WordStudy;
public class WordSimilarity
{
    private const double SimilarityThreshold = 0.7;
    private const double PronounciationSimilarityValue = 0.9;
    private const double SameCharactersSimilarityValue = 0.85;
    public static double CalculateSpellingSimilarity(string? compword1, string? compword2)
    {
        if (string.IsNullOrWhiteSpace(compword1)) return 0;
        if (string.IsNullOrWhiteSpace(compword2)) return 0;

        var word1 = compword1.Trim().ToLower();
        var word2 = compword2.Trim().ToLower();
        // same word
        if (word1 == word2) return 1.0;
        // contains
        if (word1.Contains(word2)) return word2.Length * 1.0 / word1.Length;
        if (word2.Contains(word1)) return word1.Length * 1.0 / word2.Length;

        // same characters
        string s1 = String.Concat(word1.OrderBy(c => c));
        string s2 = String.Concat(word2.OrderBy(c => c));
        if (s1 == s2) return SameCharactersSimilarityValue;

        // check similarity from start and end
        int sameFront = 0;
        for (int i = 0; i < word1.Length && i < word2.Length; i++)
            if (word1[i] == word2[i]) sameFront++;
            else break;

        int sameBack = 0;
        for (int i = word1.Length - 1, j = word2.Length - 1; i >= sameFront && j >= sameFront; i--, j--)
            if (word1[i] == word2[j]) sameBack++;
            else break;

        return (sameFront + sameBack) * 1.0 / Math.Max(word1.Length, word2.Length);
    }

    public static double CalculatePronounciationSimilarity(string? pron1, string? pron2)
    {
        return !string.IsNullOrWhiteSpace(pron1) && !string.IsNullOrWhiteSpace(pron2) 
            && pron1.Length > 0 && pron1 == pron2 ? PronounciationSimilarityValue : 0; 
    }

    public static double CalculateWordSimilarity(Word word1, Word word2)
    {
        var result = CalculateSpellingSimilarity(word1.Name, word2.Name);
        var pronounciationSimilarity = CalculatePronounciationSimilarity(word1.Pronounciation, word2.Pronounciation);
        if( pronounciationSimilarity > result) result = pronounciationSimilarity;
        return result;
    }

    public static List<Word> FindSimilarWords(string name, IList<Word> wordDictionary)
    {
        List<Word> result = [];

        if (name == "randomword")
        {
            Random rnd = new Random();
            int n = rnd.Next(0, wordDictionary.Count - 1);
            name = wordDictionary[n].Name!;
        }

        SortedList<string, Word> matchList = [];

        // find the word first
        Word w1st = new(name)
        {
            MeaningShort = "(not found)"
        };

        string nameLowcase = name.ToLower();
        foreach (var w in wordDictionary)
        {
            if (w.Name == nameLowcase) { w1st = w; break; }
        }
            // search the list
        foreach (var w in wordDictionary)
        {
            if (w.Name == nameLowcase) continue; 
            double val = CalculateSpellingSimilarity(nameLowcase, w.Name);
            var val2 = CalculatePronounciationSimilarity(w1st.Pronounciation, w.Pronounciation);
            if (val2 > val) val = val2;
            if (val < 0.7) continue;
            matchList.Add((1 - val).ToString("0.000000") + w.Frequency.ToString("00000"), w);     // sort by compare Val and frequency
        }

        result.Add(w1st);

        foreach (var m in matchList) result.Add(m.Value);
        return result;
    }

}
