namespace ApplicationCore.WordDictionary;
public class WordStudy
{
    public string? UserName { set; get; }
    public string? WordName { get; set; }  
    public DateTime StartTimeUtc { get; set; }
    public DateTime LastStudyTimeUtc { get; set; }
    public int DaysToStudy { get; set; }    // LastStudyTimeUtc + DaysToStudy = NextStudyTimeUtc to remind user to study
    public string? StudyDaysLog { get; set; }    // "1,1,2,3,5,8,13" to record days between each study

    public WordStudy(string userName)
    {
        UserName = userName;
    }
}
