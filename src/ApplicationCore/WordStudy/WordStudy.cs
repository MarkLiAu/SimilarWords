using ApplicationCore.WordDictionary;

namespace ApplicationCore.WordStudyNameSpace;
public class WordStudy
{
    public int Id { get; set; }
    public string? UserName { set; get; }
    public string? WordName { get; set; }  
    public DateTime StartTimeUtc { get; set; }
    public DateTime LastStudyTimeUtc { get; set; }
    public bool IsClosed { get; set; }
    public int DaysToStudy { get; set; }    // LastStudyTimeUtc + DaysToStudy = NextStudyTimeUtc to remind user to study

    virtual public ICollection<WordStudyLog> WordStudyLogs { get; set; } = [];

    public WordStudy(string userName, string wordName)
    {
        UserName = userName;
        WordName = wordName;
        StartTimeUtc = DateTime.MinValue;
    }
}
