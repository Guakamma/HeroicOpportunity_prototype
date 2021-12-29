using System;
using HeroicOpportunity.Gun;
using Sirenix.OdinInspector;
using UnityEngine;


namespace HeroicOpportunity.Data
{
    [Serializable]
    public class GunInfo
    {
        #region Fields

        [Space]
        [SerializeField] [Min(0.0f)]
        private float _fireRate;

        [Header("Bullet")]
        [SerializeField] [Required]
        private Bullet _bulletPrefab;
        [SerializeField] [Min(0.0f)]
        private float _bulletSpeed;
        [SerializeField] [Min(0.0f)]
        private float _bulletLifetime;

        #endregion



        #region Properties

        public float FireRate => _fireRate;

        public Bullet BulletPrefab => _bulletPrefab;
        public float BulletSpeed => _bulletSpeed;
        public float BulletLifetime => _bulletLifetime;

        #endregion
    }
}
