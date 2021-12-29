using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectsPool<T> where T : PoolObject
{
    #region Fields

    private readonly T _prefab;
    private readonly GameObject _root;
    private List<T> _pool;

    #endregion



    #region Class lifecycle

    public ObjectsPool(T prefab, int instantiationCount = 1)
    {
        _prefab = prefab;
        _root = new GameObject("ProjectsPool" + _prefab);
        _pool = new List<T>();

        for (int i = 0; i < instantiationCount; i++)
        {
            InstantPrefab();
        }
    }


    ~ObjectsPool()
    {
        DestroyPool();
    }

    #endregion



    #region Public methods

    public void DestroyPool()
    {
        foreach (var o in _pool)
        {
            Object.Destroy(o.gameObject);
        }

        _pool.Clear();
        _pool = null;

        Object.Destroy(_root);
    }


    public T GetObject(Vector3 position, Quaternion rotation = default, Transform parent = null)
    {
        T poolObject = _pool.FirstOrDefault(o => !o.IsActive);
        if (poolObject == null)
        {
            poolObject = InstantPrefab();
        }

        if (parent != null)
        {
            poolObject.transform.SetParent(parent);
        }

        poolObject.transform.SetPositionAndRotation(position, rotation);
        poolObject.Create();
        poolObject.OnReturnToPool += PoolObject_OnReturnToPool;
        return poolObject;
    }




    #endregion



    #region Private methods

    private T InstantPrefab()
    {
        T poolObject = Object.Instantiate(_prefab, _root.transform);
        _pool.Add(poolObject);
        poolObject.ReturnToPool();
        return poolObject;
    }

    #endregion



    #region Events handlers

    private void PoolObject_OnReturnToPool(Transform transform)
    {
        transform.SetParent(_root.transform);
    }

    #endregion
}
