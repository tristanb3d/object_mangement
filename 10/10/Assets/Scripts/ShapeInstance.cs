[System.Serializable]
public struct ShapeInstance {
    //shape instace and shave save indecx/ resolve 
	public bool IsValid {
		get {
			return Shape && instanceIdOrSaveIndex == Shape.InstanceId;
		}
	}

	public Shape Shape { get; private set; }

	int instanceIdOrSaveIndex;

	public ShapeInstance (Shape shape) {
		Shape = shape;
		instanceIdOrSaveIndex = shape.InstanceId;
	}

	public ShapeInstance (int saveIndex) {
		Shape = null;
		instanceIdOrSaveIndex = saveIndex;
	}
    //check index if more than 0 shape becomes a instaced of shape
	public void Resolve () {
		if (instanceIdOrSaveIndex >= 0) {
			Shape = Game.Instance.GetShape(instanceIdOrSaveIndex);
			instanceIdOrSaveIndex = Shape.InstanceId;
		}
	}

	public static implicit operator ShapeInstance (Shape shape) {
		return new ShapeInstance(shape);
	}
}