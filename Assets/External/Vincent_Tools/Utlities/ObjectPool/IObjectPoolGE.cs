using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolGE<T>
{
    public void InitPool(bool _isObjectActivated = true);

    public T GetObjectFromPool();
    public void ReturnObjectToPool(T _pooledObject, bool _isObjectActivated = true);
}
