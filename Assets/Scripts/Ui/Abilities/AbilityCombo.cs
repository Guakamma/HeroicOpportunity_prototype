using System;
using System.Collections.Generic;
using System.Linq;
using HeroicOpportunity.Character.Enemy;
using HeroicOpportunity.Data.Abilities;
using HeroicOpportunity.Data.Enemies;
using HeroicOpportunity.Services;
using UniRx;
using UnityEngine;


namespace HeroicOpportunity.Ui
{
    public class AbilityCombo : MonoBehaviour
    {
        #region Fields

        private RectTransform _cardsRoot;
        private readonly List<AbilityCard> _abilityCards = new List<AbilityCard>();
        private EnemyInfo _enemyInfo;

        #endregion



        #region Public methods

        public void Initialize(RectTransform cardsRoot)
        {
            _cardsRoot = cardsRoot;

            DisposeCards();
            CheckEnemy(ServicesHub.Level.ActiveLevel.ActiveEnemy);

            ServicesHub.Events.Enemy
                .EnemyShowed
                .Subscribe(CheckEnemy)
                .AddTo(this);

            ServicesHub.Events.Ability.AbilityDamage
                .Where(_ => _enemyInfo != null)
                .Subscribe(i =>
                {
                    var firstCard = _abilityCards.FirstOrDefault(a => a.IsFade);
                    if (firstCard == null)
                    {
                        return;
                    }

                    if (firstCard.Id == i.Id)
                    {
                        firstCard.SetFade(0.0f);
                    }

                    if (_abilityCards.All(c => !c.IsFade))
                    {
                        ServicesHub.Events.Ability.ComboDamage(_enemyInfo.AbilityComboDamage);
                        foreach (var a in _abilityCards)
                        {
                            a.SetFade(1.0f);
                        }
                    }
                })
                .AddTo(this);
        }

        #endregion



        #region Private methods

        private void CheckEnemy(BaseEnemyController enemyController)
        {
            if (enemyController.EnemyInfo.IsBoss)
            {
                Create(enemyController.EnemyInfo);
            }
            else
            {
                DisposeCards();
            }
        }


        private void Create(EnemyInfo enemyInfo)
        {
            _enemyInfo = enemyInfo;
            AbilityCard abilityCardPrefab = Resources.Load<AbilityCard>(Paths.Ui.AbilitiesCard);
            foreach (string abilityId in enemyInfo.AbilityCombo)
            {
                AbilityInfo abilityInfo = ServicesHub.Abilities.GetAllAbilityInfos().First(i => i.Id == abilityId);
                AbilityCard abilityCard = Instantiate(abilityCardPrefab, _cardsRoot);
                abilityCard.Initialize(abilityInfo);
                abilityCard.SetFade(1.0f);
                _abilityCards.Add(abilityCard);
            }

            _cardsRoot.gameObject.SetActive(true);
        }


        private void DisposeCards()
        {
            _enemyInfo = null;
            foreach (var abilityCard in _abilityCards)
            {
                Destroy(abilityCard.gameObject);
            }

            _abilityCards.Clear();

            if(_cardsRoot)
                _cardsRoot.gameObject.SetActive(false);
        }

        #endregion
    }
}
