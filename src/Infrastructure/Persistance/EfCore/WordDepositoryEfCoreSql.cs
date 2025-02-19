using ApplicationCore.WordStudy;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public class WordDepositoryEfCoreSql(AppDbContext appDbContext) : IWordDepository
{
    public async Task<IList<WordStudyModel>> GetUserWordStudyListAsync(string userName)
    {
        return await appDbContext.WordStudies.Where(ws => ws.UserName == userName && !ws.IsClosed).ToListAsync();
    }

    public async Task<WordStudyModel?> GetWordStudyAsync(string userName, string wordName)
    {
        return await appDbContext.WordStudies.FirstOrDefaultAsync(ws => ws.UserName == userName && ws.WordName == wordName && !ws.IsClosed);
    }

    public async Task<IList<WordStudyModel>> GetMultipleWordStudyAsync(string userName, IEnumerable<string> wordList)
    {
        return await appDbContext.WordStudies.Where(ws => ws.UserName == userName && !ws.IsClosed && wordList.Contains(ws.WordName)).ToListAsync();
    }

    public async Task<IList<Word>> GetMultipleWordSAsync(IEnumerable<string> wordList)
    {
        return await appDbContext.Words.Where(w => wordList.Contains(w.Name)).ToListAsync();
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
        var oldList = appDbContext.Words.ToList();

        // split into chuncks of 1000
        var chunkSize = 10;
        var batches = wordList.Chunk(chunkSize);
        var count=0;
        foreach (var batch in batches)
        {
            try
            {
                await appDbContext.Words.AddRangeAsync(batch);
                count+= await appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        return count;
    }
}
