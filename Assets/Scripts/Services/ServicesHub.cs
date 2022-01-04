using HeroicOpportunity.Services.Abilities;
using HeroicOpportunity.Services.Enemies;
using HeroicOpportunity.Services.Events;
using HeroicOpportunity.Services.Hero;
using HeroicOpportunity.Services.Level;
using HeroicOpportunity.Services.Prefs;
using Services.Abilities;
using Services.Enemies;

namespace Services
{
    public class ServicesHub : Singleton<ServicesHub>
    {
        private readonly IPrefsService _prefsService;
        private readonly IEventsService _eventsService;
        private readonly IHeroService _heroService;
        private readonly IEnemiesService _enemiesService;
        private readonly ILevelService _levelService;
        private readonly IAbilitiesService _abilitiesService;
        private readonly IComboService _comboService;


        private static IPrefsService Prefs => Instance._prefsService;
        public static IEventsService Events => Instance._eventsService;
        public static IHeroService Hero => Instance._heroService;
        public static IEnemiesService Enemies => Instance._enemiesService;
        public static ILevelService Level => Instance._levelService;
        public static IAbilitiesService Abilities => Instance._abilitiesService;
        public static IComboService Combo => Instance._comboService;


        public ServicesHub()
        {
            _prefsService = new PrefsServiceImpl();
            _eventsService = new EventsServicesImpl();
            _heroService = new HeroServicesImpl(_eventsService);
            _enemiesService = new EnemiesServiceImpl(_heroService);
            _levelService = new LevelServiceImpl(_prefsService);
            _abilitiesService = new AbilitiesServicesImpl();
            _comboService = new ComboServiceImpl();
        }
    }
}
