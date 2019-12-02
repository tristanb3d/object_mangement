using UnityEngine;

[DisallowMultipleComponent]
public class PersistableObject : MonoBehaviour
{
    //saves the position, rotation and scale via the gamedatawriter
    public virtual void Save(GameDataWriter writer)
    {
        writer.Write(transform.localPosition);
        writer.Write(transform.localRotation);
        writer.Write(transform.localScale);
    }
    //  reads and loads data (the position, rotation and scale from the game data reader
    public virtual void Load(GameDataReader reader)
    {
        transform.localPosition = reader.ReadVector3();
        transform.localRotation = reader.ReadQuaternion();
        transform.localScale = reader.ReadVector3();
    }
}