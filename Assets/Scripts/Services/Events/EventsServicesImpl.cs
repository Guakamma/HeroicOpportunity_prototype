namespace HeroicOpportunity.Services.Events
{
    public class EventsServicesImpl : IEventsService
    {
        public HeroEvents Hero { get; } = new HeroEvents();
        public InputEvents Input { get; } = new InputEvents();
        public EnemyEvents Enemy { get; } = new EnemyEvents();
        public AbilityEvents Ability { get; } = new AbilityEvents();
    }
}
