using System.Linq;
using HeroicOpportunity.Data;
using HeroicOpportunity.Data.Enemies;
using HeroicOpportunity.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Levels
{
    [CreateAssetMenu(fileName = "Data_Levels_Infos_number", menuName = "Data/Levels/Levels Info")]
    public class LevelInfo : ScriptableObject
    {
        [SerializeField]
        private Chunk _chunkPrefab;

        [Space]
        [SerializeField] [Min(1.0f)]
        private float _roadSize = 1.0f;

        [SerializeField] [Min(1)]
        [OnValueChanged(nameof(CheckQuantityRoads))]
        private int _quantityRoads = 1;

        [Header("Enemies")]
        [SerializeField] [Required]
        [OnValueChanged(nameof(CheckCorrectnessEnemies))]
        private EnemyType[] _enemies;
        
        [SerializeField] [Required] [Min(1.0f)]
        private float _spawnRate;
        
        [SerializeField]
        [Required] [Min(0.0f)]
        [HideIf(nameof(IsBossLevel))]
        private float _duration;
        
        public Chunk ChunkPrefab => _chunkPrefab;
        public float RoadSize => _roadSize;
        public int QuantityRoads => _quantityRoads;
        
        public EnemyType[] EnemyIds => _enemies;
        public float Duration => _duration;  
        public bool IsBossLevel => _enemies.Contains(EnemyType.Boss);
        public float SpawnRate => _spawnRate;

        private void CheckQuantityRoads()
        {
            if (_quantityRoads % 2 == 1)
            {
                return;
            }

            _quantityRoads -= 1;
        }

        private void CheckCorrectnessEnemies()
        {
            if (_enemies.Contains(EnemyType.Boss))
            {
                _enemies = new[] {EnemyType.Boss};
            }
        }
    }
}
