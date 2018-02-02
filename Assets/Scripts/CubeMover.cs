using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
	public float MinXYSpeedPerSecond = 1f;
	public float MaxXYSpeedPerSecond = 10f;
	public int SkipFrames = 60;
	protected int _framesSkippedSoFar = 0;

	[Range(0f, 1f)]
	public float SteerOldDirection = 0.5f;

	public bool blockCollisions = true;

	public bool MoveHorizontalAndVertical = true;

	List<GameObject> _internalCubes;
	CubeVelocity velocity;
	Vector3 randomDirection = Vector3.zero;

	public void SetInternalCubes(List<GameObject> cs)
	{
		_internalCubes = cs;

		foreach (GameObject cube in _internalCubes)
		{
			cube.tag = "WanderingCube";

			cube.AddComponent<CubeVelocity>();
			velocity = cube.GetComponent<CubeVelocity>();
			velocity.Velocity = Random.Range(MinXYSpeedPerSecond, MaxXYSpeedPerSecond);

			cube.AddComponent<Colorizer>();

			BoxCollider col = cube.GetComponent<BoxCollider>();

			if (col == null)
				col = cube.AddComponent<BoxCollider>();

			col.isTrigger = true;

			Rigidbody rb = cube.GetComponent<Rigidbody>();
			if (rb == null)
				rb = cube.AddComponent<Rigidbody>(); // needed to receive OnTrigger callbacks

			rb.useGravity = false;
		}
	}

	public void Update()
	{
		if (_framesSkippedSoFar >= SkipFrames)
		{
			_framesSkippedSoFar = 0;
			MoveCubes();
		}
		else
			_framesSkippedSoFar++;
	}

	void MoveCubes()
	{
		if (_internalCubes == null)
			return;

		foreach (GameObject cube in _internalCubes)
		{
			velocity = cube.GetComponent<CubeVelocity>();

			if (MoveHorizontalAndVertical)
			{
				float randomDist = Random.Range(-1f, 1f);

				if (Random.Range(0, 2) < 1)
					randomDirection = new Vector3(randomDist, 0f, 0f);
				else
					randomDirection = new Vector3(0f, 0f, randomDist);
			}
			else
				randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

			velocity.Direction = (1 - SteerOldDirection) * randomDirection + SteerOldDirection * velocity.PreviousDirection;
			velocity.PreviousDirection = velocity.Direction;

			Vector3 _translationThisFrame = velocity.Velocity * velocity.Direction * Time.deltaTime;

			GameObject collidee;
			bool _willCollide = WouldMovingCubeCollide(cube, _translationThisFrame, out collidee);

			if (_willCollide && blockCollisions)
			{
				//Debug.Log( "Cube " + cube + " (bounds.extends = " + cube.GetComponent<BoxCollider>().bounds.extents + ") WOULD HAVE hit (but I prevented it) " + collidee + " alng translation = " + _translationThisFrame );
				Debug.DrawRay(cube.transform.position + Vector3.up, _translationThisFrame, Color.green);

				//_translationThisFrame *= -1;
				//v.PreviousDirection = -1 * v.Direction;

				cube.GetComponent<Colorizer>().OnCollisionWasPrevented();
			}
			else
				cube.transform.position += _translationThisFrame;

			cube.transform.position = CheckMoveLimits(cube.transform.position);
		}
	}

	Vector3 CheckMoveLimits(Vector3 position)
	{
		Vector3 pos = position;

		if (pos.x > CreateScene.HalfPlaneWidth)
			pos.x -= CreateScene.PlaneWidth;
		else if (pos.x < -CreateScene.HalfPlaneWidth)
			pos.x += CreateScene.PlaneWidth;

		if (pos.z > CreateScene.HalfPlaneWidth)
			pos.z -= CreateScene.PlaneWidth;
		else if (pos.z < -CreateScene.HalfPlaneWidth)
			pos.z += CreateScene.PlaneWidth;

		return pos;
	}

	bool WouldMovingCubeCollide(GameObject cube, Vector3 translation, out GameObject objectCollidedWith)
	{
		//CubeVelocity v = cube.GetComponent<CubeVelocity>();

		RaycastHit hit;
		if (blockCollisions && Physics.BoxCast(cube.transform.position, 
												cube.GetComponent<BoxCollider>().bounds.extents,
												translation,
												out hit,
												Quaternion.identity, 
												1.1f * translation.magnitude)) // check 10% ahead because Unity floats are rather inaccurate
		{
			objectCollidedWith = hit.collider.gameObject;
			return true;
		}

		objectCollidedWith = null;
		return false;
	}
}