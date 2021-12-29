using System.Collections.Generic;
using System.Linq;
using HeroicOpportunity.Data;
using HeroicOpportunity.Data.Abilities;
using UnityEngine;


namespace HeroicOpportunity.Services.Abilities
{
    public class AbilitiesServicesImpl : IAbilitiesService
    {
        #region Fields

        private readonly Dictionary<string, AbilityInfo> _heroInfos;

        #endregion



        #region Properties

        private AbilitiesData AbilitiesData => DataHub.Abilities;

        #endregion



        #region Class lifecycle

        public AbilitiesServicesImpl()
        {
            _heroInfos = new Dictionary<string, AbilityInfo>();
            foreach (var p in AbilitiesData.AbilitiesInfoPaths)
            {
                _heroInfos.Add(p.Key, Resources.Load<AbilityInfo>(p.Value));
            }
        }

        #endregion



        #region IAbilitiesService

        public AbilityInfo[] GetAllAbilityInfos()
        {
            return _heroInfos.Values.ToArray();
        }

        #endregion
    }
}
