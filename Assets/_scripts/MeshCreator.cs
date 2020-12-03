using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshCreator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    public float width, height, steepness;
    private Vector3[] normals;

    void Update()
    {

         createFloor();
        // createTriangleWall();
    }

    void createFloor()
    {
        mesh = new Mesh();
        vertices = new Vector3[4];
        vertices[0] = new Vector3(-width, -steepness, -height);
        vertices[1] = new Vector3(-width, -steepness, height);
        vertices[2] = new Vector3(width, steepness, height);
        vertices[3] = new Vector3(width, steepness, -height);

        mesh.vertices = vertices;
        mesh.triangles = new int[] {
            0, 1, 2,
            0, 2, 3
        };


        normals = new Vector3[4];
        normals[0] = new Vector3(0, 1, 0);
        normals[1] = new Vector3(0, 1, 0);
        normals[2] = new Vector3(0, 1, 0);
        normals[3] = new Vector3(0, 1, 0);
        mesh.normals = normals;
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void createTriangleWall ()
    {
        mesh = new Mesh();
        vertices = new Vector3[3];
        vertices[0] = new Vector3(-0.5f, -0.5f);
        vertices[2] = new Vector3(0.5f, -0.5f);
        vertices[1] = new Vector3(0.5f, 0.5f);

        mesh.vertices = vertices;
        mesh.triangles = new int[] {
            0, 1, 2
        };
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
