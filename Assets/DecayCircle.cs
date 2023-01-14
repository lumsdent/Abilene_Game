using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DecayCircle : MonoBehaviour
{
    private Transform circleTransform;

    private static Vector3 circleSize, circlePosition;
    private Vector3 targetCircleSize, targetCirclePosition;
    private float circleShrinkSpeed;

    private Tilemap tilemap;
    private void Awake()
    {
        tilemap= GetComponent<Tilemap>();
        circleShrinkSpeed= 1.0f;
        circleTransform = GetComponent<Transform>();
        SetCircleSize(new Vector3(10, 10), new Vector3(0, 0));

        targetCirclePosition= new Vector3(1, 1);
        targetCircleSize = new Vector3(6, 6);
    }

    private void Update()
    {
        Vector3 sizeChangeVector = (targetCircleSize - circleSize).normalized;
        Vector3 newCircleSize = circleSize + sizeChangeVector * Time.deltaTime * circleShrinkSpeed;
        
        Vector3 positionChangeVector = (targetCirclePosition - circlePosition).normalized;
        Vector3 newCirclePosition = circlePosition + positionChangeVector * Time.deltaTime * circleShrinkSpeed;
        SetCircleSize(newCircleSize, newCirclePosition);
        StartCoroutine("RefreshTiles");
    }

    IEnumerator RefreshTiles()
    {
        if(tilemap!=null)
        {
            tilemap.RefreshAllTiles();
        }
        // execute block of code here
        yield return new WaitForSeconds(circleShrinkSpeed);

    }

    private void SetCircleSize(Vector3 size, Vector3 position)
    {
        circlePosition= position;
        circleSize = size;

        circleTransform.localScale = size;
        circleTransform.localPosition = position;  
    }

    public static bool IsOutsideCircle(Vector3 position)
    {
        float circleRadius = circleSize.x * 0.5f;
        return Vector3.Distance(position, circlePosition) > circleRadius;
    }
}
