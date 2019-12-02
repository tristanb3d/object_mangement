using System.IO;
using UnityEngine;

public class GameDataReader {

	BinaryReader reader;
    //defining the reader
	public GameDataReader (BinaryReader reader) {
		this.reader = reader;
	}
    //read float
	public float ReadFloat () {
		return reader.ReadSingle();
	}
    //read int
	public int ReadInt () {
		return reader.ReadInt32();
	}
    //read rotation
	public Quaternion ReadQuaternion () {
		Quaternion value;
		value.x = reader.ReadSingle();
		value.y = reader.ReadSingle();
		value.z = reader.ReadSingle();
		value.w = reader.ReadSingle();
		return value;
	}
    //read position
	public Vector3 ReadVector3 () {
		Vector3 value;
		value.x = reader.ReadSingle();
		value.y = reader.ReadSingle();
		value.z = reader.ReadSingle();
		return value;
	}
}