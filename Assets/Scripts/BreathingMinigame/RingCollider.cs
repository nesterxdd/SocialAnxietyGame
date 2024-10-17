using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class RingCollider2D : MonoBehaviour
{
    [Range(3, 100)]
    public int segments = 20; // Number of segments to define the ring
    public float outerRadius = 1f; // Outer radius of the ring
    public float innerRadius = 0.5f; // Inner radius of the ring

    private PolygonCollider2D polygonCollider;

    void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        CreateRingCollider();
    }

    void OnValidate()
    {
        if (polygonCollider == null)
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
        }
        CreateRingCollider();
    }

    void CreateRingCollider()
    {
        if (segments < 3)
        {
            Debug.LogWarning("Segments must be 3 or greater.");
            return;
        }
        if (innerRadius >= outerRadius)
        {
            Debug.LogWarning("Inner radius must be smaller than outer radius.");
            return;
        }

        Vector2[] vertices = new Vector2[segments * 2];
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;
            float outerX = Mathf.Cos(angle) * outerRadius;
            float outerY = Mathf.Sin(angle) * outerRadius;
            float innerX = Mathf.Cos(angle) * innerRadius;
            float innerY = Mathf.Sin(angle) * innerRadius;

            vertices[i] = new Vector2(outerX, outerY);
            vertices[segments + i] = new Vector2(innerX, innerY);
        }

        polygonCollider.pathCount = 2;
        Vector2[] outerPath = new Vector2[segments];
        Vector2[] innerPath = new Vector2[segments];

        for (int i = 0; i < segments; i++)
        {
            outerPath[i] = vertices[i];
            innerPath[segments - 1 - i] = vertices[segments + i]; // Reverse inner path to maintain correct winding
        }

        polygonCollider.SetPath(0, outerPath);
        polygonCollider.SetPath(1, innerPath);
    }
}
