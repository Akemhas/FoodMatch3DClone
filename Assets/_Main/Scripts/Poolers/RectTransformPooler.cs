using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RectTransformPooler : Singleton<RectTransformPooler>
{
    [SerializeField] private List<PoolClass> poolClasses;
    private Dictionary<string, ObjectPool<RectTransform>> _rtDict;

    [Serializable]
    public class PoolClass
    {
        public string tag;
        public RectTransform prefab;
        public int softCap, hardCap;
        public bool createAtStart;
    }

    private void Awake()
    {
        Initialize();
        CreateAtStart();
    }

    private void CreateAtStart()
    {
        for (int i = 0; i < poolClasses.Count; i++)
        {
            var p = poolClasses[i];
            if (p.createAtStart)
            {
                for (int j = 0; j < p.softCap; j++)
                {
                    _rtDict[p.tag].Get();
                }
            }
        }
    }

    private void Initialize()
    {
        _rtDict = new Dictionary<string, ObjectPool<RectTransform>>();
        var listCount = poolClasses.Count;
        for (int i = 0; i < listCount; i++)
        {
            ObjectPool<RectTransform> pool = new(CreateFunction(i), OnObjectGet, OnObjectRelease, OnObjectDestroy, true, poolClasses[i].softCap, poolClasses[i].hardCap);
            string prefabName = poolClasses[i].tag;
            _rtDict.Add(prefabName, pool);
        }
    }

    public RectTransform Get(string poolTag, Vector3 position)
    {
        var pooledObject = _rtDict[poolTag].Get();
        var t = pooledObject.transform;
        t.position = position;
        pooledObject.gameObject.SetActive(true);
        return pooledObject;
    }

    public RectTransform Get(string poolTag, Vector3 position, Quaternion rotation)
    {
        var pooledObject = _rtDict[poolTag].Get();
        var t = pooledObject.transform;
        t.position = position;
        t.rotation = rotation;
        pooledObject.gameObject.SetActive(true);
        return pooledObject;
    }

    public RectTransform Get(string poolTag, Transform parent)
    {
        var pooledObject = _rtDict[poolTag].Get();
        var t = pooledObject.transform;
        t.transform.SetParent(parent);
        t.localPosition = Vector3.zero;
        pooledObject.gameObject.SetActive(true);
        return pooledObject;
    }

    public RectTransform Get(string poolTag, Vector3 position, Transform parent)
    {
        var pooledObject = _rtDict[poolTag].Get();
        var t = pooledObject.transform;
        t.position = position;
        t.SetParent(parent);
        pooledObject.gameObject.SetActive(true);
        return pooledObject;
    }

    public RectTransform Get(string poolTag, Vector3 position, Quaternion rotation, Transform parent)
    {
        var pooledObject = _rtDict[poolTag].Get();
        var t = pooledObject.transform;
        t.position = position;
        t.rotation = rotation;
        t.SetParent(parent);
        pooledObject.gameObject.SetActive(true);
        return pooledObject;
    }

    private void OnObjectGet(RectTransform pooledObject)
    {
        pooledObject.transform.SetParent(transform);
        pooledObject.gameObject.SetActive(true);
    }

    private Func<RectTransform> CreateFunction(int i)
    {
        return new Func<RectTransform>(() =>
        {
            var pooledObject = Instantiate(this.poolClasses[i].prefab);
            return pooledObject;
        });
    }

    private void OnObjectRelease(RectTransform pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    public void Release(string poolTag, RectTransform pooledObject)
    {
        _rtDict[poolTag].Release(pooledObject);
    }

    private void OnObjectDestroy(RectTransform pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}