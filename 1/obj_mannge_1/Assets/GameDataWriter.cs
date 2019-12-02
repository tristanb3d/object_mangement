using System.IO;
using UnityEngine;

public class GameDataWriter {

    BinaryWriter writer;
    //the writer
    //defining the writer
	public GameDataWriter (BinaryWriter writer) {
		this.writer = writer;
	}
    //write value as float
	public void Write (float value) {
		writer.Write(value);
	}
    //write value as it
	public void Write (int value) {
		writer.Write(value);
	}
    //write the rotations
	public void Write (Quaternion value) {
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
		writer.Write(value.w);
	}
    //writes position
	public void Write (Vector3 value) {
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
	}
}