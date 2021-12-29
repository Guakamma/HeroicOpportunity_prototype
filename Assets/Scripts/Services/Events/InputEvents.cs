using System;
using UniRx;


namespace HeroicOpportunity.Services.Events
{
    public class InputEvents
    {
        private readonly Subject<Input.Direction> _directionChanged = new Subject<Input.Direction>();


        public IObservable<Input.Direction> DirectionChanged => _directionChanged.AsObservable();


        public void ChangeDirection(Input.Direction direction) => _directionChanged.OnNext(direction);
    }
}
