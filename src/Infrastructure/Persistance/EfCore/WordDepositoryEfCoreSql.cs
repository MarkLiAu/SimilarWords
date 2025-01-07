using ApplicationCore.WordDictionary;
using ApplicationCore.WordStudyNameSpace;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public class WordDepositoryEfCoreSql(AppDbContext appDbContext) : IWordDepository
{
    public async Task<IList<WordStudy>> GetAllWordStudyAsync(string userName)
    {
        return await appDbContext.WordStudies.Where(ws => ws.UserName == userName).ToListAsync();
    }

    public async Task<WordStudy?> GetWordStudyAsync(string userName, string wordName)
    {
        return await appDbContext.WordStudies.FirstOrDefaultAsync(ws => ws.UserName == userName && ws.WordName == wordName);
    }

    public async Task<int> UpsertWordStudyAsync(WordStudy wordStudy)
    {
        var existingWordStudy = await appDbContext.WordStudies
            .FirstOrDefaultAsync(ws => ws.UserName == wordStudy.UserName && ws.WordName == wordStudy.WordName);

        if (existingWordStudy == null)
        {
            await appDbContext.WordStudies.AddAsync(wordStudy);
        }
        else
        {
            appDbContext.WordStudies.Entry(existingWordStudy).CurrentValues.SetValues(wordStudy);
        }

        return await appDbContext.SaveChangesAsync();
    }

    async Task<IList<Word>> IWordDepository.GetSimilarWordsAsync(string name)
    {
        var result = await appDbContext.Words.Where(w => w.Name == name).AsNoTracking().Select(w=>w).ToListAsync();
        if(result.Count == 0) return new List<Word> { new Word(name) { MeaningShort = "(not found)" } };
        var similarWords = WordSimilarityProcess.SplitSimilarWords(result[0].SimilarWords);
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
