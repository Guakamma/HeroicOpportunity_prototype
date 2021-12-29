using System;
using HeroicOpportunity.Data.Abilities;
using UniRx;


namespace HeroicOpportunity.Services.Events
{
    public class AbilityEvents
    {
        private readonly Subject<AbilityInfo> _abilityDamage = new Subject<AbilityInfo>();
        private readonly Subject<int> _abilityComboDamage = new Subject<int>();


        public IObservable<AbilityInfo> AbilityDamage => _abilityDamage.AsObservable();
        public IObservable<int> AbilityComboDamage => _abilityComboDamage.AsObservable();


        public void Damage(AbilityInfo abilityInfo) => _abilityDamage.OnNext(abilityInfo);
        public void ComboDamage(int damage) => _abilityComboDamage.OnNext(damage);
    }
}
