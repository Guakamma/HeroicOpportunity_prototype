using HeroicOpportunity.Character.Hero;
using UnityEngine;


namespace HeroicOpportunity.Services.Hero
{
    public interface IHeroService
    {
        HeroController ActiveHero { get; }
        HeroController CreateHero(Transform root, Vector3 position);
        void DisposeActiveHero();
    }
}
