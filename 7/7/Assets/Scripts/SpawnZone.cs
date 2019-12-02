using UnityEngine;

public abstract class SpawnZone : PersistableObject {

	public abstract Vector3 SpawnPoint { get; }

	[System.Serializable]
	public struct SpawnConfiguration {
        //enumerator for directions
		public enum MovementDirection {
			Forward,
			Upward,
			Outward,
			Random
		}

		public MovementDirection movementDirection;

		public FloatRange speed;

		public FloatRange angularSpeed;

		public FloatRange scale;

		public ColorRangeHSV color;
	}

	[SerializeField]
	SpawnConfiguration spawnConfig;

	public virtual void ConfigureSpawn (Shape shape) {
		Transform t = shape.transform; //and set the transform rotationa nd spawn values, and coour and velocity
		t.localPosition = SpawnPoint;
		t.localRotation = Random.rotation;
		t.localScale = Vector3.one * spawnConfig.scale.RandomValueInRange;
		shape.SetColor(spawnConfig.color.RandomInRange);
		shape.AngularVelocity =
			Random.onUnitSphere * spawnConfig.angularSpeed.RandomValueInRange;
        // give a firection for up and forward, and a danom direcion and move/normalize the positon
		Vector3 direction;
		switch (spawnConfig.movementDirection) {
			case SpawnConfiguration.MovementDirection.Upward:
				direction = transform.up;
				break;
			case SpawnConfiguration.MovementDirection.Outward:
				direction = (t.localPosition - transform.position).normalized;
				break;
			case SpawnConfiguration.MovementDirection.Random:
				direction = Random.onUnitSphere;
				break;
			default:
				direction = transform.forward;
				break;
		}
		shape.Velocity = direction * spawnConfig.speed.RandomValueInRange;
	} //gives the shape a velocity and speed based on a random number
}