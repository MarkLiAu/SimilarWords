namespace ApplicationCore.WordStudy;
public static class WordStudyProcess
{
    // calculate next study date
    public static DateTime CalculateNextStudyDate(DateTime lastStudyTimeUtc, int daysToStudy)
    {
        return lastStudyTimeUtc.AddDays(daysToStudy);
    }

    public static string GetStudyDaysLog(DateTime lastStudyTimeUtc, DateTime nextStudyTimeUtc)
    {
        TimeSpan ts = nextStudyTimeUtc - lastStudyTimeUtc;
        int days = ts.Days;
        if (days < 1) return "";
        if (days == 1) return "1";
        if (days == 2) return "1,1";
        if (days == 3) return "1,1,1";
        return "1,1,1,1";
    }

}
