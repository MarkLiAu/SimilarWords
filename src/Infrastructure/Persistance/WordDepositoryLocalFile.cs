using ApplicationCore.WordDictionary;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistance;

public class WordDepositoryLocalFile(IConfiguration configuration) : IWordDepository
{
    private const string WordFileName = "WordSimilarityList.txt";
    private static List<Word> _wordList = [];
    async Task<IList<Word>> IWordDepository.GetSimilarWordsAsync(string name)
    {
        await LoadWordListAsync();
        var result = _wordList.FindSimilarWords(name);
        return result;
    }

    async Task<IList<Word>> IWordDepository.GetWordListAsync()
    {
        await LoadWordListAsync();
        return _wordList;
    }

    private async Task LoadWordListAsync()
    {
        if (_wordList.Count > 0) return;
        _wordList = await LoadWordListFromFile(WordFileName);
    }

    async Task<int> IWordDepository.UpdateWordAsync(Word word)
    {
        throw new NotImplementedException();
    }

    async Task<int> IWordDepository.UpdateWordListAsync(Word[] wordList)
    {
        throw new NotImplementedException();
    }

    private string[] FindFileFullPath(string? searchPath, string name)
    {
        try
        {
        if (string.IsNullOrWhiteSpace(searchPath) || string.IsNullOrWhiteSpace(name)) return [];

        var files = Directory.GetFiles(searchPath, name, SearchOption.AllDirectories);
        return files is null ? [] : files;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return [];
        }
    }

    private async Task<List<Word>> LoadWordListFromFile(string fileName, string delimiter = "\t")
    {
        var file = FindFileFullPath(Environment.GetEnvironmentVariable("HOME"), fileName).FirstOrDefault()
                    ?? FindFileFullPath(Directory.GetCurrentDirectory(), fileName).FirstOrDefault();

        if (file is null) return [];
        List<Word> WordList= [];
        using (StreamReader sr = new StreamReader(file))
        {
            var line = await sr.ReadLineAsync();
            if (line is null) return WordList;
            string[] columns = line.Split(delimiter);
            while (true)
            {
                line = sr.ReadLine();
                if (line == null) break;
                // read content
                string[] ss = line.Split(delimiter);
                if (ss.Length < 2) continue;
                Word w = new Word(ss[0]);
                if (ss[0] == "cool") { string tmp = ss[0]; }
                for (int i = 1; i < columns.Length && i < ss.Length; i++)
                {
                    if (ss[i].Trim() == "") continue;
                    ss[i] = ss[i].Replace("<~>", "\n");
                    if (columns[i].Equals("pronounciation", StringComparison.CurrentCultureIgnoreCase)) w.Pronounciation = ss[i];
                    else if (columns[i].Equals("pronounciationam", StringComparison.CurrentCultureIgnoreCase)) w.PronounciationAm = ss[i];
                    else if (columns[i].Equals("frequency", StringComparison.CurrentCultureIgnoreCase)) w.Frequency = Convert.ToInt32(ss[i]);
                    else if (columns[i].Equals("similarwords", StringComparison.CurrentCultureIgnoreCase)) w.SimilarWords = ss[i];
                    else if (columns[i].Equals("meaningshort", StringComparison.CurrentCultureIgnoreCase)) w.MeaningShort = ss[i];
                    else if (columns[i].Equals("meaninglong", StringComparison.CurrentCultureIgnoreCase)) w.MeaningLong = ss[i];
                    else if (columns[i].Equals("soundurl", StringComparison.CurrentCultureIgnoreCase)) w.SoundUrl = ss[i];
                    else if (columns[i].Equals("examplesoundurl", StringComparison.CurrentCultureIgnoreCase)) w.ExampleSoundUrl = ss[i];
                }
                if(string.IsNullOrWhiteSpace(w.SoundUrl)) w.SoundUrl = $"https://ssl.gstatic.com/dictionary/static/sounds/oxford/{w.Name}--_us_1.mp3";
                WordList.Add(w);
            }
        }
        return WordList;
    }


}
