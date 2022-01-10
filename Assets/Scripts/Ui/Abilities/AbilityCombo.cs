using System.Collections.Generic;
using System.Linq;
using Character.Enemy;
using Data.Abilities;
using Game;
using HeroicOpportunity.Data.Abilities;
using HeroicOpportunity.Game;
using HeroicOpportunity.Ui;
using Services;
using Services.Abilities;
using UniRx;
using UnityEngine;

namespace Ui.Abilities
{
    public class AbilityCombo : MonoBehaviour
    {
        private RectTransform _cardsRoot;
        private readonly List<AbilityCard> _abilityCards = new List<AbilityCard>();
        private ComboInfo _comboInfo;

        
        public void Initialize(RectTransform cardsRoot)
        {
            _cardsRoot = cardsRoot;

            DisposeCards();
            CheckEnemy(ServicesHub.Level.ActiveLevel.ActiveEnemy);

            // ServicesHub.Events.Enemy
            //     .EnemyShowed
            //     .Subscribe(CheckEnemy)
            //     .AddTo(this);
            
            GameStateController.OnStateChanged
                .Subscribe(OnStateChanged)
                .AddTo(this);

            ServicesHub.Events.Ability.AbilityUse
                .Where(_ => _comboInfo != null)
                .Subscribe(CheckComboComplete)
                .AddTo(this);
        }

        private void OnStateChanged(GameStateType type)
        {
            CheckEnemy(null);
        }

        private void CheckComboComplete(AbilityInfo abilityInfo)
        {
            AbilityCard firstCard = _abilityCards.FirstOrDefault(a => (!_comboInfo.IsRandomActive || a.Type == abilityInfo.Type) && a.IsFade);

            if (firstCard != null && firstCard.Id == abilityInfo.Id)
            {
                firstCard.SetFade(0.0f);
            }
            else
            {
                foreach (var a in _abilityCards) 
                    a.SetFade(1.0f);
                
                return;
            }

            if (_abilityCards.All(c => !c.IsFade))
            {
                ServicesHub.Events.Ability.ComboDamage(_comboInfo.AbilityComboDamage);
                foreach (var a in _abilityCards)
                {
                    a.SetFade(1.0f);
                }
            }
        }

        private void CheckEnemy(BaseEnemyController enemyController)
        {
            if (ServicesHub.Level.ActiveLevel.IsBossLevel)
            {
                if (_comboInfo != null)
                    DisposeCards();
                
                Create();
            }
            else
            {
                DisposeCards();
            }
        }


        private void Create()
        {
            _comboInfo = ServicesHub.Combo.GetRandomComboInfo();
            AbilityCard abilityCardPrefab = Resources.Load<AbilityCard>(HeroicOpportunity.Paths.Ui.AbilitiesCard);
            
            foreach (AbilityType abilityType in _comboInfo.AbilityComboSchedule)
            {
                AbilityInfo abilityInfo = ServicesHub.Abilities.GetAllAbilityInfos().First(i => i.Type == abilityType);
                AbilityCard abilityCard = Instantiate(abilityCardPrefab, _cardsRoot);
                abilityCard.Initialize(abilityInfo);
                abilityCard.SetFade(1.0f);
                _abilityCards.Add(abilityCard);
            }

            _cardsRoot.gameObject.SetActive(true);
        }


        private void DisposeCards()
        {
            _comboInfo = null;
            foreach (var abilityCard in _abilityCards)
            {
                Destroy(abilityCard.gameObject);
            }

            _abilityCards.Clear();

            if(_cardsRoot)
                _cardsRoot.gameObject.SetActive(false);
        }
    }
}
