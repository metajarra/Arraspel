using Godot;

public struct EventParams
{
	// Primitives
	public string[] stringParam;
	public bool[] boolParam;
	public int[] intParam;
	public float[] floatParam;

	// Godot
	public Vector2[] vector2Param;
	public Vector3[] vector3Param;
	public Transform[] transformParam;
	public Node[] nodeParam;
	
	//  Custom
}
