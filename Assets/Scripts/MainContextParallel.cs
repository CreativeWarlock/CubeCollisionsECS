using CreativeWarlock.CubeCollisionECS.Engines;
using Svelto.Context;
using Svelto.ECS;
using UnityEngine;

namespace CreativeWarlock.CubeCollisionECS
{
	public class ParallelCompositionRoot : ICompositionRoot
	{
		IContextNotifer _contextNotifier;
		EnginesRoot _enginesRoot;

		public ParallelCompositionRoot()
		{
			QualitySettings.vSyncCount = -1;

			_contextNotifier = new ContextNotifier();
		}

		void ICompositionRoot.OnContextCreated(UnityContext contextHolder)
		{
			_enginesRoot = new EnginesRoot(new Svelto.ECS.Schedulers.Unity.UnitySumbmissionEntityViewScheduler());
            IEntityFactory entityFactory = _enginesRoot.GenerateEntityFactory();

			CubeSpawnerEngine cubeSpawnerEngine = new CubeSpawnerEngine(entityFactory);
			//_contextNotifier.AddFrameworkInitializationListener(cubeSpawnerEngine);
			_enginesRoot.AddEngine(cubeSpawnerEngine);

			var cubesEngine = new CubesEngine();
			_contextNotifier.AddFrameworkDestructionListener(cubesEngine);
			_enginesRoot.AddEngine(cubesEngine);
		}

		void ICompositionRoot.OnContextInitialized()
		{
			Debug.Log("Initializing Context");
		}

		void ICompositionRoot.OnContextDestroyed()
		{
			_contextNotifier.NotifyFrameworkDeinitialized();
			TaskRunner.Instance.StopAndCleanupAllDefaultSchedulerTasks();
		}
	}

	public class MainContextParallel : UnityContext<ParallelCompositionRoot>
	{ }
}

