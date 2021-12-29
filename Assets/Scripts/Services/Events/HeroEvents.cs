using System;
using HeroicOpportunity.Character.Hero;
using UniRx;


namespace HeroicOpportunity.Services.Events
{
    public class HeroEvents
    {
        private readonly Subject<HeroController> _heroCreated = new Subject<HeroController>();


        public IObservable<HeroController> HeroCreated => _heroCreated.AsObservable();


        public void CreateHero(HeroController hero) => _heroCreated.OnNext(hero);
    }
}
