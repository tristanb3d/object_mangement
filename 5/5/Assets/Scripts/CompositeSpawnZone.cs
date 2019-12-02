using UnityEngine;

public class CompositeSpawnZone : SpawnZone {

	[SerializeField]
	SpawnZone[] spawnZones;
    //creats an array for spawn zone
	public override Vector3 SpawnPoint {
		get {
			int index = Random.Range(0, spawnZones.Length); //gets a random number between 0 and teh spanzone lenth and return the spawqn zone index
			return spawnZones[index].SpawnPoint;
		}
	}
}