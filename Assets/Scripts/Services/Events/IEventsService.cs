namespace HeroicOpportunity.Services.Events
{
    public interface IEventsService
    {
        HeroEvents Hero { get; }
        InputEvents Input { get; }
        EnemyEvents Enemy { get; }
        AbilityEvents Ability { get; }
    }
}
