using CreativeWarlock.CubeCollisionECS.Components;
using Svelto.ECS;
using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS.EntityViews
{
	public class CubeEntityView :	EntityView
	//public struct CubeEntityView : IEntityStruct // How can we employ structs in implementors?
	{
		//BoxCollider BoxCollider;

		//Vector3 Direction;
		//Vector3 PreviousDirection;

		//Transform Transform;

		//float Velocity;

		public int ID { get; set; }

		public ICubeMovementComponent CubeMovementComponent;
	}
}