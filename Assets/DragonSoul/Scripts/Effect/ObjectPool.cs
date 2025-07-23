using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    Dictionary<int, List<GameObject>> poolObjects = new Dictionary<int, List<GameObject>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public GameObject GetGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int key = prefab.GetInstanceID();

        if (!poolObjects.ContainsKey(key))
        {
            poolObjects.Add(key, new List<GameObject>());
        }

        List<GameObject> gameObjects = poolObjects[key];

        GameObject go;

        for (int i = 0; i < gameObjects.Count; i++)
        {
            go = gameObjects[i];

            if (!go.activeInHierarchy)
            {
                go.transform.position = position;

                go.transform.rotation = rotation;

                go.SetActive(true);

                return go;
            }
        }

        go = Instantiate(prefab, position, rotation);

        go.transform.parent = transform;

        gameObjects.Add(go);

        return go;
    }

    public void ReleaseGameObject(GameObject go)
    {
        go.SetActive(false);
    }
}
