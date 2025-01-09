using ApplicationCore.WordStudy;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public class WordDepositoryEfCoreSql(AppDbContext appDbContext) : IWordDepository
{
    public async Task<IList<WordStudyModel>> GetAllWordStudyAsync(string userName)
    {
        return await appDbContext.WordStudies.Where(ws => ws.UserName == userName).ToListAsync();
    }

    public async Task<WordStudyModel?> GetWordStudyAsync(string userName, string wordName)
    {
        return await appDbContext.WordStudies.FirstOrDefaultAsync(ws => ws.UserName == userName && ws.WordName == wordName && !ws.IsClosed);
    }

    public async Task<IList<WordStudyModel>> GetMultipleWordStudyAsync(string userName, IEnumerable<string> wordList)
    {
        return await appDbContext.WordStudies.Where(ws => ws.UserName == userName && !ws.IsClosed && wordList.Contains(ws.WordName)).ToListAsync();
    }

    public async Task<int> UpsertWordStudyAsync(WordStudyModel wordStudy)
    {
        var existingWordStudy = await GetWordStudyAsync(wordStudy.UserName!, wordStudy.WordName!);

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
