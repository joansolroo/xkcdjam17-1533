using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierRenderer : MonoBehaviour {

    [System.Serializable]
    public enum Type
    {
        POINT, LINEAR, BEZIER
    }
    public Type type = Type.BEZIER;
    public Vector3[] points;
    public int subsegments = 3;

    public Vector3[] subpoints_static;
    protected Vector3[] subpoints_updated;

    protected LineRenderer lr;
    public float width = 0.1f;
    public float noiseRatio = 0.1f;
    public Material material;
    public Color color;
	// Use this for initialization
	void Awake () {
        material = new Material(Shader.Find("Unlit/Color"));
        lr = gameObject.AddComponent<LineRenderer>();
        if (points != null && points.Length > 0)
        {
            ComputeLine();
        }
	}


    public void SetLine(Vector3[] _points, int _subsegments, float _width = 1, float _noiseRatio = 0, Type _type = Type.BEZIER)
    {
        points = _points;
        subsegments = _subsegments;
        width = _width;
        noiseRatio = _noiseRatio;
        type = _type;
        ComputeLine();
    }
    protected void ComputeLine()
    {
        if (subsegments > 1)
        {
            if (type == Type.BEZIER)
            {
                List<Vector3> controlPoints = new List<Vector3>(points.Length);
                foreach (Vector3 p in points) controlPoints.Add(p);
                subpoints_static = BezierCurveScript.BezierPath.CreateCurve(controlPoints, subsegments).ToArray();
            }
            else
            {
                subpoints_static = new Vector3[(points.Length-1) * subsegments+1];
                for(int p = 0; p< points.Length-1; ++p)
                {
                    for(int s = 0; s< subsegments; ++s)
                    {
                        float t = ((float)s) / subsegments;
                        subpoints_static[p * subsegments + s] = points[p] * (1 - t) + points[p + 1] * t;
                    }
                }
                subpoints_static[subpoints_static.Length - 1] = points[points.Length - 1];
            }
            subpoints_updated = new Vector3[subpoints_static.Length];
            for (int p = 0; p < subpoints_static.Length; ++p)
            {
                float r = noiseRatio * width * Random.value;
                subpoints_static[p] += new Vector3(Random.Range(-r, +r), Random.Range(-r, +r), 0);
                //subpoints_noisy[p] = transform.TransformPoint(subpoints[p]);
                subpoints_updated[p] = transform.TransformPoint(subpoints_static[p]) + new Vector3(Random.Range(-r, +r), Random.Range(-r, +r), 0);
            }
            lr.positionCount = subpoints_updated.Length;
            lr.SetPositions(subpoints_updated);
            
        }
        else
        {
            subpoints_static = new Vector3[points.Length];
            subpoints_updated = new Vector3[points.Length];
            for (int p = 0; p < points.Length; ++p)
            {
                float r = noiseRatio * width * Random.value;

                subpoints_static[p] = points[p]+new Vector3(Random.Range(-r, +r), Random.Range(-r, +r), 0);

                //subpoints_noisy[p] = transform.TransformPoint(subpoints[p]);
                subpoints_updated[p] = transform.TransformPoint(points[p]) + new Vector3(Random.Range(-r, +r), Random.Range(-r, +r), 0);
            }
            lr.positionCount = subpoints_updated.Length;
            lr.SetPositions(subpoints_updated);
        }

        lr.numCapVertices = 10;
        lr.numCornerVertices = 10;
        lr.SetWidth(width, width);
        lr.material = material;
        lr.material.color = color;
    }
	
	// Update is called once per frame
	protected void Update () {
        if (points != null && points.Length > 0 && subpoints_updated!=null)
        {
            if (transform.hasChanged)
            {
                for (int p = 0; p < subpoints_static.Length; ++p)
                {
                    subpoints_updated[p] = transform.TransformPoint(subpoints_static[p]);
                }
                lr.positionCount = subpoints_updated.Length;
                lr.SetPositions(subpoints_updated);
            }
            
        }
        lr.material.color = color;
    }
}
