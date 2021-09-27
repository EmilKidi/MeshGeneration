using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
	public int lineSteps = 12;

	private BezierCurve curve;
	private Transform handleTransform;
	private Quaternion handleRotation;
	private Vector3[] miniPoints;

	private GameObject roadObject;
	private Mesh roadMesh;
	private Vector3[] roadVertices;
	private int[] roadTriangles;

	public void OnSceneGUI()
	{
		curve = target as BezierCurve;
		handleTransform = curve.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local 
			? handleTransform.rotation 
			: Quaternion.identity;

		miniPoints = (curve.lineSteps > 0) ? new Vector3[curve.lineSteps * curve.points.Length] : new Vector3[1];

		Handles.color = Color.blue;
		ShowControlPoint();

		Handles.color = Color.white;
		ShowLineSteps();

		if (curve.lineSteps > 0)
        {
			CreateVertices();
        }
	}

	private void ShowControlPoint()
    {
		for (int i = 1; i <= curve.points.Length && i >= 0 && i + 1 < curve.points.Length; i++)
		{
			Vector3 p1 = ShowPoint(i);
			Vector3 p2 = ShowPoint(i + 1);
			Handles.DrawLine(p1, p2);
		}
	}

	private void ShowLineSteps()
    {
		int miniPointsIndex = 0;

		for (int i = 0; i <= curve.points.Length && i >= 0 && i + 2 < curve.points.Length; i++)
		{
			Vector3 lineStartMini = curve.GetPoint(i, 0);
			for (int j = 1; j <= curve.lineSteps; j++)
			{
				miniPoints[miniPointsIndex] = lineStartMini;
				miniPointsIndex += 1;

				float t = j / (float)curve.lineSteps;
				Vector3 lineEndMini = curve.GetPoint(i, t);
				Handles.SphereHandleCap(0, lineEndMini, Quaternion.identity, 0.3f, EventType.Repaint);
				lineStartMini = lineEndMini;
			}

			i += 1;
		}
	}

	private Vector3 ShowPoint(int index)
	{
		Vector3 point = handleTransform.TransformPoint(curve.points[index]);
		EditorGUI.BeginChangeCheck();

		point = Handles.DoPositionHandle(point, handleRotation);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(curve, "Move Point");
			EditorUtility.SetDirty(curve);
			curve.points[index] = handleTransform.InverseTransformPoint(point);
		}
		return point;
	}

	private void CreateVertices()
	{
		int t = 0;
		int v = 0;
		Vector3 normal = new Vector3();

		roadVertices = new Vector3[miniPoints.Length * 4];
		roadTriangles = new int[roadVertices.Length];

		roadObject = GenerateObject();
		roadMesh = new Mesh();
		roadObject.GetComponent<MeshFilter>().mesh = roadMesh;

		for (int i = 0; i + 1 < miniPoints.Length && v + 3 < roadVertices.Length; i+=1)
        {

			Vector3 n1 = miniPoints[i];
			Vector3 n2 = miniPoints[i + 1];
			Vector3 n3 = miniPoints[i + 1] + Vector3.up;
			normal = GetNormal(n1, n2, n3);

			roadVertices[v] = miniPoints[i];
			roadVertices[v+1] = miniPoints[i] + normal * curve.width;
			roadVertices[v+2] = miniPoints[i+1];
			roadVertices[v+3] = miniPoints[i+1] + normal * curve.width;
			v += 4;
		}

		for (int i = 0; t + 9 < roadTriangles.Length - 9; i+=4)
		{
			roadTriangles[t] = i;
			roadTriangles[t+1] = i+1;
			roadTriangles[t+2] = i+2;

			roadTriangles[t+3] = i+1;
			roadTriangles[t+4] = i+5;
			roadTriangles[t+5] = i+2;

			t += 9;
		}

		roadMesh.Clear();
		roadMesh.vertices = roadVertices;
		roadMesh.triangles = roadTriangles;
		roadMesh.RecalculateNormals();
	}

	Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
	{
		Vector3 side1 = b - a;
		Vector3 side2 = c - a;

		return Vector3.Cross(side1, side2).normalized;
	}

	private GameObject GenerateObject()
	{
		if (GameObject.Find("RoadObject") == null)
		{
			roadObject = new GameObject("RoadObject");
			roadObject.AddComponent<MeshFilter>();
			roadObject.AddComponent<MeshRenderer>();
			roadObject.transform.parent = curve.transform.parent;
			return roadObject;
		}

		return GameObject.Find("RoadObject").gameObject;
	}
}
