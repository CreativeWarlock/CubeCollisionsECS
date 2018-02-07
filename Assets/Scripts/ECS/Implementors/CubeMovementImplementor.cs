using CreativeWarlock.CubeCollisionECS.Components;
using CreativeWarlock.CubeCollisionECS.EntityViews;
using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS.Implementors
{
	public class CubeMovementImplementor : MonoBehaviour, IImplementor, ICubeMovementComponent
	{
		CubeEntityView cubeEntityView;

		float MinXYSpeedPerSecond = 1f; // TODO: make it changeable via user input!
		float MaxXYSpeedPerSecond = 10f; // TODO: make it changeable via user input!

		float steerOldDirection = 0.5f; // TODO: make it changeable via user input!
		bool blockCollisions = true;  // TODO: make it changeable via user input!

		public BoxCollider BoxCollider { get { return cubeEntityView.BoxCollider; } }

		public Vector3 Direction {
			get { return cubeEntityView.Direction; }
			set { cubeEntityView.Direction = value; }
		}

		public Vector3 PreviousDirection {
			get { return cubeEntityView.PreviousDirection; }
			set { cubeEntityView.PreviousDirection = value; }
		}

		public Transform Transform {
			get { return cubeEntityView.Transform; }
			set { cubeEntityView.Transform = value; }
		}
		
		public float Velocity {
			get { return cubeEntityView.Velocity; }
			set { cubeEntityView.Velocity = value; }
		}

		void Awake()
		{
			cubeEntityView.BoxCollider = GetComponent<BoxCollider>();
			cubeEntityView.Transform = transform;
		}
	}
}