using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(PolygonCollider2D))]
public class SpriteMesh : MonoBehaviour
{
    Mesh mesh;
    MeshFilter filter;
    new MeshRenderer renderer;
    new PolygonCollider2D collider;

    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();
        collider = GetComponent<PolygonCollider2D>();
        mesh = filter.mesh = new Mesh();
    }

    void Update()
    {
        //Render thing
        int pointCount = 0;
        pointCount = collider.GetTotalPointCount();
        Vector2[] points = collider.points;
        Vector3[] vertices = new Vector3[pointCount];
        Vector2[] uv = new Vector2[pointCount];
        for (int j = 0; j < pointCount; j++)
        {
            Vector2 actual = points[j];
            vertices[j] = new Vector3(actual.x, actual.y, 0);
            uv[j] = actual;
        }
        Triangulator tr = new Triangulator(points);
        int[] triangles = tr.Triangulate();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        filter.mesh = mesh;

        if (gameObject.isStatic && Application.isPlaying)
            enabled = false;
    }

}
