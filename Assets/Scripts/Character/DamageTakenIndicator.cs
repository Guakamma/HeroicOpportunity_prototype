using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character
{
    public class DamageTakenIndicator : PoolObject
    {
        [SerializeField] [Required]
        private TextMeshProUGUI _damageCounter;

        public void Show(int damage, float endPosY, float duration, Vector2 startPos = default, Color color = default)
        {
            RectTransform rectTransform = (RectTransform) _damageCounter.transform;

            rectTransform.anchoredPosition3D = new Vector3(startPos.x, startPos.y, 0);
            _damageCounter.text = $"-{damage}";
            _damageCounter.color = color != default ? color : Random.ColorHSV();
            
            rectTransform.DOAnchorPos(new Vector2(0, endPosY), duration)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    ReturnToPool();
                });

            _damageCounter.DOFade(0, duration)
                .SetLink(gameObject);
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();
            gameObject.transform.DOKill();
        }
    }
}