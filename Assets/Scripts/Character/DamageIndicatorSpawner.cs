using Helpers.Pool;
using UnityEngine;

namespace Character
{
    public class DamageIndicatorSpawner
    {
        private readonly ObjectsPool<DamageTakenIndicator> _indicatorsPool;
        private readonly RectTransform _root;
        private readonly Vector2 _startPosition;
        private readonly float _endPositionY;
        private readonly float _duration;
        private readonly Color _color;

        public DamageIndicatorSpawner(RectTransform root, float endPositionY, float duration, Vector2 startPosition = default, Color color = default)
        {
            DamageTakenIndicator prefab = Resources.Load<DamageTakenIndicator>("Prefabs/Characters/DamageDealtAmount");
            _indicatorsPool = new ObjectsPool<DamageTakenIndicator>(prefab, 2);
            _root = root;
            _endPositionY = endPositionY;
            _duration = duration;
            _startPosition = startPosition;
            _color = color;
        }

        public void Show(int damage)
        {
            DamageTakenIndicator indicator = _indicatorsPool.GetObject(_startPosition, parent: _root);
            indicator.Show(damage, _endPositionY, _duration, _startPosition, _color);
        }
    }
}