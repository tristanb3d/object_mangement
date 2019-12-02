using System.IO;
using UnityEngine;

public class GameDataWriter {

	BinaryWriter writer;

	public GameDataWriter (BinaryWriter writer) {
		this.writer = writer; //defining the writer
	}

	public void Write (float value) {
		writer.Write(value);  //write the value into afloat
    }

	public void Write (int value) { //write value into int
		writer.Write(value);
	}

	public void Write (Color value)
    { //writres the rgba colour values
        writer.Write(value.r);
		writer.Write(value.g);
		writer.Write(value.b);
		writer.Write(value.a);
	}

	public void Write (Quaternion value) {  //writing rotation values
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
		writer.Write(value.w);
	}

	public void Write (Vector3 value) {  //writing position values
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
	}
}