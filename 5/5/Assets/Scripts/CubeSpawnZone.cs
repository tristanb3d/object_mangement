using UnityEngine;

public class CubeSpawnZone : SpawnZone {

	[SerializeField]
	bool surfaceOnly;

	public override Vector3 SpawnPoint {
		get {  //here generate a rnadom positon, using a vector 3 and getting a random range 
            //between -0.5 and 0.5 pro the xyz position 
            //and add a random range for the azis and return the transform 
            //objects will spawn inside the space of thee wire cube
			Vector3 p;
			p.x = Random.Range(-0.5f, 0.5f);
			p.y = Random.Range(-0.5f, 0.5f);
			p.z = Random.Range(-0.5f, 0.5f);
			if (surfaceOnly) {
				int axis = Random.Range(0, 3);
				p[axis] = p[axis] < 0f ? -0.5f : 0.5f;
			}
			return transform.TransformPoint(p);
		}
	}

	void OnDrawGizmos () { //draw gizmos give them colour, a matrix and draw a wireframe cube object
		Gizmos.color = Color.cyan;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}
}