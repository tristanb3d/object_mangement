using System.Collections.Generic;
using UnityEngine;

public class Game : PersistableObject {

	const int saveVersion = 1;

	public ShapeFactory shapeFactory;
    //asigning keycode values,
 	public KeyCode createKey = KeyCode.C;
    public KeyCode destroyKey = KeyCode.X;
	public KeyCode newGameKey = KeyCode.N;
	public KeyCode saveKey = KeyCode.S;
	public KeyCode loadKey = KeyCode.L;

	public PersistentStorage storage;

	public float CreationSpeed { get; set; } //get and set cration/destruction speeds

	public float DestructionSpeed { get; set; }

	List<Shape> shapes;

	float creationProgress, destructionProgress;

	void Awake () {
		shapes = new List<Shape>(); //list shapes
	}
    //checking key inputs and accesing there funtions e.g create shape
    void Update () {
		if (Input.GetKeyDown(createKey)) {
			CreateShape();
		}
		else if (Input.GetKeyDown(destroyKey)) { //destroy eky destroyds the shae 
			DestroyShape();
		}
		else if (Input.GetKeyDown(newGameKey)) { // new Game key (n)  starts a new game, acces the funtion to begin new game
			BeginNewGame();
		}
		else if (Input.GetKeyDown(saveKey)) { //save key saves data as a version
			storage.Save(this, saveVersion);
		}
		else if (Input.GetKeyDown(loadKey)) { //load key starts new game and laods data
			BeginNewGame();
			storage.Load(this);
		}

		creationProgress += Time.deltaTime * CreationSpeed;
		while (creationProgress >= 1f) {
			creationProgress -= 1f;
			CreateShape();
		}

		destructionProgress += Time.deltaTime * DestructionSpeed;
		while (destructionProgress >= 1f) {
			destructionProgress -= 1f;
			DestroyShape();
		}
	}

	void BeginNewGame () {
		for (int i = 0; i < shapes.Count; i++) {
			//Destroy(shapes[i].gameObject);
			shapeFactory.Reclaim(shapes[i]);
		}
		shapes.Clear();
	}

    void CreateShape()
    {
        Shape instance = shapeFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = Random.insideUnitSphere * 5f; //random shape position
        t.localRotation = Random.rotation; //random sahpe roation
        t.localScale = Vector3.one * Random.Range(0.1f, 1f); //random scale
        instance.SetColor(Random.ColorHSV( //random colour 
            hueMin: 0f, hueMax: 1f,
            saturationMin: 0.5f, saturationMax: 1f,
            valueMin: 0.25f, valueMax: 1f,
            alphaMin: 1f, alphaMax: 1f
        )); //serring range for the hue/saturation, aplha and value minimum and maximums
        shapes.Add(instance);
    }
    //saving the shape id, material id and saves to the writer
    //destroys all objects, clears stage and starts a new game
    void DestroyShape () {
		if (shapes.Count > 0) {
			int index = Random.Range(0, shapes.Count);
			shapeFactory.Reclaim(shapes[index]);
			int lastIndex = shapes.Count - 1;
			shapes[index] = shapes[lastIndex];
			shapes.RemoveAt(lastIndex);
		}
	}
    //saving the shape id, material id and saves to the writer	
    public override void Save (GameDataWriter writer) {
    writer.Write(shapes.Count);
		for (int i = 0; i < shapes.Count; i++) {
			writer.Write(shapes[i].ShapeId);
			writer.Write(shapes[i].MaterialId);
			shapes[i].Save(writer);
		}
	}
 //loads the save version from the game data readder
	public override void Load (GameDataReader reader) {
		int version = reader.Version;
		if (version > saveVersion) {
			Debug.LogError("Unsupported future save version " + version);
			return;
		}
		int count = version <= 0 ? -version : reader.ReadInt();
		for (int i = 0; i < count; i++) {
			int shapeId = version > 0 ? reader.ReadInt() : 0;
			int materialId = version > 0 ? reader.ReadInt() : 0;
			Shape instance = shapeFactory.Get(shapeId, materialId);
			instance.Load(reader);
			shapes.Add(instance);
		}
	}
}