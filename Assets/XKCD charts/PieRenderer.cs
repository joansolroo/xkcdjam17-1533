using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PieRenderer : BezierRenderer
{

    public int segments = 60;
    public float[] sectionsQuantities;
    public string[] sectionsLabels;
    BezierRenderer[] separators;
    Gradient newGradient;

    public Canvas captionSpace;
    public Text textTemplate;
    public Text[] captions;
    // Use this for initialization
    void Start()
    {
        points = new Vector3[segments + 1];
        for (int angle = 0; angle < segments; angle++)
        {
            points[angle] = new Vector3(Mathf.Sin(360f * angle / points.Length * Mathf.Deg2Rad), Mathf.Cos(360f * angle / points.Length * Mathf.Deg2Rad), 0);

        }
        points[points.Length - 1] = points[0];
        ComputeLine();

        sections_cumulate = new float[sectionsLabels.Length];
        sections_cumulate2 = new float[sectionsLabels.Length];
        UpdateCumulate();
        separators = new BezierRenderer[sectionsLabels.Length];

        material = new Material(Shader.Find("Particles/Alpha Blended"));
        newGradient = new Gradient();
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[sectionsLabels.Length];
        GradientColorKey[] colorKeys = new GradientColorKey[sectionsLabels.Length];
        for (int s = 0; s < sectionsLabels.Length; ++s)
        {
            BezierRenderer l2 = new GameObject().AddComponent<BezierRenderer>();
            l2.transform.parent = this.transform;
            l2.transform.localPosition = Vector3.zero;
            l2.transform.localRotation = Quaternion.identity;
            l2.transform.localScale = Vector3.one;
            Vector3[] p2 = new Vector3[2];
            int idx = (int)(sections_cumulate[s] * (points.Length - 1));
            p2[1] = points[idx];
            l2.SetLine(p2, 10, width, noiseRatio, Type.LINEAR);
            separators[s] = l2;
            l2.color = Color.Lerp(Color.black, Color.gray, Random.value);
            alphaKeys[s] = new GradientAlphaKey(1, sections_cumulate[s]);
            colorKeys[s] = new GradientColorKey(l2.color, sections_cumulate[s]);
        }
        newGradient.mode = GradientMode.Fixed;
        newGradient.SetKeys(colorKeys, alphaKeys);
        lr.colorGradient = newGradient;
        lr.material = material;

        captions = new Text[sectionsLabels.Length];

        for (int s = 0; s < sections_cumulate.Length - 1; ++s)
        {
            int idx1 = (int)(sections_cumulate[s] * (points.Length - 1));
            int idx2 = (int)(sections_cumulate[s + 1] * (points.Length - 1));

            Vector3 p0 = transform.TransformPoint(Vector3.zero);
            Vector3 p1 = transform.TransformPoint(subpoints_static[idx1 * subsegments]);
            Vector3 p2 = transform.TransformPoint(subpoints_static[(idx1 + idx2) / 2 * subsegments]);
            Vector3 p3 = transform.TransformPoint(subpoints_static[idx2 * subsegments]);

            if (s == 0)
            {
                captions[s] = textTemplate;
            }
            else
            {
                captions[s] = GameObject.Instantiate(textTemplate);
            }
            Vector3 center = ((p0 + p1 + p2 + p3) * 0.25f + p2) * .5f;
            captions[s].transform.SetParent(captionSpace.transform);
            captions[s].rectTransform.position = center;
            captions[s].rectTransform.localScale = textTemplate.rectTransform.localScale;
            captions[s].text = sectionsLabels[s];
        }

    }
    float[] sections_cumulate;
    float[] sections_cumulate2;
    bool UpdateCumulate()
    {
        bool changed = false;
        float sections_total = 0;
        for (int s = 0; s < sectionsLabels.Length; ++s)
        {
            sections_total += Mathf.Max(0, sectionsQuantities[s]);
            sections_cumulate2[s] = sections_total;

        }
        for (int s = 0; s < sections_cumulate.Length; ++s)
        {
            sections_cumulate2[s] = sections_cumulate2[s] / sections_total;
            if (sections_cumulate[s] != sections_cumulate2[s])
            {
                sections_cumulate[s] = sections_cumulate2[s];
                changed = true;
            }
        }
        return changed;
    }
    void UpdateGradient()
    {
        GradientAlphaKey[] alphaKeys = newGradient.alphaKeys;
        GradientColorKey[] colorKeys = newGradient.colorKeys;
        for (int s = 0; s < sectionsLabels.Length; ++s)
        {
            BezierRenderer l2 = separators[s];

            //l2.color = Color.Lerp(Color.black, Color.gray, Random.value);
            alphaKeys[s].time = sections_cumulate[s];
            colorKeys[s].time = sections_cumulate[s];
        }
        newGradient.mode = GradientMode.Fixed;
        newGradient.SetKeys(colorKeys, alphaKeys);
        lr.colorGradient = newGradient;
        lr.material = material;
    }
    private void Update()
    {
        base.Update();

        if (UpdateCumulate())
        {
            for (int s = 0; s < sectionsLabels.Length; ++s)
            {
                BezierRenderer l2 = separators[s];

                /*l2.transform.parent = this.transform;
                l2.transform.localPosition = Vector3.zero;
                l2.transform.localRotation = Quaternion.identity;
                l2.transform.localScale = Vector3.one;*/
                Vector3[] p2 = new Vector3[2];
                int idx = (int)(sections_cumulate[s] * (points.Length - 1));
                p2[1] = points[idx];
                l2.SetLine(p2, 10, width, noiseRatio, Type.LINEAR);

            }
            UpdateGradient();

            for (int s = 0; s < sections_cumulate.Length - 1; ++s)
            {
                int idx1 = (int)(sections_cumulate[s] * (points.Length - 1));
                int idx2 = (int)(sections_cumulate[s + 1] * (points.Length - 1));

                Vector3 p0 = transform.TransformPoint(Vector3.zero);
                Vector3 p1 = transform.TransformPoint(subpoints_static[idx1 * subsegments]);
                Vector3 p2 = transform.TransformPoint(subpoints_static[(idx1 + idx2) / 2 * subsegments]);
                Vector3 p3 = transform.TransformPoint(subpoints_static[idx2 * subsegments]);

                float offs = 0.25f;
                Vector3 center = ((p0 + p1 + p2 + p3) * 0.25f*(1-offs) + p2*offs);
                captions[s].rectTransform.position = center;
                captions[s].rectTransform.localScale = textTemplate.rectTransform.localScale;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (sections_cumulate != null)
        {
            Gizmos.color = Color.red;
            for (int s = 0; s < sections_cumulate.Length - 1; ++s)
            {
                int idx1 = (int)(sections_cumulate[s] * (points.Length - 1));
                int idx2 = (int)(sections_cumulate[s + 1] * (points.Length - 1));

                //  Gizmos.DrawSphere(transform.TransformPoint(subpoints_static[idx1 * subsegments]), 0.5f);
                //  Gizmos.DrawSphere(transform.TransformPoint((subpoints_static[idx2 * subsegments])), 0.5f);
                //  Gizmos.DrawSphere(transform.TransformPoint((subpoints_static[(idx1+idx2)/2 * subsegments])), 0.5f);

                Vector3 p0 = transform.TransformPoint(Vector3.zero);
                Vector3 p1 = transform.TransformPoint(subpoints_static[idx1 * subsegments]);
                Vector3 p2 = transform.TransformPoint(subpoints_static[(idx1 + idx2) / 2 * subsegments]);
                Vector3 p3 = transform.TransformPoint(subpoints_static[idx2 * subsegments]);
                Gizmos.DrawLine(p0, p1);
                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p2, p3);
                Gizmos.DrawLine(p3, p0);
                Vector3 center = ((p0 + p1 + p2 + p3) * 0.25f + p2) * .5f;
                Gizmos.DrawSphere(center, 0.25f);

            }
        }
    }
}
