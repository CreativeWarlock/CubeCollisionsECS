using System.Collections.Generic;
using UnityEngine;

public class CubeCreator : MonoBehaviour
{
	public int NumberOfCubes = 1000;
	public float CubeScale = 1f;

	public CubeMover cubeMover;

	List<GameObject> _internalCubes = new List<GameObject>();
	GameObject _cubeHolder;

	void OnEnable()
	{
		_cubeHolder = gameObject;
	}

	void Start()
	{
		CreateCubes();

		AssignCubeMover();
	}

	void CreateCubes()
	{
		if (_cubeHolder == null)
			_cubeHolder = new GameObject("Auto-created cubes");

		for (int i = 0; i < NumberOfCubes; i++)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.name = "Cube-" + i;
			cube.transform.localScale = new Vector3(CubeScale, CubeScale, CubeScale);
			cube.transform.position = new Vector3(
				Random.Range(	-CreateScene.HalfPlaneWidth, CreateScene.HalfPlaneWidth),
								.1f,
								Random.Range(-CreateScene.HalfPlaneWidth, CreateScene.HalfPlaneWidth));

			_internalCubes.Add(cube);
			cube.transform.SetParent(_cubeHolder.transform, true);
		}
	}

	void AssignCubeMover()
	{
		if (cubeMover == null)
		{
			GameObject go_cubemover = new GameObject("Cube Mover");
			cubeMover = go_cubemover.AddComponent<CubeMover>();
		}
		cubeMover.SetInternalCubes(_internalCubes);
		_internalCubes = null;
	}
}