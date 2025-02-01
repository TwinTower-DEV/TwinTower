using UnityEngine;

namespace TwinTower
{
    public class ResourceManager
    {
        public T Load<T>(string path) where T : Object
        {
            /*if (typeof(T) == typeof(GameObject))
            {
                string name = path;

                // path에서 오브젝트의 이름을 추출
                int index = name.LastIndexOf('/');
                if (index >= 0)
                    name = name.Substring(index + 1);

                GameObject go = PoolManager.Instance.GetOriginal(name);
                if (go != null)
                    return go as T;
            }*/
            return Resources.Load<T>(path);
        }
        
        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject original = Load<GameObject>($"Prefabs/{path}");
            if (original == null)
            {
                Debug.Log($"Failed to load prefab : {path}");
                return null;
            }

            // Poolable 컴포넌트가 붙어있다면 오브젝트 풀링을 한다.
            /*
            if (original.GetComponent<Poolable>() != null)
                return PoolManager.Instance.Pop(original, parent).gameObject;
                */
            GameObject go = Object.Instantiate(original, parent);
            go.name = original.name;
            return go;
        }
        
        public void Destroy(GameObject go)
        {
            if (go == null)
                return;
            Object.Destroy(go);
        }
    }
}