using System.Collections;
using CreativeWarlock.CubeCollisionECS.EntityViews;
using Svelto.Context;
using Svelto.ECS;
using Svelto.Tasks;
using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS.Engines
{
	class CubesEngine : IQueryingEntityViewEngine, IWaitForFrameworkDestruction
	{
//#if TURBO_EXAMPLE
//        SyncRunner _syncRunner;
//#else
		CubeEnumerator _cubeEnumerator;
//#endif

		static int _totalCount;

//#if TURBO_EXAMPLE
//        MultiThreadedParallelTaskCollection _multiParallelTask;       
//#endif
		//PrintTimeEntityView _printEntityView;

		public IEntityViewsDB entityViewsDB { get; set; }

#if TURBO_EXAMPLE
        public const uint NUM_OF_THREADS = 8; //must be divisible by 4 for this exercise as I am not handling reminders
#endif

		public void OnFrameworkDestroyed()
		{
#if TURBO_EXAMPLE && UNITY_EDITOR
//not needed on an actual client, but unity doesn't stop other threads when the execution is stopped
            _multiParallelTask.ClearAndKill();
#endif
		}

		public void Ready()
		{
			TaskRunner.Instance.Run(WaitForEntityViewsAdded());
		}

		IEnumerator WaitForEntityViewsAdded()
		{  
			int count = 0;

            CubeEntityView[] _entityViews;

			do {
                _entityViews = entityViewsDB.QueryEntityViewsAsArray<CubeEntityView>(out count);

				yield return null;
			} while (count == 0);

#if TURBO_EXAMPLE
            int numberOfThreads = (int)Mathf.Min(NUM_OF_THREADS, count);

            var countn = count / numberOfThreads;

            _multiParallelTask = new MultiThreadedParallelTaskCollection(numberOfThreads, false);
            _syncRunner = new SyncRunner(true);

            for (int i = 0; i < numberOfThreads; i++)
                _multiParallelTask.Add(new CubeEnumerator(_entityViews, countn * i, countn));
#else
			_cubeEnumerator = new CubeEnumerator(_entityViews, 0, count);
#endif
			Update().ThreadSafeRunOnSchedule(StandardSchedulers.updateScheduler);
		}

		IEnumerator Update()
		{
			while (true)
			{
#if TURBO_EXAMPLE
                yield return _multiParallelTask.ThreadSafeRunOnSchedule(_syncRunner);
#else
				yield return _cubeEnumerator.ThreadSafeRunOnSchedule(StandardSchedulers.syncScheduler);
#endif
				yield return null;
			}
		}

		class CubeEnumerator : IEnumerator
		{
			private int _countn;
			private int _start;

			CubeEntityView[] _entityViews;

			float MinXYSpeedPerSecond = 1f; // TODO: make it changeable via user input!
			float MaxXYSpeedPerSecond = 10f; // TODO: make it changeable via user input!

			float steerOldDirection = 0.5f; // TODO: make it changeable via user input!
			bool blockCollisions = true;  // TODO: make it changeable via user input!

			Vector3 randomDirection = Vector3.zero;

			public object Current { get { return null; } }
			public CubeEnumerator(CubeEntityView[] entityViews, int start, int countn)
			{
				_entityViews = entityViews;
				_start = start;
				_countn = countn;
			}

			public bool MoveNext()
			{
				var entities = _entityViews;

				Vector3 realTarget = new Vector3();
				realTarget.Set(1, 2, 3);

				var count = _start + _countn;
				var totalCount = _countn;
				for (int index = _start; index < count; index++)
				{
					MoveCube(entities[index]);
				}
//#if TURBO_EXAMPLE
//                System.Threading.Interlocked.Add(ref _totalCount, totalCount);
//#else
				_totalCount += totalCount;
//#endif
				return false;
			}

			void MoveCube(CubeEntityView cubeEntityView)
			{
				if (true) //MoveHorizontalAndVertical)
				{
					float randomDist = Random.Range(-1f, 1f);

					if (Random.Range(0, 2) < 1)
						randomDirection = new Vector3(randomDist, 0f, 0f);
					else
						randomDirection = new Vector3(0f, 0f, randomDist);
				}
				else
					randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

				cubeEntityView.Direction = (1 - steerOldDirection) * randomDirection + steerOldDirection * cubeEntityView.PreviousDirection;
				cubeEntityView.PreviousDirection = cubeEntityView.Direction;

				Vector3 _translationThisFrame = cubeEntityView.Velocity * cubeEntityView.Direction * Time.deltaTime;

				GameObject collidee;
				bool _willCollide = WouldMovingCubeCollide(cubeEntityView, _translationThisFrame, out collidee);

				if (_willCollide && blockCollisions)
				{
					//Debug.Log( "Cube " + cube + " (bounds.extends = " + cube.GetComponent<BoxCollider>().bounds.extents + ") WOULD HAVE hit (but I prevented it) " + collidee + " alng translation = " + _translationThisFrame );
					Debug.DrawRay(cubeEntityView.Transform.position + Vector3.up, _translationThisFrame, Color.green);

					//_translationThisFrame *= -1;
					//v.PreviousDirection = -1 * v.Direction;

					//cubeGO.GetComponent<Colorizer>().OnCollisionWasPrevented();
				}
				else
					cubeEntityView.Transform.position += _translationThisFrame;

				cubeEntityView.Transform.position = CheckMoveLimits(cubeEntityView.Transform.position);
			}

			Vector3 CheckMoveLimits(Vector3 position)
			{
				Vector3 pos = position;

				if (pos.x > CreateScene.HalfPlaneWidth)
					pos.x -= CreateScene.PlaneWidth;
				else if (pos.x < -CreateScene.HalfPlaneWidth)
					pos.x += CreateScene.PlaneWidth;

				if (pos.z > CreateScene.HalfPlaneWidth)
					pos.z -= CreateScene.PlaneWidth;
				else if (pos.z < -CreateScene.HalfPlaneWidth)
					pos.z += CreateScene.PlaneWidth;

				return pos;
			}

			bool WouldMovingCubeCollide(CubeEntityView cubeEntityView, Vector3 translation, out GameObject objectCollidedWith)
			{
				//CubeVelocity v = cube.GetComponent<CubeVelocity>();

				RaycastHit hit;
				if (blockCollisions && Physics.BoxCast(cubeEntityView.Transform.position,
														cubeEntityView.BoxCollider.bounds.extents,
														translation,
														out hit,
														Quaternion.identity,
														1.1f * translation.magnitude)) // check 10% ahead because Unity floats are rather inaccurate
				{
					objectCollidedWith = hit.collider.gameObject;
					return true;
				}

				objectCollidedWith = null;
				return false;
			}

			public void Reset()
			{ }
		}
	}
}