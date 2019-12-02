using System.IO;
using UnityEngine;

public class GameDataWriter {

	BinaryWriter writer;

	public GameDataWriter (BinaryWriter writer) {
		this.writer = writer;//definign writer
	}

	public void Write (float value) {
		writer.Write(value); //write the value
	}

	public void Write (int value) {
		writer.Write(value);
	} 

	public void Write (Color value) {
		writer.Write(value.r);
		writer.Write(value.g);
		writer.Write(value.b);
		writer.Write(value.a);
	} //writres the rgba colour values

	public void Write (Quaternion value) { //writing rotation values
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
		writer.Write(value.w);
	}

	public void Write (Vector3 value) { //writing translation values
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
	}
}