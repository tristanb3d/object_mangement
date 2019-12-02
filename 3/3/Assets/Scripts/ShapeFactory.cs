using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject {

	[SerializeField]
	Shape[] prefabs;
    //giving a shapes an array field
    [SerializeField]
	Material[] materials;
    //material array //serized
    [SerializeField]
	bool recycle;

	List<Shape>[] pools;
    //get tjhe shape and give it an ID
    public Shape Get (int shapeId = 0, int materialId = 0) {
		Shape instance;
		if (recycle) {
			if (pools == null) {
				CreatePools();
            }    //checks the list indext for shape pool
            List<Shape> pool = pools[shapeId];
			int lastIndex = pool.Count - 1;
			if (lastIndex >= 0) {
				instance = pool[lastIndex];
				instance.gameObject.SetActive(true);
				pool.RemoveAt(lastIndex);  //checks the pools index and instanced//creats a game object and removes the index
            }
			else {
				instance = Instantiate(prefabs[shapeId]);
				instance.ShapeId = shapeId;
			}
        } //checks shape id,  and creates a gamebojev for the poolScence
        else {
			instance = Instantiate(prefabs[shapeId]);
			instance.ShapeId = shapeId;
		}

		instance.SetMaterial(materials[materialId], materialId);
		return instance;
	}
    //sets_random_range
    public Shape GetRandom () {
		return Get(
			Random.Range(0, prefabs.Length),
			Random.Range(0, materials.Length)
		);
	}
    //when pools are to be created and destoryed
	public void Reclaim (Shape shapeToRecycle) {
		if (recycle) {
			if (pools == null) {
				CreatePools();
			}
			pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
			shapeToRecycle.gameObject.SetActive(false);
		}
		else {
			Destroy(shapeToRecycle.gameObject);
		}
	}
    //creates pools, adds a prefab in the list and pool are a part of the list
    void CreatePools () {
		pools = new List<Shape>[prefabs.Length];
		for (int i = 0; i < pools.Length; i++) {
			pools[i] = new List<Shape>();
		}
	}
}