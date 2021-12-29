using HeroicOpportunity.Services;
using HeroicOpportunity.Services.Events;
using UniRx;
using UnityEngine;


namespace HeroicOpportunity.Cameras
{
    public class Follow : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Vector3 _offset;
        [SerializeField] [Min(0.0f)]
        private float _smoothTime;

        private Transform _target;
        private Vector3 _currentVelocity;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            IEventsService eventsService = ServicesHub.Events;
            eventsService.Hero.HeroCreated
                .Subscribe(hero => _target = hero.transform)
                .AddTo(this);
        }


        private void Start()
        {
            Observable.EveryLateUpdate()
                .Where(_ => _target != null)
                .Subscribe(_ =>
                {
                    Vector3 targetPosition =  _target.position + _offset;
                    targetPosition.x = ServicesHub.Level.ActiveLevel.GetMiddlePositionX();
                    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime);
                })
                .AddTo(this);
        }

        #endregion
    }
}
