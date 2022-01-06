using System;
using Character.Hero;
using Input;
using UniRx;

namespace Services.Events
{
    public class HeroEvents
    {
        private readonly Subject<HeroController> _heroCreated = new Subject<HeroController>();
        private readonly Subject<(Direction, float)> _directionChanged = new Subject<(Direction, float)>();


        public IObservable<HeroController> HeroCreated => _heroCreated.AsObservable();
        public IObservable<(Direction, float)> DirectionChanged => _directionChanged.AsObservable();


        public void CreateHero(HeroController hero) => _heroCreated.OnNext(hero);
        public void ChangeDirection(Direction direction, float delta) => _directionChanged.OnNext((direction, delta));
    }
}
