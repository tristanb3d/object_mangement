using UnityEngine;

public class Shape : PersistableObject {
    //shape class that inherits from perst objs
    static int colorPropertyId = Shader.PropertyToID("_Color");

	static MaterialPropertyBlock sharedPropertyBlock;
    //mat propertys
    public int MaterialId { get; private set; }

	public int ShapeId {
		get {
			return shapeId;
		}
		set {
			if (shapeId == int.MinValue && value != int.MinValue) {
				shapeId = value;
            }  //value can only get set once
            else {
				Debug.LogError("Not allowed to change ShapeId.");
            } //get and set th shape id and a value
        }
	}

	public Vector3 AngularVelocity { get; set; }
    //get and set the angular velocity and velocity
	public Vector3 Velocity { get; set; }

	int shapeId = int.MinValue;

	Color color;

	MeshRenderer meshRenderer;
    //give mesh and colour names, and the shape id a value
    void Awake ()
    { //upon startup meshrend - the meshREnder attached
        meshRenderer = GetComponent<MeshRenderer>();
	}

	public void GameUpdate () {
		transform.Rotate(AngularVelocity * Time.deltaTime);
		transform.localPosition += Velocity * Time.deltaTime;
	}
    //setting color to shape 
    public void SetColor (Color color) {
		this.color = color;
		if (sharedPropertyBlock == null) {
			sharedPropertyBlock = new MaterialPropertyBlock();
		}
		sharedPropertyBlock.SetColor(colorPropertyId, color);
		meshRenderer.SetPropertyBlock(sharedPropertyBlock);
	}

	public void SetMaterial (Material material, int materialId) {
		meshRenderer.material = material;
		MaterialId = materialId;
	}
    //saving game data to writer
    public override void Save (GameDataWriter writer) {
		base.Save(writer);
		writer.Write(color);
		writer.Write(AngularVelocity);
		writer.Write(Velocity);
	}
    //loading game data via reader
    public override void Load (GameDataReader reader) {
		base.Load(reader);
		SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
		AngularVelocity =
			reader.Version >= 4 ? reader.ReadVector3() : Vector3.zero;
		Velocity = reader.Version >= 4 ? reader.ReadVector3() : Vector3.zero;
	}
}