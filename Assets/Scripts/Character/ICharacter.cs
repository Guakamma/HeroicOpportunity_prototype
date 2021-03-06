using UnityEngine;


namespace HeroicOpportunity.Character
{
    public interface ICharacter
    {
        GameObject Root { get; }
        int Damage { get; }
        int Health { get; }

        void GetDamage(int value);
    }
}
