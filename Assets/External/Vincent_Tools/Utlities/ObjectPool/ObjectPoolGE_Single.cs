using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Example to Use:
    Pull from Pool -
    var object = GetObjectFromPool();
    List.Add(object);
    
    Put in Pool - 
    ReturnObjetToPool(List[0]);
*/
public class ObjectPoolGE_Single<T> : MonoBehaviour,IObjectPoolGE<T> where T : MonoBehaviour
{
    public T prefab;
    public int poolSize;

    private List<T> m_freeList;
    private List<T> m_usedList;

    public void InitPool(bool _isObjectActivated = true)
    {
        m_freeList = new List<T>(poolSize);
        m_usedList = new List<T>(poolSize);

        for (int i = 0; i < poolSize; i++)
        {
            var pooledObject = Instantiate(prefab, transform);
            pooledObject.gameObject.SetActive(_isObjectActivated);
            m_freeList.Add(pooledObject);
        }
    }

    public T GetObjectFromPool() {
        int numFree = m_freeList.Count;
        if (numFree == 0)
            return null;

        var pooledObject = m_freeList[numFree - 1];
        m_freeList.RemoveAt(numFree - 1);
        m_usedList.Add(pooledObject);
        return pooledObject;
    }

    public void ReturnObjectToPool(T _pooledObject)
    {
       
    }

    public void ReturnObjectToPool(T _pooledObject, bool _isObjectActivated = true)
    {
        Debug.Assert(m_usedList.Contains(_pooledObject));

        m_usedList.Remove(_pooledObject);
        m_freeList.Add(_pooledObject);

        var pooledObjectTransform = _pooledObject.transform;
        pooledObjectTransform.parent = transform;
        pooledObjectTransform.localPosition = Vector3.zero;
        _pooledObject.gameObject.SetActive(_isObjectActivated);
    }
}
