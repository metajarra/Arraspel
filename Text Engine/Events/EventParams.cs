using Godot;

// This script handles the storage of different types of parameters for events
// It is intended to be used with event calling in EventManager, but can also be used for other data storage purposes

// Note that when initializing a list with parameter param, the preferred way to do this is:
// stringParam = new string[] { param };
// Though it is also possible to initalize in other ways

// Feel free to add types as needed, but do not remove existing types
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
