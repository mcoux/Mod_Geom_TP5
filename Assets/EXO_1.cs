using JetBrains.Annotations;
using UnityEngine;
using System.Collections.Generic;
public class EXO_1 : MonoBehaviour
{
    [SerializeField] public List<Vector2> startPoints = new List<Vector2>();
    private List<Vector2> newPoints = new List<Vector2>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        algo(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void algo(int depth)
    {
        List<Vector2> points = startPoints;
        for (int j = 0; j < depth; j++)
        {
            newPoints = new List<Vector2>();
            for (int i = 1; i < points.Count; i++)
            {
                newPoints.Add(points[i - 1] * 3 / 4 + points[i] * 1 / 4);
                newPoints.Add(points[i - 1] * 1 / 4 + points[i] * 3 / 4);

            }

            newPoints.Add(points[points.Count - 1] * 3 / 4 + points[0] * 1 / 4);
            newPoints.Add(points[points.Count - 1] * 1 / 4 + points[0] * 3 / 4);

            points = newPoints;
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var point in startPoints) 
        {
            Gizmos.DrawIcon(point, "p");
        }

        foreach (var point in newPoints)
        {
            Gizmos.DrawIcon(point, "p");
        }

        for (int i = 0; i < newPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(newPoints[i], newPoints[i + 1]);
        }
        if (newPoints.Count > 0)
        {
            Gizmos.DrawLine(newPoints[0], newPoints[newPoints.Count - 1]);
        }
    }
}
