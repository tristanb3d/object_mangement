using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour {

	string savePath;

	void Awake () {
		savePath = Path.Combine(Application.persistentDataPath, "saveFile");
	}
    //saves data via the writer to the savepath
    public void Save (PersistableObject o, int version) {
		using (
			var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
		) {
			writer.Write(-version);
			o.Save(new GameDataWriter(writer));
		}
	}
    //load persiatble objects from binary and game data reader opens throught eh savepath
    public void Load (PersistableObject o) {
		using (
			var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
		) {
			o.Load(new GameDataReader(reader, -reader.ReadInt32()));
		}
	}
}
