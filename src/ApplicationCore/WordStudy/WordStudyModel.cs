namespace ApplicationCore.WordStudy;
public class WordStudyModel
{
    public int Id { get; set; }
    public string? UserName { set; get; }
    public string? WordName { get; set; }  
    public DateTime StartTimeUtc { get; set; } = DateTime.MinValue;
    public DateTime LastStudyTimeUtc { get; set; } = DateTime.MinValue;
    public int StudyCount { get; set; }
    public bool IsClosed { get; set; } = true;
    public int DaysToStudy { get; set; }    // LastStudyTimeUtc + DaysToStudy = NextStudyTimeUtc to remind user to study

    virtual public Word? Word { get; set; }

    public WordStudyModel(string? userName, string? wordName)
    {
        UserName = userName;
        WordName = wordName;
    }

    public WordStudyModel(string? userName, Word word)
    {
        UserName = userName;
        WordName = word.Name;
        Word = word;
    }
}
