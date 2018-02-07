using CreativeWarlock.CubeCollisionECS.Components;
using Svelto.ECS;
using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS.EntityViews
{
	//public class CubeEntityView :	EntityView
	public struct CubeEntityView : IEntityStruct // How can we employ structs in implementors?
	{
		public BoxCollider BoxCollider;

		public Vector3 Direction;
		public Vector3 PreviousDirection;

		public Transform Transform;

		public float Velocity;

		public int ID { get; set; }

		//public ICubeMovementComponent CubeMovementComponent;
	}
}