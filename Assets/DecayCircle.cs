using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayCircle : MonoBehaviour
{
  

    private static Vector3 circleSize, circlePosition;
    private Vector3 targetCircleSize, targetCirclePosition;
    private float circleShrinkSpeed;


    //TODO remove Hard Coding
    private void Awake()
    { 
        SetCircleSize(transform.localScale, transform.position);

        //circle movement
        circleShrinkSpeed= 1.0f;
        targetCirclePosition = new Vector3(1, 1);
        targetCircleSize = new Vector3(6, 6);
    }

    //Only needed if circle moves
    private void Update()
    {
        Vector3 sizeChangeVector = (targetCircleSize - circleSize).normalized;
        Vector3 newCircleSize = circleSize + sizeChangeVector * Time.deltaTime * circleShrinkSpeed;
        
        Vector3 positionChangeVector = (targetCirclePosition - circlePosition).normalized;
        Vector3 newCirclePosition = circlePosition + positionChangeVector * Time.deltaTime * circleShrinkSpeed;
        // SetCircleSize(newCircleSize, newCirclePosition);
    }

    public static bool IsOutsideCircle(Vector3 position)
    {
        //TODO only works for grid value, maybe make it configurable
        float circleRadius = circleSize.x / 0.16f * 0.5f;
        Vector3 circleGridPosition = circlePosition / 0.16f;
        return Vector3.Distance(position, circleGridPosition) > circleRadius;
    }

    private void SetCircleSize(Vector3 size, Vector3 position)
    {
        circlePosition= position;
        circleSize = size;

        transform.localScale = size;
        transform.localPosition = position;  
    }


}
