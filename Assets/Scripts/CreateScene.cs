using UnityEngine;

public class CreateScene : MonoBehaviour
{
	public int _planeWidth = 50;
	public static float PlaneWidth = 50f;
	public static float HalfPlaneWidth = 10f;

	public float PlaneOffset = 10f;

	public void Awake()
	{
		CreatePlane();
		CreateAndPositionCam();
	}

	void CreatePlane()
	{
		PlaneWidth = _planeWidth;
		HalfPlaneWidth = 0.5f * _planeWidth;
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
		plane.name = "plane";
		plane.transform.localScale = new Vector3(PlaneWidth + PlaneOffset, 1f, PlaneWidth + PlaneOffset);
		plane.transform.position = new Vector3(0f, -1f, 0f);
		plane.transform.SetParent(gameObject.transform, true);
	}

	void CreateAndPositionCam()
	{
		Camera cam = Camera.main;
		if (cam == null)
		{
			GameObject _goCam = new GameObject("Camera");
			cam = _goCam.AddComponent<Camera>();
		}
		cam.transform.position = new Vector3(0f, 50f, 0f);
		cam.transform.LookAt(Vector3.zero);
	}
}