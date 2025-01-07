namespace ApplicationCore.WordStudyNameSpace;
public class WordStudyLog
{
    public int Id { get; set; }
    public int WordStudyId { get; set; }
    public DateTime StudyTimeUtc { get; set; }
    public DateTime ScheduledStudyTimeUtc { get; set; }

}
