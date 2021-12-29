using HeroicOpportunity.Level;


namespace HeroicOpportunity.Services.Level
{
    public interface ILevelService
    {
        int LevelNumber { get; }
        LevelController ActiveLevel { get; }
        void CrateLevel();
        void DisposeActiveLevel();
        void IncrementLevelNumber();
    }
}
