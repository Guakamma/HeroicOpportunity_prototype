using System.Collections.Generic;
using Character.Hero;
using HeroicOpportunity.Data;
using HeroicOpportunity.Data.Heroes;
using HeroicOpportunity.Services.Events;
using UnityEngine;


namespace HeroicOpportunity.Services.Hero
{
    public class HeroServicesImpl : IHeroService
    {
        #region Fields

        private readonly IEventsService _eventsService;
        private readonly Dictionary<string, HeroInfo> _heroInfos;

        #endregion



        #region Properties

        private HeroesData HeroesData => DataHub.Heroes;

        #endregion



        #region Class lifecycle

        public HeroServicesImpl(IEventsService eventsService)
        {
            _eventsService = eventsService;
            _heroInfos = new Dictionary<string, HeroInfo>();
            foreach (var p in HeroesData.HeroInfoPaths)
            {
                _heroInfos.Add(p.Key, Resources.Load<HeroInfo>(p.Value));
            }
        }

        #endregion



        #region Private methods

        HeroInfo GetSelectedHeroInfo()
        {
            return _heroInfos[HeroesData.DefaultNameHero];
        }

        #endregion



        #region IHeroService

        public HeroController ActiveHero { get; private set; }


        public HeroController CreateHero(Transform root, Vector3 position)
        {
            DisposeActiveHero();

            HeroInfo info = GetSelectedHeroInfo();
            HeroController hero = new GameObject(nameof(HeroController) + "_" + info.Id)
                .AddComponent<HeroController>();
            hero.transform.SetParent(root);
            hero.transform.position = position;
            hero.Initialize(info);

            ActiveHero = hero;
            _eventsService.Hero.CreateHero(hero);
            return ActiveHero;
        }


        public void DisposeActiveHero()
        {
            if (ActiveHero == null)
            {
                return;
            }

            ActiveHero.Dispose();
            ActiveHero = null;
        }

        #endregion
    }
}
