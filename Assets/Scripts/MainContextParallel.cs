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

            var cubesEngine = new CubesEngine();

			_enginesRoot.AddEngine(cubesEngine);
            _contextNotifier.AddFrameworkDestructionListener(cubesEngine);

			CubeSpawnerEngine cubeSpawnerEngine = new CubeSpawnerEngine(entityFactory);

			_enginesRoot.AddEngine(cubeSpawnerEngine);
		}

		void ICompositionRoot.OnContextInitialized()
		{ }

		void ICompositionRoot.OnContextDestroyed()
		{
			_contextNotifier.NotifyFrameworkDeinitialized();
			TaskRunner.Instance.StopAndCleanupAllDefaultSchedulerTasks();
		}
	}

	public class MainContextParallel : UnityContext<ParallelCompositionRoot>
	{ }
}

