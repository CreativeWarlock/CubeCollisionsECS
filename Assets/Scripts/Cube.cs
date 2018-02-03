using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS
{
	public class Cube : MonoBehaviour, ICubeComponent
	{
		Transform _transform;
		Vector3 _direction;
		Vector3 _prevDirection;
		float _velocity;

		void Start()
		{
			_transform = transform;
		}

		public Vector3 Direction
		{
			get { return _direction; }
			set { _direction = value; }
		}

		public Vector3 PreviousDirection
		{
			get { return _prevDirection; }
			set { _prevDirection = value; }
		}

		public Vector3 Position
		{
			get { return _transform.position; }
			set { _transform.position = value; }
		}

		public float Velocity
		{
			get { return _velocity; }
			set { _velocity = value; }
		}
	}
}