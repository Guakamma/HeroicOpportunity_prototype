using System.Collections.Generic;
using System.Linq;
using Data.Abilities;
using HeroicOpportunity.Data;
using HeroicOpportunity.Data.Abilities;
using UnityEngine;

namespace Services.Abilities
{
    public class ComboServiceImpl : IComboService
    {
        private readonly Dictionary<string, ComboInfo> _comboInfos;

        private ComboData ComboData => DataHub.Combo;

        public ComboServiceImpl()
        {
            _comboInfos = new Dictionary<string, ComboInfo>();
            foreach (var p in ComboData.ComboInfoPaths)
            {
                _comboInfos.Add(p.Key, Resources.Load<ComboInfo>(p.Value));
            }
        }

        public ComboInfo[] GetAllComboInfos()
        {
            return _comboInfos.Values.ToArray();
        }

        public ComboInfo GetRandomComboInfo()
        {
            int length = _comboInfos.Values.Count;
            return _comboInfos.Values.ToArray()[Random.Range(0, length)];
        }
    }
}