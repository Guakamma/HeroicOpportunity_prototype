using System.Collections.Generic;
using System.Linq;
using Data.Abilities;
using HeroicOpportunity.Data;
using HeroicOpportunity.Data.Abilities;
using HeroicOpportunity.Services.Abilities;
using UnityEngine;

namespace Services.Abilities
{
    public class AbilitiesServicesImpl : IAbilitiesService
    {
        private readonly Dictionary<AbilityType, AbilityInfo> _heroInfos;

        private AbilitiesData AbilitiesData => DataHub.Abilities;


        public AbilitiesServicesImpl()
        {
            _heroInfos = new Dictionary<AbilityType, AbilityInfo>();
            foreach (var p in AbilitiesData.AbilitiesInfoPaths)
            {
                _heroInfos.Add(p.Key, Resources.Load<AbilityInfo>(p.Value));
            }
        }

        public AbilityInfo[] GetAllAbilityInfos()
        {
            List<AbilityInfo> allAbilityInfos = _heroInfos.Values.ToList();
            allAbilityInfos.Sort((x, y) => x.Type.CompareTo(y.Type));
            return allAbilityInfos.ToArray();
        }
    }
}
