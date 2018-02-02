using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

namespace CreativeWarlock.CubeCollisionECS
{
	public class PrintTime : MonoBehaviour
	{
#if !UNITY_EDITOR
		Stopwatch stopWatch = new Stopwatch();
#endif
		Text text;

		int _currentTime;
		int _frames;

		void Start()
		{
			text = GetComponent<Text>();
#if UNITY_EDITOR
			text.text = "this value has a different meaning in the Editor, look at the stats or profiler window instead";
			text.alignment = TextAnchor.MiddleLeft;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
#else
			stopWatch.Start();
#endif
		}

#if !UNITY_EDITOR
		void Update()
		{
			_currentTime = stopWatch.Elapsed.Milliseconds;
			_frames++;
			if (_currentTime > 100)
			{
				text.text = (_currentTime / _frames).ToString("N6");
				_currentTime = 0;
				_frames = 0;
				stopWatch.Reset();
				stopWatch.Start();
			}
		}
#endif
	}
}
