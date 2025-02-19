using System.Collections.Generic;

namespace Infrastructure.WordExplanation;

// Models/GeminiRequest.cs
public class GeminiRequest
{
    public GeminiContent[] Contents { get; set; }
}

public class GeminiContent
{
    public string Role { get; set; }
    public GeminiPart[] Parts { get; set; }
}

public class GeminiPart
{
    public string Text { get; set; }
}

// Models/GeminiResponse.cs
public class GeminiResponse
{
    public GeminiCandidate[] Candidates { get; set; }
}

public class GeminiCandidate
{
    public GeminiContent Content { get; set; }
}