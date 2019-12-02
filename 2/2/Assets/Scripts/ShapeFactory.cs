using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject {


	[SerializeField]
	Shape[] prefabs;
    //giving a shapes an array field
	[SerializeField]
	Material[] materials;
    //material array //serized

	public Shape Get (int shapeId = 0, int materialId = 0) {
        //get tjhe shape and give it an ID
        Shape instance = Instantiate(prefabs[shapeId]);
        //define what shapes are made
        instance.ShapeId = shapeId;
        //give shape ID
		instance.SetMaterial(materials[materialId], materialId);
        return instance;
        //used the shape id to ser a matieral
	}

	public Shape GetRandom () {
		return Get(
            //gets random shapes
			Random.Range(0, prefabs.Length),
			Random.Range(0, materials.Length)
		);
	}
}