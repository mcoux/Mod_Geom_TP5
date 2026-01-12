using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class EXO_2 : MonoBehaviour
{
    private Mesh mesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        createMesh();
        // foreach (Vector3 p in mesh.vertices) Debug.Log(p);
        Debug.Log(mesh.vertices.Length);
        Debug.Log(valence(0));
       Loop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int valence(int index)
    {
        int count = 0;
        for (int i = 0; i < mesh.triangles.Length; i++)
        {
            if (mesh.triangles[i] == index)
            {
                count++;
            }
        }
        return count;
    }

    private void Loop()
    {
        Mesh mP = mesh;
        List<Vector3> newPoints = mesh.vertices.ToList();
        Vector3 p1, p2, p3, newP;
        List<int> currentVs = new List<int>();
        for(int i = 0;i < mesh.triangles.Length; i += 3)
        {
            newPoints.Add(createNewPoint(mesh.triangles[i], mesh.triangles[i + 1]));
            newPoints.Add(createNewPoint(mesh.triangles[i+1], mesh.triangles[i +2]));
            newPoints.Add(createNewPoint(mesh.triangles[i], mesh.triangles[i + 2]));
            Debug.Log(newPoints.Count);
        }
    }

    private List<int> getVL_VR(int i1, int i2)
    {
        List<int> output = new List<int>();
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            if ((mesh.triangles[i] == i1 && mesh.triangles[i + 1] == i2) ||
                (mesh.triangles[i] == i2 && mesh.triangles[i + 1] == i1))
            {
                output.Add(mesh.triangles[i + 2]);
            }

            if ((mesh.triangles[i + 1] == i1 && mesh.triangles[i + 2] == i2) ||
               (mesh.triangles[i + 1] == i2 && mesh.triangles[i + 2] == i1))
            { 
                output.Add(mesh.triangles[i]); 
            }

            if ((mesh.triangles[i] == i1 && mesh.triangles[i + 2] == i2) ||
               (mesh.triangles[i] == i2 && mesh.triangles[i + 2] == i1))
            { 
                output.Add(mesh.triangles[i + 1]); 
            }
        }

        return output;
    }

    private Vector3 createNewPoint(int p1, int p2) 
    {
        List<int> vs = getVL_VR(p1, p2);

        return 3 * (mesh.vertices[p1] + mesh.vertices[p2]) / 8 + (mesh.vertices[vs[0]] + mesh.vertices[vs[1]]) / 8;
    }

    //Le cube de base de unity est fait avec le c#l donc pour faire marcher l'algo je le recrée manuellement
    private void createMesh()
    {
        Vector3[] vertices = new Vector3[8];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(1, 0, 0);
        vertices[2] = new Vector3(1, 1, 0);
        vertices[3] = new Vector3(0, 1, 0);
        vertices[4] = new Vector3(0, 1, 1);
        vertices[5] = new Vector3(1, 1, 1);
        vertices[6] = new Vector3(1, 0, 1);
        vertices[7] = new Vector3(0, 0, 1);

        List<int> triangles = new List<int>
        {
             0, 3, 2,
             0 ,2, 1,
             7, 4, 3,
             7, 3, 0,
             1, 2, 5,
             1, 5, 6,
             0, 6, 7,
             0, 1, 6,
             3, 4, 5,
             3, 5, 2,
             6, 5, 4,
             6, 4, 7,
        };

        mesh.triangles = triangles.ToArray();
        mesh.vertices = vertices;
        mesh.RecalculateBounds();




    }

   
}
