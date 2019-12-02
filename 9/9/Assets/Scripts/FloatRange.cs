using UnityEngine;

[System.Serializable]
public struct FloatRange {

	public float min, max;
    //set a random range for the float
	public float RandomValueInRange {
		get {
			return Random.Range(min, max);
		}
	}
}