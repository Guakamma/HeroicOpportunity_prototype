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
        #region Fields

        private readonly Dictionary<AbilityType, AbilityInfo> _heroInfos;

        #endregion



        #region Properties

        private AbilitiesData AbilitiesData => DataHub.Abilities;

        #endregion



        #region Class lifecycle

        public AbilitiesServicesImpl()
        {
            _heroInfos = new Dictionary<AbilityType, AbilityInfo>();
            foreach (var p in AbilitiesData.AbilitiesInfoPaths)
            {
                _heroInfos.Add(p.Key, Resources.Load<AbilityInfo>(p.Value));
            }
        }

        #endregion



        #region IAbilitiesService

        public AbilityInfo[] GetAllAbilityInfos()
        {
            List<AbilityInfo> allAbilityInfos = _heroInfos.Values.ToList();
            allAbilityInfos.Sort((x, y) => x.Type.CompareTo(y.Type));
            return allAbilityInfos.ToArray();
        }

        #endregion
    }
}
