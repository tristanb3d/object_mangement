using System.Collections.Generic;
using UnityEngine;

public class Game : PersistableObject {

	public PersistableObject prefab;
    //setting key codes and theprepab
	public KeyCode makeKey = KeyCode.C;
	public KeyCode newGameKey = KeyCode.N;
	public KeyCode saveKey = KeyCode.S;
	public KeyCode loadKey = KeyCode.L;

	public PersistentStorage storage;
    //setting storage 
	List<PersistableObject> objects;

	void Awake () {
		objects = new List<PersistableObject>();
	}

    //check for key inputs
	void Update () {
		if (Input.GetKeyDown(makeKey)) {
			CreateObject();
		}
		else if (Input.GetKeyDown(newGameKey)) {
			BeginNewGame();
		}
		else if (Input.GetKeyDown(saveKey)) {
			storage.Save(this);
		}
		else if (Input.GetKeyDown(loadKey)) {
			BeginNewGame();
			storage.Load(this);
		}
	}
    //start game and clear the list
    void BeginNewGame () {
		for (int i = 0; i < objects.Count; i++) {
			Destroy(objects[i].gameObject);
		} //destroy
        objects.Clear();
        //clear list
	}


    // createing object, setting random positions and rotations
	void CreateObject () {
		PersistableObject o = Instantiate(prefab);
		Transform t = o.transform;
		t.localPosition = Random.insideUnitSphere * 5f;
		t.localRotation = Random.rotation;
		t.localScale = Vector3.one * Random.Range(0.1f, 1f);
		objects.Add(o);
	}

    //save
	public override void Save (GameDataWriter writer) {
		writer.Write(objects.Count);
		for (int i = 0; i < objects.Count; i++) {
			objects[i].Save(writer);
		}
	}
    //load
	public override void Load (GameDataReader reader) {
		int count = reader.ReadInt();
		for (int i = 0; i < count; i++) {
			PersistableObject o = Instantiate(prefab);
			o.Load(reader);
			objects.Add(o);
		}
	}
}