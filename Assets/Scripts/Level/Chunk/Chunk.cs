using UnityEngine;


namespace HeroicOpportunity.Level
{
    public class Chunk : MonoBehaviour
    {
        #region Fields

        [SerializeField] [Min(0.0f)]
        private Vector2 _size;

        [SerializeField] [Min(0.0f)]
        private float _roadSize;

        #endregion



        #region Properties

        public Vector2 Size => _size;


        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        #endregion



        #region Unity lifecycle

        private void OnDrawGizmosSelected()
        {
            Vector3 position = transform.position;

            //Chunk size
            Gizmos.color = Color.red;
            Vector3 size = new Vector3(_size.x, 1.0f, _size.y);
            Gizmos.DrawWireCube(position, size);

            //Road size
            Gizmos.color = Color.yellow;
            size = new Vector3(_roadSize, 1.0f, 1.0f);
            Gizmos.DrawWireCube(position, size);
        }

        #endregion



        #region Public methods

        public bool InChunk(Vector3 position)
        {
            float halfSize = _size.y * 0.5f;
            var positionZ = transform.position.z;
            return positionZ + halfSize >= position.z && positionZ - halfSize <= position.z;
        }

        #endregion
    }
}
