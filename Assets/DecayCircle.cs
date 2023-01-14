using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayCircle : MonoBehaviour
{
    private Transform circleTransform;

    private static Vector3 circleSize, circlePosition;

    private void Awake()
    {
        circleTransform = GetComponent<Transform>();
        SetCircleSize(new Vector3(10, 10), new Vector3(0, 0));
    }

    private void SetCircleSize(Vector3 size, Vector3 position)
    {
        circleTransform.localScale = size;
        circleTransform.localPosition = position;  
        circlePosition= position;
        circleSize = size;
    }

    public static bool IsOutsideCircle(Vector3 position)
    {
        float circleRadius = circleSize.x * 0.5f;
        return Vector3.Distance(position, circlePosition) > circleRadius;
    }
}
