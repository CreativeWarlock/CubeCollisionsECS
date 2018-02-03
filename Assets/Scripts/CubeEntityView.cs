using Svelto.ECS;
using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS
{
	public struct CubeEntityView : IEntityStruct
	{
		public Vector3 position;

		public int ID { get; set; }
	}
}