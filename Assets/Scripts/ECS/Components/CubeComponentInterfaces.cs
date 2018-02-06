using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS.Components
{
	public interface ICubeMovementComponent : IComponent
	{
		BoxCollider BoxCollider { get; }

		Vector3 Direction { get; set; }
		Vector3 PreviousDirection { get; set; }

		Transform Transform { get; set; }

		float Velocity { get; set; }
	}
}