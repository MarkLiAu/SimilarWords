namespace ApplicationCore.WordDictionary;

public interface IWordQuery
{
    Task<IList<Word>> SearchSimilarWords(string searchText);
}