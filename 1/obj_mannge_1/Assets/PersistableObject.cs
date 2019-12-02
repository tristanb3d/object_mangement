using UnityEngine;

[DisallowMultipleComponent]
//sets to only 1 persistable object
public class PersistableObject : MonoBehaviour {
    //save the positon, rotation and scale cords
	public virtual void Save (GameDataWriter writer) {
		writer.Write(transform.localPosition);
		writer.Write(transform.localRotation);
		writer.Write(transform.localScale);
	}
    // reads the saved positions
	public virtual void Load (GameDataReader reader) {
		transform.localPosition = reader.ReadVector3();
		transform.localRotation = reader.ReadQuaternion();
		transform.localScale = reader.ReadVector3();
	}
}