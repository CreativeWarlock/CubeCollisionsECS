using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS
{
	public interface ICubeComponent
	{
        //Vector3 velocity { get; set; }
		//Vector3 position { get; set; }
		//Quaternion rotation { get; set; }

		Vector3 Direction { get; set; }
		Vector3 PreviousDirection { get; set; }

		float Velocity { get; set; }
	}
}