using System.Collections;
using UnityEngine;

public class Colorizer : MonoBehaviour
{
	public Vector3 maxAbsOfPositions;

	public static Color[] availableColors = new Color[] { Color.grey, Color.blue, Color.cyan, Color.yellow, Color.red };

	public static Material[] availableMaterials;

	public static Material almostCollidedMaterial;
	public static Material errorMaterial;
	public static Material originalMaterial;
//#pragma ignore warning 
	private Coroutine ResetMaterialCoroutine;

	public void Awake()
	{
		SetupMaterials();
		AssignMaterial();
	}

	void SetupMaterials()
	{
		Shader standardShader = Shader.Find("Standard");

		if (standardShader == null)
		{
			Debug.LogError("Could not find standard shader! Aborting SetupMaterials()...");
			return;
		}

		if (availableMaterials == null)
		{
			availableMaterials = new Material[availableColors.Length];

			for (int i = 0; i < availableColors.Length; i++)
			{
				availableMaterials[i] = new Material(standardShader)
				{
					color = availableColors[i]
				};
			}
		}

		if (almostCollidedMaterial == null)
		{
			almostCollidedMaterial = new Material(standardShader)
			{
				color = Color.yellow
			};
		}

		if (errorMaterial == null)
		{
			errorMaterial = new Material(standardShader)
			{
				color = Color.red
			};
		}

		if (originalMaterial == null)
		{
			originalMaterial = new Material(standardShader)
			{
				color = Color.blue
			};
		}
	}

	void AssignMaterial()
	{
		//GetComponent<MeshRenderer>().sharedMaterial = availableMaterials[Random.Range(0, availableMaterials.Length)];
		GetComponent<MeshRenderer>().sharedMaterial = originalMaterial;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (CompareTag(other.gameObject.tag))
		{
			if (ResetMaterialCoroutine != null)
				StopCoroutine(ResetMaterialCoroutine);

			GetComponent<MeshRenderer>().sharedMaterial = errorMaterial;
			ResetMaterialCoroutine = StartCoroutine(ResetMaterial());
		}
	}

	//public void OnTriggerExit(Collider other)
	//{
	//	//StopCoroutine(ResetMaterialCoroutine);
	//	GetComponent<MeshRenderer>().sharedMaterial = originalMaterial;
	//}

	public void OnCollisionWasPrevented()
	{
		if (ResetMaterialCoroutine != null)
			StopCoroutine(ResetMaterialCoroutine);

		GetComponent<MeshRenderer>().sharedMaterial = almostCollidedMaterial;

		ResetMaterialCoroutine = StartCoroutine(ResetMaterial());
	}

	IEnumerator ResetMaterial()
	{
		yield return new WaitForSeconds(0.2f);
		GetComponent<MeshRenderer>().sharedMaterial = originalMaterial;
	}
}