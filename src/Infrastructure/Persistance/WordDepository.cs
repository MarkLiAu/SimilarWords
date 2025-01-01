using ApplicationCore.WordDictionary;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistance;

public class WordDepository(IConfiguration configuration) : IWordDepository
{
    async Task<IList<Word>> IWordDepository.GetSimilarWordsAsync(string name)
    {
        string rootPath = Environment.GetEnvironmentVariable("HOME") ?? Directory.GetCurrentDirectory();
        string filePath = Path.Combine(rootPath, "site", "wwwroot", "myfile.txt"); // Adjust as needed
        if (File.Exists(filePath))
        {
            string content = await File.ReadAllTextAsync(filePath);
            return [new Word(name) { MeaningLong = content }];
        }
        else
        {
            return [];
        }
    }

    async Task<IList<Word>> IWordDepository.GetWordListAsync()
    {
        string rootPath = Environment.GetEnvironmentVariable("HOME") ?? Directory.GetCurrentDirectory();
        string filePath = Path.Combine(rootPath, "site", "wwwroot", "myfile.txt"); // Adjust as needed
        var files = new DirectoryInfo(rootPath).GetFileSystemInfos();
        return files.Select(x => new Word(x.FullName)).ToList();
    }

    async Task<int> IWordDepository.UpdateWordAsync(Word word)
    {
        throw new NotImplementedException();
    }

    async Task<int> IWordDepository.UpdateWordListAsync(Word[] wordList)
    {
        throw new NotImplementedException();
    }
}
