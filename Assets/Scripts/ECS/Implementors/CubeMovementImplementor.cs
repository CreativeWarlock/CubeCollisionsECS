using System;
using CreativeWarlock.CubeCollisionECS.Components;
using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS.Implementors
{
	public class CubeMovementImplementor : MonoBehaviour, IImplementor, ICubeMovementComponent
	{
		BoxCollider _boxCollider;
		Vector3 _direction = Vector3.zero;
		Vector3 _prevDirection = Vector3.zero;
		Transform _transform;
		public float _velocity = 1f;

		float MinXYSpeedPerSecond = 1f; // TODO: make it changeable via user input!
		float MaxXYSpeedPerSecond = 10f; // TODO: make it changeable via user input!

		float steerOldDirection = 0.5f; // TODO: make it changeable via user input!
		bool blockCollisions = true;  // TODO: make it changeable via user input!

		public BoxCollider BoxCollider { get { return _boxCollider; } }

		public Vector3 Direction {
			get { return _direction; }
			set { _direction = value; }
		}

		public Vector3 PreviousDirection {
			get { return _prevDirection; }
			set { _prevDirection = value; }
		}

		public Transform Transform {
			get { return _transform; }
			set { _transform = value; }
		}
		
		public float Velocity {
			get { return _velocity; }
			set { _velocity = value; }
		}

		void Awake()
		{
			_boxCollider = GetComponent<BoxCollider>();
			_transform = transform;
		}
	}
}