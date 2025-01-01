namespace ApplicationCore.WordDictionary;
public class Word
{
    public string? Name { set; get; }
    public string? Pronounciation { get; set; }          // British
    public string? PronounciationAm { get; set; }        // American
    public int Frequency { get; set; }
    public string? MeaningShort { get; set; }
    public string? MeaningLong { get; set; }
    public string? SoundUrl { get; set; }
    public string? ExampleSoundUrl { get; set; }
    public string? SimilarWords { get; set; }
    public int Id { get; set; }

    public Word(string name)
    {
        Name = name;
    }
}