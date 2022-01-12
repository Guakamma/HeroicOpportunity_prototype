using System;
using System.Collections.Generic;
using System.Linq;
using Character.Enemy;
using Character.Hero;
using Data.Levels;
using Game;
using HeroicOpportunity.Data.Enemies;
using HeroicOpportunity.Game;
using HeroicOpportunity.Level;
using HeroicOpportunity.Services.Events;
using Services;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Level
{
    public class LevelController : MonoBehaviour
    {
        #region Fields

        private const int SizePool = 3;
        private const int CountChunkBefore = 1;

        private LevelInfo _levelInfo;
        private Transform _target;
        private List<Chunk> _chunks;
        private List<Chunk> _chunksPool;
        private List<BaseEnemyController> _enemies;
        private CompositeDisposable _spawnDisposable;
        private List<float> _lastSpawnPositionsX;

        #endregion



        #region Properties

        public BaseEnemyController ActiveEnemy => _enemies.FirstOrDefault();
        public bool IsBossLevel => _levelInfo.IsBossLevel;

        #endregion


        #region Public methods

        public void Initialize(LevelInfo levelInfo)
        {
            _levelInfo = levelInfo;
            _spawnDisposable = new CompositeDisposable();
            _lastSpawnPositionsX = new List<float>(3);

            IEventsService eventsService = ServicesHub.Events;
            eventsService.Hero.HeroCreated
                .Subscribe(HeroCreated)
                .AddTo(this);

            eventsService.Enemy.EnemyDied
                .Subscribe(OnEnemyDied)
                .AddTo(this);

            GameStateController.OnStateChanged
                .Subscribe(OnStateChanged)
                .AddTo(this);

            _chunks = new List<Chunk>();

            CreateChunkPool();
            SetFirstChunks();

            Observable.EveryUpdate()
                .Where(_ => _target != null)
                .Select(_ => _target.position)
                .Subscribe(CheckChunks)
                .AddTo(this);

        }
        
        private void OnStateChanged(GameStateType gameStateType)
        {
            if (gameStateType == GameStateType.InGame)
            {
                if (!_levelInfo.IsBossLevel)
                {
                    Observable.Timer(TimeSpan.FromSeconds(_levelInfo.Duration))
                        .Subscribe(_ => LevelWin())
                        .AddTo(this);
                }

                float startDelay = _levelInfo.IsBossLevel ? 0.5f : _levelInfo.SpawnRate;
                Observable.Timer(TimeSpan.FromSeconds(startDelay), TimeSpan.FromSeconds(_levelInfo.SpawnRate))
                    .Subscribe(_ => SpawnTrashEnemy())
                    .AddTo(this);
            }
            else
            {
                StopSpawnTrash();
            }
        }

        private void StopSpawnTrash()
        {
            _spawnDisposable.Dispose();
        }

        private void SpawnTrashEnemy()
        {
            LevelController level = ServicesHub.Level.ActiveLevel;
            Vector3 spawnPosition = transform.position;
            
            float randomX = level.GetRandomRoadX();
            int tries = 0;
            while (_lastSpawnPositionsX.Count > (0 + tries) && tries < 3 && 
                   Mathf.Approximately(_lastSpawnPositionsX[_lastSpawnPositionsX.Count - (1 + tries)], randomX))
            {
                randomX = level.GetRandomRoadX();
                tries++;
            }

            spawnPosition.x = randomX;
            _lastSpawnPositionsX.Add(randomX);
            
            BaseEnemyController enemy = ServicesHub.Enemies.CreateEnemy(EnemyType.Default, level.transform, spawnPosition);
            enemy.Show();
            enemy.SetIsShoot(true);
        }

        private void OnEnemyDied(BaseEnemyController enemy)
        {
            if (_enemies.Contains(enemy))
                _enemies.Remove(enemy);

            if (_enemies.Count <= 0)
            {
                LevelWin();
            }
            else
            {
                _enemies.First().Show();
            }
        }

        public float GetMiddlePositionX()
        {
            return transform.position.x;
        }


        public float GetStepX()
        {
            return _levelInfo.RoadSize * 0.5f / (_levelInfo.QuantityRoads - 1);
        }


        public Vector2 MinMaxPositionX()
        {
            Vector2 positions = new Vector2(GetMiddlePositionX(), GetMiddlePositionX());
            positions.x -= GetStepX() * (_levelInfo.QuantityRoads - 2);
            positions.y += GetStepX() * (_levelInfo.QuantityRoads - 2);
            return positions;
        }

        #endregion



        #region Private methods

        private void CreateChunkPool()
        {
            _chunksPool = new List<Chunk>();
            for (int i = 0; i < SizePool; i++)
            {
                Chunk chunk = Instantiate(_levelInfo.ChunkPrefab, Vector3.zero, Quaternion.identity, transform);
                // chunk.gameObject.SetActive(false);
                _chunksPool.Add(chunk);
            }
        }


        private void SetFirstChunks()
        {
            Vector3 position = Vector3.zero;
            for (int i = 0; i < _chunksPool.Count; i++)
            {
                Chunk chunk = _chunksPool[i];
                if (i == 0)
                {
                    position.z -= chunk.Size.y * 0.5f;
                }
                else
                {
                    position = GetLastChunkEndPosition();
                }

                SetChunk(position);
            }
        }

        private void CheckChunks(Vector3 heroPosition)
        {
            Chunk inChunk = _chunks.FirstOrDefault(c => c.InChunk(heroPosition));
            if (inChunk == null)
            {
                return;
            }

            //Set Before
            Chunk[] chunksBefore = _chunks.Where(c => c.Position.z > inChunk.Position.z).ToArray();
            if (chunksBefore.Length < CountChunkBefore)
            {
                for (int i = 0; i < CountChunkBefore - chunksBefore.Length; i++)
                {
                    Chunk lastChunk = _chunks.MaxBy(p => p.Position.z);
                    Vector3 position = lastChunk.Position;
                    position.z += lastChunk.Size.y * 0.5f;
                    SetChunk(position);
                }
            }

            //Hide After
            Chunk[] chunksAfter = _chunks.Where(c => c.Position.z < inChunk.Position.z).ToArray();
            if (chunksAfter.Length > 0)
            {
                for (int i = _chunks.Count - 1; i >= 0; i--)
                {
                    Chunk c = _chunks[i];
                    if (c.Position.z + c.Size.y < heroPosition.z)
                    {
                       ReturnChunkToPool(c);
                    }
                }
            }
        }


        private Vector3 GetLastChunkEndPosition()
        {
            Vector3 position = Vector3.zero;
            if (_chunks.Count != 0)
            {
                Chunk lastChunk = _chunks.MaxBy(p => p.Position.z);
                position = lastChunk.Position;
                position.z += lastChunk.Size.y * 0.5f;
            }

            return position;
        }

        private void SetChunk(Vector3 endChunkPosition)
        {
            Chunk chunk = _chunksPool.First();
            _chunksPool.RemoveAt(0);
            // chunk.gameObject.SetActive(true);

            Vector3 position = endChunkPosition;
            position.z += chunk.Size.y * 0.5f;
            chunk.Position = position;

            chunk.gameObject.name = _chunks.Count.ToString();
            _chunks.Add(chunk);
        }


        private void ReturnChunkToPool(Chunk chunk)
        {
            // c.gameObject.SetActive(false);
            _chunksPool.Add(chunk);
            _chunks.Remove(chunk);
        }


        private void HeroCreated(HeroController hero)
        {
            _target = hero.transform;
            CreateEnemies();
        }


        private void CreateEnemies()
        {
            if (_enemies == null)
            {
                _enemies = new List<BaseEnemyController>();
            }
            else
            {
                DestroyEnemies();
            }

            EnemyType[] enemyIds = _levelInfo.EnemyIds;
            for (int i = 0; i < enemyIds.Length; i++)
            {
                BaseEnemyController enemy = ServicesHub.Enemies.CreateEnemy(enemyIds[i], transform);
                if (i == 0)
                {
                    enemy.Show();
                }
                else
                {
                    enemy.Hide();
                }

                _enemies.Add(enemy);
            }
            _lastSpawnPositionsX.Add(0);
        }

        private float GetRandomRoadX()
        {
            int roadIndex = Random.Range(1, 4);
            switch (roadIndex)
            {
                case 1:
                    return GetMiddlePositionX() - GetStepX();

                case 3:
                    return GetMiddlePositionX() + GetStepX();

                default:
                    return GetMiddlePositionX();
            }
        }

        private void DestroyEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Dispose();
            }

            _enemies.Clear();
        }


        public void LevelWin()
        {
            StopSpawnTrash();
            ServicesHub.Level.IncrementLevelNumber();
            GameManager.Instance.SetGameState(GameStateType.Result);
        }

        #endregion
    }
}
