//#define DONT_TRY_THIS_AT_HOME

using Svelto.Context;
using Svelto.ECS;
using UnityEngine;

//Main is the Application Composition Root.
//Composition Root is the place where the framework can be initialised.
namespace CreativeWarlock.CubeCollisionECS
{
	class GUITextEntityDescriptor : GenericEntityDescriptor<PrintTimeEntityView>
	{ }

	//A GameObject containing UnityContext must be present in the scene
	//All the monobehaviours present in the scene statically that need
	//to notify the Context, must belong to GameObjects children of UnityContext.
	public class MainContextParallel : UnityContext<ParallelCompositionRoot>
	{ }

	public class NumberOfEntities
	{
		public const int numberOfEntities = 64; //* 4000
	}

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
            var tasksCount = NumberOfEntities.numberOfEntities;

#if DONT_TRY_THIS_AT_HOME
            for (int i = 0; i < tasksCount; i++)
            {
                GameObject crazyness = new GameObject();
                crazyness.AddComponent<UnityWay>();
            }
#else
			_enginesRoot = new EnginesRoot(new Svelto.ECS.Schedulers.Unity.UnitySumbmissionEntityViewScheduler());
            IEntityFactory entityFactory = _enginesRoot.GenerateEntityFactory();

            var cubesEngine = new CubesEngine();

			_enginesRoot.AddEngine(cubesEngine);
            _contextNotifier.AddFrameworkDestructionListener(cubesEngine);

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

			for (int i = 0; i < tasksCount; i++)
            {
				entityFactory.BuildEntity<CubeEntityDescriptor>(i);
			}

            watch.Stop();
            Utility.Console.Log(watch.ElapsedMilliseconds.ToString());

            entityFactory.BuildEntity<GUITextEntityDescriptor>(0, 
                contextHolder.GetComponentsInChildren<PrintIteration>());
#endif
		}

		void ICompositionRoot.OnContextInitialized()
		{ }

		void ICompositionRoot.OnContextDestroyed()
		{
			_contextNotifier.NotifyFrameworkDeinitialized();
			TaskRunner.Instance.StopAndCleanupAllDefaultSchedulerTasks();
		}
	}
}

