using ApplicationCore.WordStudy;

namespace Infrastructure.Persistance;

public class WordDepositoryLocalFile() : IWordDepository
{
    private readonly string WordFileName = Environment.GetEnvironmentVariable("DbConnection") ?? "WordSimilarityList.txt";
    private static List<Word> _wordList = [];

    public async Task<IList<Word>> GetSimilarWordsAsync(string name)
    {
        await LoadWordListAsync();
        var result = _wordList.FindSimilarWords(name);
        return result;
    }

    public async Task<IList<Word>> GetWordListAsync()
    {
        await LoadWordListAsync();
        return _wordList;
    }

    private async Task LoadWordListAsync()
    {
        if (_wordList.Count > 0) return;
        _wordList = await LoadWordListFromFile(WordFileName);
    }

    public async Task<int> UpdateWordAsync(Word word)
    {
        throw new NotImplementedException();
    }

    public async Task<int> UpdateWordListAsync(IList<Word> wordList)
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
                    ?? FindFileFullPath(Directory.GetCurrentDirectory(), fileName).FirstOrDefault()
                    ?? FindFileFullPath(Directory.GetCurrentDirectory(), "WordSimilarityList.txt").FirstOrDefault();

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

    public Task<WordStudyModel> GetWordStudyAsync(string userName, string wordName)
    {
        throw new NotImplementedException();
    }

    public Task<IList<WordStudyModel>> GetUserWordStudyListAsync(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpsertWordStudyAsync(WordStudyModel wordStudy)
    {
        throw new NotImplementedException();
    }

    public Task<IList<WordStudyModel>> GetMultipleWordStudyAsync(string userName, IEnumerable<string> wordList)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Word>> GetMultipleWordSAsync(IEnumerable<string> wordList)
    {
        throw new NotImplementedException();
    }
}
