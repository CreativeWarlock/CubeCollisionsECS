using CreativeWarlock.CubeCollisionECS.Components;
using CreativeWarlock.CubeCollisionECS.Descriptors;
using CreativeWarlock.CubeCollisionECS.Implementors;
using Svelto.ECS;
using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS.Engines
{
	public class CubeSpawnerEngine : IEngine
	{
		IEntityFactory _entityFactory;

		int _numberOfCubesToSpawn = 1024;   // TODO: Let the user change this parameter
		float _cubeScale = 0.5f;			// TODO: Let the user change this parameter

		public CubeSpawnerEngine(IEntityFactory entityFactory)
		{
			_entityFactory = entityFactory;

			CreateCubes();
		}

		void CreateCubes()
		{
			// with a GO holder for our cubes the FPS are higher!
			GameObject _cubeHolder = new GameObject("Cubes-Holder");

			for (int i = 0; i < _numberOfCubesToSpawn; i++)
			{
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.name = "Cube-" + i;
				cube.tag = "WanderingCube"; //used for physics layer

				cube.AddComponent<CubeMovementImplementor>();

				cube.AddComponent<Colorizer>();

				BoxCollider col = cube.GetComponent<BoxCollider>();

				if (col == null)
					col = cube.AddComponent<BoxCollider>();

				col.isTrigger = true;

				Rigidbody rb = cube.GetComponent<Rigidbody>();
				if (rb == null)
					rb = cube.AddComponent<Rigidbody>(); // needed to receive OnTrigger callbacks

				rb.useGravity = false;

				cube.transform.localScale = new Vector3(_cubeScale, _cubeScale, _cubeScale);
				cube.transform.position = new Vector3(
					Random.Range(-CreateScene.HalfPlaneWidth, CreateScene.HalfPlaneWidth),
									.1f,
									Random.Range(-CreateScene.HalfPlaneWidth, CreateScene.HalfPlaneWidth));
				cube.transform.SetParent(_cubeHolder.transform, true);

				_entityFactory.BuildEntity<CubeEntityDescriptor>(cube.GetInstanceID(),
					cube.GetComponentsInChildren<IComponent>());
			}
		}
	}
}