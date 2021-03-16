using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using EasyEditorGUI;
using UnityEngine;

public class LevelConfig : MonoBehaviour
{
	[SerializeField] private MinMaxFloat      _width;
	[SerializeField] private int              _startTrack = 1;
	public                   int              StartTrack => _startTrack;
	public                   SplineComputer[] tracks = new SplineComputer[3];
	public                   float            minWidth => _width.Min;
	public                   float            maxWidth => _width.Max;

#if UNITY_EDITOR
	public float _sideTrackDistance = 2f;
	[ContextMenu("Generate side tracks")]
	public void GenerateSideTracks()
	{
		var centerSpline = tracks[1];

		var left = Instantiate(centerSpline, transform);
		left.gameObject.name = "left track";
		var points = centerSpline.GetPoints();

		for (var i = 0; i < points.Length; i++)
		{
			var pos = centerSpline.Project(points[i].position);
			var p   = centerSpline.Evaluate(pos);
			points[i].position -= p.right * _sideTrackDistance;
		}
		left.SetPoints(points);
		tracks[0] = left;

		var right = Instantiate(centerSpline, transform);
		right.gameObject.name = "right track";
		points                = centerSpline.GetPoints();

		for (var i = 0; i < points.Length; i++)
		{
			var pos = centerSpline.Project(points[i].position);
			var p   = centerSpline.Evaluate(pos);
			points[i].position += p.right * _sideTrackDistance;
		}
		right.SetPoints(points);
		tracks[2] = right;
		eGUI.SetDirty(this.gameObject);
	}

	[Range(0.001f, 0.25f)] public float step         = 0.01f;
	[Range(-3f,    3f)]    public float heightOffset = 0.01f;
	private void OnDrawGizmosSelected()
	{
		var spline = tracks[1];
		var points = new List<SplineResult>(100);
		step = Mathf.Max(0.001f, step);
		for (var i = 0d; i <= 1d; i += step)
			points.Add(spline.Evaluate(i));
		points.Add(spline.Evaluate(1d));
		for (var i = 0; i < points.Count - 1; i += 1)
		{
			var p0     = points[i];
			var p0rMin = p0.right * _width.Min;
			var p0rMax = p0.right * _width.Max;
			var p1     = points[i + 1];
			var p1rMin = p1.right * _width.Min;
			var p1rMax = p1.right * _width.Max;
			Gizmos.color = Color.red;

			Gizmos.DrawLine(p0.position + p0rMin + new Vector3(0f, heightOffset, 0f), p0.position + p0rMax + new Vector3(0f, heightOffset, 0f));
			Gizmos.DrawLine(p1.position + p1rMin + new Vector3(0f, heightOffset, 0f), p1.position + p1rMax + new Vector3(0f, heightOffset, 0f));
			Gizmos.DrawLine(p0.position + p0rMin + new Vector3(0f, heightOffset, 0f), p1.position + p1rMin + new Vector3(0f, heightOffset, 0f));
			Gizmos.DrawLine(p0.position + p0rMax + new Vector3(0f, heightOffset, 0f), p1.position + p1rMax + new Vector3(0f, heightOffset, 0f));
			Gizmos.DrawLine(p0.position + p0rMax + new Vector3(0f, heightOffset, 0f), p1.position + p1rMin + new Vector3(0f, heightOffset, 0f));
			Gizmos.DrawLine(p0.position + p0rMin + new Vector3(0f, heightOffset, 0f), p1.position + p1rMax + new Vector3(0f, heightOffset, 0f));

			Gizmos.DrawLine(p0.position - p0rMin + new Vector3(0f, heightOffset, 0f), p0.position - p0rMax + new Vector3(0f, heightOffset, 0f));
			Gizmos.DrawLine(p1.position - p1rMin + new Vector3(0f, heightOffset, 0f), p1.position - p1rMax + new Vector3(0f, heightOffset, 0f));
			Gizmos.DrawLine(p0.position - p0rMin + new Vector3(0f, heightOffset, 0f), p1.position - p1rMin + new Vector3(0f, heightOffset, 0f));
			Gizmos.DrawLine(p0.position - p0rMax + new Vector3(0f, heightOffset, 0f), p1.position - p1rMax + new Vector3(0f, heightOffset, 0f));
			Gizmos.DrawLine(p0.position - p0rMax + new Vector3(0f, heightOffset, 0f), p1.position - p1rMin + new Vector3(0f, heightOffset, 0f));
			Gizmos.DrawLine(p0.position - p0rMin + new Vector3(0f, heightOffset, 0f), p1.position - p1rMax + new Vector3(0f, heightOffset, 0f));

			Gizmos.DrawLine(p0.position + new Vector3(0f, heightOffset, 0f), p0.position + p0.normal + new Vector3(0f, heightOffset, 0f));
		}
	}
#endif
}
