using UnityEngine;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
public class Shape : MonoBehaviour
{
    const float MIN_CIRCLE_PERCENT = 0.001f;
    static readonly float rootOf2 = Mathf.Sqrt(2);

    [SerializeField, Range(0, 1)] float circlePercent;
    [SerializeField, Range(4, 100)] int resolution = 8;
    [SerializeField, Range(1, 10)] float circleRadius = 1;
    [SerializeField, Range(1, 10)] float squareRadius = 1;

    new PolygonCollider2D collider;
    Vector2[] circleArray, squareArray, currentArray;

    public float CirclePercent
    {
        get { return Mathf.Max(circlePercent, MIN_CIRCLE_PERCENT); }
        set { circlePercent = Mathf.Clamp01(value); }
    }

    private void Awake()
    {
        collider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        UpdateArraysSize();

    }

    void UpdateArraysSize()
    {
        Array.Resize(ref circleArray, resolution);
        Array.Resize(ref squareArray, resolution);
        Array.Resize(ref currentArray, resolution);

        float degreesStep = 360f / resolution;
        for (int i = 0; i < resolution; i++)
        {
            float degrees = (degreesStep * i + 45) % 360;
            var rotation = Quaternion.Euler(0, 0, degrees);
            circleArray[i] = rotation * Vector2.up * circleRadius;
            squareArray[i] = new Vector2(
                degrees < 90 || degrees > 270 ? 1 : -1,
                degrees < 180 ? 1 : -1) * squareRadius;
            currentArray[i] = Vector2.Lerp(squareArray[i], circleArray[i], CirclePercent);
        }

        collider.SetPath(0, currentArray);
    }


}
