using System;
using UnityEngine;


public class PoolObject : MonoBehaviour
{
    public event Action<Transform> OnReturnToPool;

    public bool IsActive => gameObject.activeSelf;
    public void Create()
    {
        gameObject.SetActive(true);
    }


    public virtual void ReturnToPool()
    {
        gameObject.SetActive(false);
        OnReturnToPool?.Invoke(transform);
    }
}
