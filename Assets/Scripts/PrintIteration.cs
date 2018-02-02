using UnityEngine;
using UnityEngine.UI;

namespace CreativeWarlock.CubeCollisionECS
{
	public class PrintIteration : MonoBehaviour, IPrintStuffComponent
	{
		int _iterations;
		Text _text;

		public int iterations
		{
			get { return _iterations; }

			set { _iterations = value + 1; _text.text = _iterations.ToString(); }
		}

		void Start()
		{
			_text = GetComponent<Text>();

			_text.alignment = TextAnchor.MiddleLeft;
			_text.horizontalOverflow = HorizontalWrapMode.Overflow;
		}
	}

}