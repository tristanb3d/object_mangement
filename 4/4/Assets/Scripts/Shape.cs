using UnityEngine;

public class Shape : PersistableObject
{
    //shape class that inherits from perst objs
    static int colorPropertyId = Shader.PropertyToID("_Color");

    static MaterialPropertyBlock sharedPropertyBlock;
    //mat propertys
    public int MaterialId { get; private set; }

    public int ShapeId
    {
        get
        {
            return shapeId;
        }
        set
        {
            if (shapeId == int.MinValue && value != int.MinValue)
            {
                shapeId = value;
            } //value can only get set once
            else
            {
                Debug.LogError("Not allowed to change ShapeId."); //value can o
            } //get and set th shape id and a value
        }
    }

    int shapeId = int.MinValue;

    Color color;

    MeshRenderer meshRenderer;
    //give mesh and colour names, and the shape id a value

    void Awake()
    { //upon startup meshrend - the meshREnder attached
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color)
    {
        this.color = color; //setting colour 
        if (sharedPropertyBlock == null)
        {//checking if has a color
         //ig nto adda new material to the shape
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
        //setting color to shape 
    }

    public void SetMaterial(Material material, int materialId)
    {
        meshRenderer.material = material;
        MaterialId = materialId;
    }

    //saving game data to writer
    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(color);
    }
    //loading game data via reader
    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
    }
}