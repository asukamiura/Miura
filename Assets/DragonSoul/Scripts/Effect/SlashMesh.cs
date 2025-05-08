using UnityEngine;

public class SlashMesh : MonoBehaviour
{
    [SerializeField] Transform tipTransform;
    [SerializeField] Transform baseTransform;
    [SerializeField] GameObject trailMesh;
    [SerializeField] int trailFrameLength;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    int frameCount;
    Vector3 previousTipPosition;
    Vector3 previousBasePosition;

    const int NUM_VERTICES = 12;

    void Start()
    {
        mesh = new Mesh();
        trailMesh.GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[trailFrameLength * NUM_VERTICES];
        triangles = new int[vertices.Length];

        previousTipPosition = tipTransform.position;
        previousBasePosition = baseTransform.position;
    }

    void LateUpdate()
    {
        if (frameCount == (trailFrameLength * NUM_VERTICES))
        {
            frameCount = 0;
        }

        vertices[frameCount] = baseTransform.position;
        vertices[frameCount + 1] = tipTransform.position;
        vertices[frameCount + 2] = previousTipPosition;

        vertices[frameCount + 3] = baseTransform.position;
        vertices[frameCount + 4] = previousTipPosition;
        vertices[frameCount + 5] = tipTransform.position;

        vertices[frameCount + 6] = previousTipPosition;
        vertices[frameCount + 7] = baseTransform.position;
        vertices[frameCount + 8] = previousBasePosition;

        vertices[frameCount + 9] = previousTipPosition;
        vertices[frameCount + 10] = previousBasePosition;
        vertices[frameCount + 11] = baseTransform.position;

        triangles[frameCount] = frameCount;
        triangles[frameCount + 1] = frameCount + 1;
        triangles[frameCount + 2] = frameCount + 2;
        triangles[frameCount + 3] = frameCount + 3;
        triangles[frameCount + 4] = frameCount + 4;
        triangles[frameCount + 5] = frameCount + 5;
        triangles[frameCount + 6] = frameCount + 6;
        triangles[frameCount + 7] = frameCount + 7;
        triangles[frameCount + 8] = frameCount + 8;
        triangles[frameCount + 9] = frameCount + 9;
        triangles[frameCount + 10] = frameCount + 10;
        triangles[frameCount + 11] = frameCount + 11;

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        previousTipPosition = tipTransform.position;
        previousBasePosition = baseTransform.position;

        frameCount += NUM_VERTICES;
    }
}
