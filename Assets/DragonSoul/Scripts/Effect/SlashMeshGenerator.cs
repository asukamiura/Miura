using System.Collections.Generic;
using UnityEngine;

public class SlashMeshGenerator : MonoBehaviour
{
    [SerializeField] Transform tipTransform;
    [SerializeField] Transform baseTransform;
    [SerializeField] GameObject trailMesh;
    [SerializeField] int trailFrameLength;
    [SerializeField] Material trailMaterial;
    [SerializeField] float trailLifeTime = 0.3f;
    [SerializeField] float minDistance = 0.01f;
    [SerializeField] float maxSegmentInterval = 0.02f;
    [SerializeField] float smoothing = 0.5f;

    Vector3 previousTipPosition, previousBasePosition;

    Vector3 smoothedTopPosition, smoothedBottomPosition;

    Mesh mesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    bool isGenerate = false;
    float segmentTimer = 0;

    class SlashSegment
    {
        public Vector3 top;
        public Vector3 bottom;
        public float time;

        public SlashSegment(Vector3 top, Vector3 bottom, float time)
        {
            this.top = top;
            this.bottom = bottom;
            this.time = time;
        }
    }

    List<SlashSegment> segments = new List<SlashSegment>();

    void Start()
    {
        mesh = new Mesh();
        mesh.MarkDynamic();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;

        meshRenderer.material = trailMaterial;
    }

    void LateUpdate()
    {
        if (!isGenerate) { return; }

        segmentTimer += Time.deltaTime;

        segments.RemoveAll(seg => Time.time - seg.time > trailLifeTime);

        Vector3 rawTop = tipTransform.position;
        Vector3 rawBottom = baseTransform.position;

        smoothedTopPosition = Vector3.Lerp(smoothedTopPosition, rawTop, smoothing);
        smoothedBottomPosition = Vector3.Lerp(smoothedBottomPosition, rawBottom, smoothing);

        if ((smoothedTopPosition - previousTipPosition).sqrMagnitude > minDistance * minDistance || segmentTimer > maxSegmentInterval)
        {
            segments.Add(new SlashSegment(smoothedTopPosition, smoothedBottomPosition, Time.time));

            previousTipPosition = smoothedTopPosition;

            segmentTimer = 0;
        }

        if (segments.Count < 2) { return; }

        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uvs = new List<Vector2>();

        for (int i = 0; i < segments.Count - 1; i++)
        {
            var a = segments[i];
            var b = segments[i + 1];

            int index = vertices.Count;

            vertices.Add(a.bottom);
            vertices.Add(a.top);
            vertices.Add(b.bottom);
            vertices.Add(b.top);

            triangles.Add(index);
            triangles.Add(index + 1);
            triangles.Add(index + 2);

            triangles.Add(index + 2);
            triangles.Add(index + 1);
            triangles.Add(index + 3);

            float u = (float)i / segments.Count;
            uvs.Add(new Vector2(u, 0));
            uvs.Add(new Vector2(u, 1));
            uvs.Add(new Vector2(u + 1f / segments.Count, 0));
            uvs.Add(new Vector2(u + 1f / segments.Count, 1));
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateBounds();
    }

    public void GenerateSlashEffect()
    {
        isGenerate = true;
    }

    public void StopSlashEffect()
    {
        isGenerate = false;
        segments.Clear();
        mesh.Clear();
    }
}
