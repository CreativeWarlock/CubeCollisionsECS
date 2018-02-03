using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS
{
	public interface ICubeComponent
	{
		Vector3 Direction { get; set; }
		Vector3 PreviousDirection { get; set; }
		Vector3 Position { get; set; }

		float Velocity { get; set; }
	}
}