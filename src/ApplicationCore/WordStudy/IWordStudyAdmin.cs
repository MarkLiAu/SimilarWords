using ApplicationCore.WordStudy;

namespace ApplicationCore.WordStudy;
public interface IWordStudyAdmin
{
    Task<int> SetupWordDbAsync();
}
