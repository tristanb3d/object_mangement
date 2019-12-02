using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //used to change scences

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{

    [SerializeField]
    Shape[] prefabs;

    [SerializeField]
    Material[] materials;

    [SerializeField]
    bool recycle;
    // create a recycle bool
    List<Shape>[] pools;
    //create/declare a list within shape make array/list thing to stpore the pools value// 
    Scene poolScene;
    //checks the list indext for shape pool
    public Shape Get(int shapeId = 0, int materialId = 0)
    {
        Shape instance;
        if (recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }
            List<Shape> pool = pools[shapeId];
            int lastIndex = pool.Count - 1;
            if (lastIndex >= 0)
            {
                instance = pool[lastIndex];
                instance.gameObject.SetActive(true);
                pool.RemoveAt(lastIndex);
            }   //checks the pools index and instanced//creats a game object and removes the index
            else
            { //checks shape id,  and creates a gamebojev for the poolScence
                instance = Instantiate(prefabs[shapeId]);
                instance.ShapeId = shapeId;
                SceneManager.MoveGameObjectToScene(
                    instance.gameObject, poolScene
                );
            }
        }
        else
        {
            instance = Instantiate(prefabs[shapeId]);
            instance.ShapeId = shapeId;
        }

        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }
    //sets_random_range
    public Shape GetRandom()
    {
        return Get(
            Random.Range(0, prefabs.Length),
            Random.Range(0, materials.Length)
        );
    }

    public void Reclaim(Shape shapeToRecycle)
    {
        if (recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }
            pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
            shapeToRecycle.gameObject.SetActive(false);
        }
        else
        {
            Destroy(shapeToRecycle.gameObject);
        }
    }
    //creates pools, adds a prefab in the list and pool are a part of the list
    void CreatePools()
    { //if rootobject lenth is less than 0, poolshape gets the rootobjects shape
        pools = new List<Shape>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<Shape>();
        }

        if (Application.isEditor)
        {
            poolScene = SceneManager.GetSceneByName(name);
            if (poolScene.isLoaded)
            {
                GameObject[] rootObjects = poolScene.GetRootGameObjects();
                for (int i = 0; i < rootObjects.Length; i++)
                {
                    Shape pooledShape = rootObjects[i].GetComponent<Shape>();
                    if (!pooledShape.gameObject.activeSelf)
                    {
                        pools[pooledShape.ShapeId].Add(pooledShape);
                    }
                } // checks if pool scence is lided in the editor, and checks te game jojects, 
                  //gets the pool shape componet from root object, 
                return;
            }
        }

        poolScene = SceneManager.CreateScene(name);  //names the scence
    }
}