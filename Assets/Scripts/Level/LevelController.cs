using System;
using System.Collections.Generic;
using System.Linq;
using HeroicOpportunity.Character.Enemy;
using HeroicOpportunity.Character.Hero;
using HeroicOpportunity.Data.Levels;
using HeroicOpportunity.Game;
using HeroicOpportunity.Services;
using HeroicOpportunity.Services.Events;
using UniRx;
using UnityEngine;


namespace HeroicOpportunity.Level
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

        #endregion



        #region Properties

        public BaseEnemyController ActiveEnemy => _enemies.FirstOrDefault();

        #endregion


        #region Public methods

        public void Initialize(LevelInfo levelInfo)
        {
            _levelInfo = levelInfo;

            IEventsService eventsService = ServicesHub.Events;
            eventsService.Hero.HeroCreated
                .Subscribe(HeroCreated)
                .AddTo(this);

            eventsService.Enemy.EnemyDied
                .Subscribe(e =>
                {
                    _enemies.Remove(e);
                    if (_enemies.Count <= 0)
                    {
                        LevelWin();
                    }
                    else
                    {
                        _enemies.First().Show();
                    }
                })
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

            string[] enemyIds = _levelInfo.EnemyIds;
            for (int i = 0; i < enemyIds.Length; i++)
            {
                var enemy = ServicesHub.Enemies.CreateEnemy(enemyIds[i], transform);
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
        }


        private void DestroyEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Dispose();
            }

            _enemies.Clear();
        }


        private void LevelWin()
        {
            ServicesHub.Level.IncrementLevelNumber();
            GameManager.Instance.SetGameState(GameStateType.Result);
        }

        #endregion
    }
}
