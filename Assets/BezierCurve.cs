using UnityEngine;

public class BezierCurve : MonoBehaviour
{
	public Vector3[] points;
	public int lineSteps = 12;
	public float width;

	public void Reset()
	{
		points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f)
		};
	}

	public Vector3 GetPoint(int index, float t)
	{
		return transform.TransformPoint(Vector3.Lerp(Vector3.Lerp(points[index], points[index+1], t), Vector3.Lerp(points[index+1], points[index+2], t), t));
	}
}