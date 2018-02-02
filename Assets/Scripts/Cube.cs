using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS
{
	public class Cube : ICubeComponent
	{
		public Vector3 velocity { get; set; }
		public Vector3 position { get; set; }
		public Quaternion rotation { get; set; }
	}
}