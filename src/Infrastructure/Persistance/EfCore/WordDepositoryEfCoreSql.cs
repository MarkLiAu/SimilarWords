using ApplicationCore.WordDictionary;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public class WordDepositoryEfCoreSql(AppDbContext appDbContext) : IWordDepository
{
    async Task<IList<Word>> IWordDepository.GetSimilarWordsAsync(string name)
    {
        var result = await appDbContext.Words.Where(w => w.Name == name).AsNoTracking().Select(w=>w).ToListAsync();
        if(result.Count == 0) return new List<Word> { new Word(name) { MeaningShort = "(not found)" } };
        var similarWords = WordSimilarityExtensions.SplitSimilarWords(result[0].SimilarWords);
        var similarWordList = await appDbContext.Words.Where(w => similarWords.Contains(w.Name)).AsNoTracking().ToListAsync();
        result.AddRange(similarWordList);
        return result;
    }

    async Task<IList<Word>> IWordDepository.GetWordListAsync()
    {
        var result = await appDbContext.Words.AsNoTracking().ToListAsync();
        return result??[];
    }

    async Task<int> IWordDepository.UpdateWordAsync(Word word)
    {
        throw new NotImplementedException();
    }

    async Task<int> IWordDepository.UpdateWordListAsync(IList<Word> wordList)
    {
        if (wordList is null || wordList.Count == 0) return 0;
        var ll = wordList.Where(x=>x.Name!.Length>100).ToList();
        appDbContext.Words.AddRange(wordList);
        return await appDbContext.SaveChangesAsync();
    }
}
