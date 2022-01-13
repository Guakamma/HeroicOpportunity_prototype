using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Character
{
    public class CharacterFxHandler : MonoBehaviour
    {
        public enum FxType
        {
            None = 0,
            ProtectedShield = 1,
        }

        [Serializable]
        public class CharacterFx
        {
            public GameObject body;
            public FxType type;
        }

        [SerializeField]
        private CharacterFx[] _characterFxs;

        private List<(FxType type, Coroutine coroutine)> _fxCoroutines;

        public void Initialize()
        {
            HideAll();
            _fxCoroutines = new List<(FxType type, Coroutine coroutine)>();
        }

        public void ShowFx(FxType type, float duration, Action callBack)
        {
            CharacterFx fx = _characterFxs.FirstOrDefault(i => i.type == type);

            if (fx == null)
            {
                Debug.LogException(new Exception($"Fx {type} on character {name} not found"));
                return;
            }

            if (!_fxCoroutines.IsNullOrEmpty())
            {
                var fxCoroutine = _fxCoroutines.FirstOrDefault(i => i.type == type);
                if (fxCoroutine != default)
                {
                    StopCoroutine(fxCoroutine.coroutine);
                    _fxCoroutines.Remove(fxCoroutine);
                }
            }

            Coroutine newFxCoroutine = StartCoroutine(ShowFxRoutine(fx, duration, callBack));
            _fxCoroutines.Add((type, newFxCoroutine));
        }

        private IEnumerator ShowFxRoutine(CharacterFx fx, float duration, Action callBack)
        {
            fx.body.SetActive(true);
            yield return new WaitForSeconds(duration);
            fx.body.SetActive(false);
            callBack?.Invoke();
        }

        public void HideAll()
        {
            StopAllCoroutines();
            if (!_fxCoroutines.IsNullOrEmpty())
                _fxCoroutines.Clear();

            foreach (CharacterFx fx in _characterFxs)
            {
                fx.body.SetActive(false);
            }
        }
    }
}