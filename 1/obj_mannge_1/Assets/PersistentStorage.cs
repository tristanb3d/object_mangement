using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour {
    //declare savepath info
	string savePath;

	void Awake () {
		savePath = Path.Combine(Application.persistentDataPath, "saveFile");
	}//defininf what save path dose, saves as saveFile 

    public void Save(PersistableObject o) {
        using (
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
            //writes the into the save path/ to saveFile
        ) {
            o.Save(new GameDataWriter(writer));
        }
    }

    //loads
	public void Load (PersistableObject o) {
		using (
			var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
            //loads from the save path
            //and opens the data
		) {
			o.Load(new GameDataReader(reader));
		}
	}
}
