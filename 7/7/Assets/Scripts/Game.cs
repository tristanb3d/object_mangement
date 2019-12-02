using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Game : PersistableObject
{

    const int saveVersion = 4;
    //asigning keycode values,
    [SerializeField] ShapeFactory shapeFactory;

    [SerializeField] KeyCode createKey = KeyCode.C;
    [SerializeField] KeyCode destroyKey = KeyCode.X;
    [SerializeField] KeyCode newGameKey = KeyCode.N;
    [SerializeField] KeyCode saveKey = KeyCode.S;
    [SerializeField] KeyCode loadKey = KeyCode.L;

    [SerializeField] PersistentStorage storage;

    [SerializeField] int levelCount;

    [SerializeField] bool reseedOnLoad;

    [SerializeField] Slider creationSpeedSlider;
    [SerializeField] Slider destructionSpeedSlider;

    public float CreationSpeed { get; set; } //get and set cration/destruction speeds

    public float DestructionSpeed { get; set; }

    List<Shape> shapes;

    float creationProgress, destructionProgress;

    int loadedLevelBuildIndex;

    Random.State mainRandomState;

    void Start()
    {
        mainRandomState = Random.state;
        shapes = new List<Shape>();

        if (Application.isEditor)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name.Contains("Level "))
                {
                    SceneManager.SetActiveScene(loadedScene);
                    loadedLevelBuildIndex = loadedScene.buildIndex;
                    return;
                }
            }
        }

        BeginNewGame();
        StartCoroutine(LoadLevel(1));
    }
    //checking key inputs and accesing there funtions e.g create shape
    void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateShape();
        } //create shape
        else if (Input.GetKeyDown(destroyKey))
        {
            DestroyShape();
        }  //destroy eky destroyds the shae
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
            StartCoroutine(LoadLevel(loadedLevelBuildIndex));
        }// new Game key (n)  starts a new game, acces the funtion to begin new game
        else if (Input.GetKeyDown(saveKey))
        {
            storage.Save(this, saveVersion);
        } //save key saves data as a version
        else if (Input.GetKeyDown(loadKey))
        { //load key starts new game and laods data
            BeginNewGame();
            storage.Load(this);
        }
        else
        {
            for (int i = 1; i <= levelCount; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    BeginNewGame();
                    StartCoroutine(LoadLevel(i));
                    return;
                }
            }
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
            shapes[i].GameUpdate();
        }

        creationProgress += Time.deltaTime * CreationSpeed;
        while (creationProgress >= 1f)
        {
            creationProgress -= 1f;
            CreateShape();  //create shape
        }

        destructionProgress += Time.deltaTime * DestructionSpeed;
        while (destructionProgress >= 1f)
        {
            destructionProgress -= 1f;
            DestroyShape();  //Destroy(shapes[i].gameObject);
        }
    }

    void BeginNewGame()
    {
        Random.state = mainRandomState;
        int seed = Random.Range(0, int.MaxValue) ^ (int)Time.unscaledTime;
        mainRandomState = Random.state;
        Random.InitState(seed);

        creationSpeedSlider.value = CreationSpeed = 0;
        destructionSpeedSlider.value = DestructionSpeed = 0;

        for (int i = 0; i < shapes.Count; i++)
        {
            shapeFactory.Reclaim(shapes[i]);
        }
        shapes.Clear();
    }

    IEnumerator LoadLevel(int levelBuildIndex)
    {
        enabled = false;
        if (loadedLevelBuildIndex > 0)
        {
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
        GameLevel.Current.ConfigureSpawn(instance);
        shapes.Add(instance);
    }

    void DestroyShape()
    {
        if (shapes.Count > 0)
        {
            int index = Random.Range(0, shapes.Count);
            shapeFactory.Reclaim(shapes[index]);
            int lastIndex = shapes.Count - 1;
            shapes[index] = shapes[lastIndex];
            shapes.RemoveAt(lastIndex);
        }
    }
    //data tp save
    public override void Save(GameDataWriter writer)
    {
        writer.Write(shapes.Count); //save shape
        writer.Write(Random.state); //save random state
        writer.Write(CreationSpeed);//save cration speed
        writer.Write(creationProgress); //save cration
        writer.Write(DestructionSpeed); //save destuction speed
        writer.Write(destructionProgress); //save destruction progress
        writer.Write(loadedLevelBuildIndex);
        GameLevel.Current.Save(writer); //save gmae lvl
        for (int i = 0; i < shapes.Count; i++)
        {
            writer.Write(shapes[i].ShapeId);
            writer.Write(shapes[i].MaterialId);
            shapes[i].Save(writer);
        }
    }
    //loads 
    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        if (version > saveVersion)
        {
            Debug.LogError("Unsupported future save version " + version);
            return;
        }
        StartCoroutine(LoadGame(reader));
    }
    //load game data
    IEnumerator LoadGame(GameDataReader reader)
    {
        int version = reader.Version;
        int count = version <= 0 ? -version : reader.ReadInt();

        if (version >= 3)
        {
            Random.State state = reader.ReadRandomState();
            if (!reseedOnLoad)
            {
                Random.state = state;
            }
            creationSpeedSlider.value = CreationSpeed = reader.ReadFloat();
            creationProgress = reader.ReadFloat();
            destructionSpeedSlider.value = DestructionSpeed = reader.ReadFloat();
            destructionProgress = reader.ReadFloat();
        }
        //load ;v;
        yield return LoadLevel(version < 2 ? 1 : reader.ReadInt());
        if (version >= 3)
        {
            GameLevel.Current.Load(reader);
        }

        for (int i = 0; i < count; i++)
        {
            int shapeId = version > 0 ? reader.ReadInt() : 0;
            int materialId = version > 0 ? reader.ReadInt() : 0;
            Shape instance = shapeFactory.Get(shapeId, materialId);
            instance.Load(reader);
            shapes.Add(instance);
        }
    }
}