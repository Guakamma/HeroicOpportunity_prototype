using HeroicOpportunity.Level;
using Sirenix.OdinInspector;
using UnityEngine;


namespace HeroicOpportunity.Data.Levels
{
    [CreateAssetMenu(fileName = "Data_Levels_Infos_number", menuName = "Data/Levels/Levels Info")]
    public class LevelInfo : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private Chunk _chunkPrefab;

        [Space]
        [SerializeField] [Min(1.0f)]
        private float _roadSize = 1.0f;

        [SerializeField] [Min(1)]
        [OnValueChanged(nameof(CheckQuantityRoads))]
        private int _quantityRoads = 1;

        [Space]
        [SerializeField]
        // [ValueDropdown(nameof(EnemiesIds))]
        private string[] _enemyIds;

        #endregion



        #region Properties

        public Chunk ChunkPrefab => _chunkPrefab;
        public float RoadSize => _roadSize;
        public int QuantityRoads => _quantityRoads;


        public string[] EnemyIds => _enemyIds;

        #endregion



        #region Private methods

        private void CheckQuantityRoads()
        {
            if (_quantityRoads % 2 == 1)
            {
                return;
            }

            _quantityRoads -= 1;
        }


        private string[] EnemiesIds()
        {
            return DataHub.Enemies.GetAllIds();
        }

        #endregion
    }
}
