using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Game : PersistableObject {

	const int saveVersion = 2;

	public static Game Instance { get; private set; }

	[SerializeField] ShapeFactory shapeFactory;
    //asigning keycode values,
    [SerializeField] KeyCode createKey = KeyCode.C;
	[SerializeField] KeyCode destroyKey = KeyCode.X;
	[SerializeField] KeyCode newGameKey = KeyCode.N;
	[SerializeField] KeyCode saveKey = KeyCode.S;
	[SerializeField] KeyCode loadKey = KeyCode.L;

	[SerializeField] PersistentStorage storage;

	[SerializeField] int levelCount;

	public SpawnZone SpawnZoneOfLevel { get; set; } 

	public float CreationSpeed { get; set; } //get and set cration/destruction speeds


    public float DestructionSpeed { get; set; }

	List<Shape> shapes;

	float creationProgress, destructionProgress;

	int loadedLevelBuildIndex;

	void OnEnable () {
		Instance = this;
	}

	void Start () {
		Instance = this;
		shapes = new List<Shape>();

		if (Application.isEditor) {
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				Scene loadedScene = SceneManager.GetSceneAt(i);
				if (loadedScene.name.Contains("Level ")) {
					SceneManager.SetActiveScene(loadedScene);
					loadedLevelBuildIndex = loadedScene.buildIndex;
					return;
				}
			}
		}

		StartCoroutine(LoadLevel(1));
	}

    //checking key inputs and accesing there funtions e.g create shape
    void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateShape();
        }
        else if (Input.GetKeyDown(destroyKey))
        { //destroy eky destroyds the shae
            DestroyShape();
        }
        else if (Input.GetKeyDown(newGameKey))
        { // new Game key (n)  starts a new game, acces the funtion to begin new game
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        { //save key saves data as a version
            storage.Save(this, saveVersion);
        }
        else if (Input.GetKeyDown(loadKey))
        { //load key starts new game and laods data
            BeginNewGame();
            storage.Load(this);
        }
        else {
			for (int i = 1; i <= levelCount; i++) {
				if (Input.GetKeyDown(KeyCode.Alpha0 + i)) {
					BeginNewGame();
					StartCoroutine(LoadLevel(i));
					return;
				}
			}
		}

		creationProgress += Time.deltaTime * CreationSpeed;
		while (creationProgress >= 1f) {
			creationProgress -= 1f;
			CreateShape();
		}

		destructionProgress += Time.deltaTime * DestructionSpeed;
		while (destructionProgress >= 1f) {
			destructionProgress -= 1f;
			DestroyShape();   //Destroy(shapes[i].gameObject);
        }
	}

	void BeginNewGame () {
		for (int i = 0; i < shapes.Count; i++) {
			shapeFactory.Reclaim(shapes[i]);
		}
		shapes.Clear();
	}

	IEnumerator LoadLevel (int levelBuildIndex) {
		enabled = false;
		if (loadedLevelBuildIndex > 0) {
			yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);
		}
		yield return SceneManager.LoadSceneAsync(
			levelBuildIndex, LoadSceneMode.Additive
		);
		SceneManager.SetActiveScene(
			SceneManager.GetSceneByBuildIndex(levelBuildIndex)
		);
		loadedLevelBuildIndex = levelBuildIndex; //load lvl from guild index
        enabled = true;
	}

    void CreateShape()
    {
        Shape instance = shapeFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = Random.insideUnitSphere * 5f; //random shape position
        t.localRotation = Random.rotation;  //random sahpe roation
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);  //random scale
        instance.SetColor(Random.ColorHSV(//random colour 
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
		writer.Write(loadedLevelBuildIndex);
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
		StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt()));
		for (int i = 0; i < count; i++) {
			int shapeId = version > 0 ? reader.ReadInt() : 0;
			int materialId = version > 0 ? reader.ReadInt() : 0;
			Shape instance = shapeFactory.Get(shapeId, materialId);
			instance.Load(reader);
			shapes.Add(instance);
		}
	}
}