using UnityEngine;

public class FindPath : MonoBehaviour
{
public int gridSizeX = 10, gridSizeY = 10;
    private Vector3[,] gridNodes;

    void Start() {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        gridNodes = new Vector3[gridSizeX, gridSizeY];

        int index = 0;
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                if (index < vertices.Length) {
                    gridNodes[x, y] = transform.TransformPoint(vertices[index]);
                    index++;
                }
            }
        }
    }


    void OnDrawGizmos() {
    if (gridNodes == null) return;

    Gizmos.color = Color.green;
    foreach (Vector3 node in gridNodes) {
        Gizmos.DrawSphere(node, 0.1f);
    }
}


}
