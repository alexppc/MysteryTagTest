using UnityEngine;
using System.Collections.Generic;
using System;
using UniRx;

namespace Pools
{
    //для инспектора
    public enum PoolObjectTypes { Asteroid, Bullets }

    public interface IPooled
    {
        void Reset();
        void Bind(BaseModel model);
    }

    public interface IPool { }

    public interface IPool<T> : IPool where T : MonoBehaviour, IPooled
    {
        T GetObject();
        void ReturnObject(T obj);
        void Clear();
        void ChangePrefab(GameObject prefab);
    }

    [Serializable]
    public class PoolParameters
    {
        public PoolObjectTypes Type;
        public GameObject Prefab;
        public Transform Parent;
    }

    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private PoolParameters[] m_Params;
        private Dictionary<System.Type, IPool> m_Pools;


        public T GetObject<T>() where T : MonoBehaviour, IPooled
        {
            return GetPool<T>().GetObject();
        }

        public void ReturnObject<T>(T obj) where T : MonoBehaviour, IPooled
        {
            GetPool<T>().ReturnObject(obj);
        }

        public void ClearPool<T>() where T : MonoBehaviour, IPooled
        {
            GetPool<T>().Clear();
        }

        public void ChangePrefab<T>(GameObject prefab) where T : MonoBehaviour, IPooled
        {
            GetPool<T>().ChangePrefab(prefab);
        }

        public IPool<T> GetPool<T>() where T : MonoBehaviour, IPooled
        {
            return m_Pools[typeof(T)] as IPool<T>;
        }

        private void InitPools()
        {
            m_Pools = new Dictionary<Type, IPool>(2)
            {
                { typeof(Asteroids.AsteroidInfoMono), new ReactivePool<Asteroids.AsteroidInfoMono>(GetParamsByEnum(PoolObjectTypes.Asteroid)) },
                { typeof(Bullets.BulletInfoMono), new ReactivePool<Bullets.BulletInfoMono>(GetParamsByEnum(PoolObjectTypes.Bullets)) }
            };
        }

        private void Awake()
        {
            InitPools();
        }

        private PoolParameters GetParamsByEnum(PoolObjectTypes type)
        {
            for (int i = 0; i < m_Params.Length; ++i)
            {
                if (m_Params[i].Type == type) return m_Params[i];
            }
            return null;
        }
    }

    public class Pool<T> : IPool<T> where T : MonoBehaviour, IPooled
    {
        private List<T> m_ActiveObjects, m_DeactiveObjects;
        private GameObject m_Prefab;
        private Transform m_Parent;
        private int m_SpawnedCount = 0;

        public Pool(PoolParameters param)
        {
            m_Prefab = param.Prefab;
            m_Parent = param.Parent;
            m_ActiveObjects = new List<T>(5);
            m_DeactiveObjects = new List<T>(5);
        }

        public void ChangePrefab(GameObject prefab)
        {
            m_Prefab = prefab;
        }

        public T GetObject()
        {
            T obj;
            if (m_DeactiveObjects.Count > 0)
            {
                obj = m_DeactiveObjects[0];
                m_DeactiveObjects.RemoveAt(0);
            }
            else
            {
                obj = Spawn();
                if (obj == null)
                {
                    //error
                    return null;
                }
            }
            m_ActiveObjects.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.Reset();
            m_ActiveObjects.Remove(obj);
            m_DeactiveObjects.Add(obj);
        }

        public void Clear()
        {
            for (int i = m_ActiveObjects.Count - 1; i >= 0; --i)
            {
                GameObject.Destroy(m_ActiveObjects[i].gameObject);
            }
            for (int i = m_DeactiveObjects.Count - 1; i >= 0; --i)
            {
                GameObject.Destroy(m_DeactiveObjects[i].gameObject);
            }
            m_ActiveObjects.Clear();
            m_DeactiveObjects.Clear();
            m_SpawnedCount = 0;
        }

        private T Spawn()
        {
            m_SpawnedCount++;
            GameObject go = GameObject.Instantiate(m_Prefab, m_Parent);
            go.name = String.Concat(m_Prefab.name, " ", m_SpawnedCount.ToString());
            T obj = go.GetComponent<T>();
            return obj;
        }
    }

    public class ReactivePool<T> : IPool<T> where T : MonoBehaviour, IPooled
    {
        private readonly ReactiveCollection<T> m_PooledObjects;
        private GameObject m_Prefab;
        private Transform m_Parent;
        private int m_SpawnedCount = 0;


        public ReactivePool(PoolParameters param)
        {
            m_Prefab = param.Prefab;
            m_Parent = param.Parent;
            m_PooledObjects = new ReactiveCollection<T>();
            m_PooledObjects
                .ObserveAdd()
                .Subscribe(AddInCollection);
            m_PooledObjects
                .ObserveRemove()
                .Subscribe(RemoveAtCollection);
        }

        public void ChangePrefab(GameObject prefab)
        {
            m_Prefab = prefab;
        }

        public void Clear()
        {
            for (int i = m_PooledObjects.Count - 1; i >= 0; --i)
            {
                GameObject.Destroy(m_PooledObjects[i].gameObject);
            }
            m_PooledObjects.Clear();
            m_SpawnedCount = 0;
        }

        public T GetObject()
        {
            T obj;
            if (m_PooledObjects.Count > 0)
            {
                obj = m_PooledObjects[0];
                m_PooledObjects.RemoveAt(0);
            }
            else
            {
                obj = Spawn();
                if (obj == null)
                {
                    //error
                    return null;
                }
                obj.Reset();
                obj.gameObject.SetActive(true);
            }
            return obj;
        }

        public void ReturnObject(T obj)
        {
            m_PooledObjects.Add(obj);
        }

        private T Spawn()
        {
            m_SpawnedCount++;
            GameObject go = GameObject.Instantiate(m_Prefab, m_Parent);
            go.name = String.Concat(m_Prefab.name, " ", m_SpawnedCount.ToString());
            T obj = go.GetComponent<T>();
            return obj;
        }

        private void AddInCollection(CollectionAddEvent<T> addEvent)
        {
            T obj = addEvent.Value;
            obj.gameObject.SetActive(false);
            obj.Reset();
        }

        private void RemoveAtCollection(CollectionRemoveEvent<T> removeEvent)
        {
            T obj = removeEvent.Value;
            obj.Reset();
            obj.gameObject.SetActive(true);
        }
    }
}