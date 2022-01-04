using HeroicOpportunity.Services;
using HeroicOpportunity.Services.Events;
using Services;
using UniRx;
using UnityEngine;


namespace HeroicOpportunity.Input
{
    public class SwipeHandler : MonoBehaviour
    {
        private const float DeadRadios = 10f;

        private bool _isButtonDown;
        private bool _isSendEvent;


        private void Start()
        {
            IEventsService eventsService = ServicesHub.Events;

            Vector3 startPosition = Vector3.zero;

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (UnityEngine.Input.GetMouseButtonDown(0))
                    {
                        _isButtonDown = true;
                        startPosition = UnityEngine.Input.mousePosition;
                    }

                    if (UnityEngine.Input.GetMouseButtonUp(0))
                    {
                        _isButtonDown = false;
                        _isSendEvent = false;
                    }
                })
                .AddTo(this);

            Observable.EveryUpdate()
                .Where(_ => !_isSendEvent && _isButtonDown)
                .Select(_ => UnityEngine.Input.mousePosition)
                .Where(p => Vector3.Distance(p, startPosition) > DeadRadios)
                .Subscribe(p =>
                {
                    Direction direction = startPosition.x - p.x > 0 ? Direction.Left : Direction.Right;
                    eventsService.Input.ChangeDirection(direction);
                    _isSendEvent = true;
                })
                .AddTo(this);


        }
    }
}
