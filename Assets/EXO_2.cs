using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
public class EXO_2 : MonoBehaviour
{
    private Mesh mesh;
    private List<Vector3> newVertrices = new List<Vector3>();

    // Clé : index du point de base dans la mesh de base. Valeurs : index des nouveaux points 
    Dictionary<(int,int), int> pointsParArete = new Dictionary<(int,int), int>();
    //Clé ; index du point dans la forme de base. Valeur : index du point dans la nouvelle mesh
    Dictionary<int,int> majPoints = new Dictionary<int,int>();

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

    private float alpha(int n)
    {
        if (n == 3)
            return 3 / 16;
        else
        {
            return (5/8 - Mathf.Pow((3/8 + (MathF.Cos((2*MathF.PI)/n))/4),2)) / n;
        }
    }

    private void Loop()
    {
        Mesh mP = mesh;
        List<Vector3> newPoints = mesh.vertices.ToList();
        Vector3 p1, p2, p3, newP;
        List<int> currentVs = new List<int>();
        //Etape 1
        for(int i = 0;i < mesh.triangles.Length; i += 3)
        {
            //On ajoute le point de chaque arête dans un dictionnaire qui sera réutilisé pour la partie 3
            newPoints.Add(createNewPoint(mesh.triangles[i], mesh.triangles[i + 1]));
            pointsParArete.Add((mesh.triangles[i], mesh.triangles[i + 1]), newPoints.Count - 1);

            newPoints.Add(createNewPoint(mesh.triangles[i+1], mesh.triangles[i +2]));
            pointsParArete.Add((mesh.triangles[i+1], mesh.triangles[i + 2]), newPoints.Count - 1);

            newPoints.Add(createNewPoint(mesh.triangles[i+2], mesh.triangles[i]));
            pointsParArete.Add((mesh.triangles[i + 2], mesh.triangles[i]), newPoints.Count - 1);
            Debug.Log(newPoints.Count);
        }

        //Etape 2
        int valence;
        float alpha;
        float sommeV;
        Vector3 newPoint;
        for (int i = 0; i < mesh.vertices.Length; i++) 
        {
            valence = this.valence(i);
            alpha = this.alpha(valence);
            newPoint = (1 - valence * alpha) * mesh.vertices[i] ;
            sommeV = getSommeVoisins(i);
            newPoint.x += alpha * getSommeVoisins(i);
            newPoint.y += alpha * getSommeVoisins(i);
            newPoint.z += alpha * getSommeVoisins(i);
            newPoints.Add(newPoint);    
            majPoints.Add(i,newPoints.Count - 1);
        }

        //Etape3
        List<int> newFaces = new List<int>();
        int x1,x2,x3, x1x2, x2x3, x3x1;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            x1 = majPoints[mesh.triangles[i]];
            x2 = majPoints[mesh.triangles[i + 1]];
            x3 = majPoints[mesh.triangles[i +2]];

            x1x2 = pointsParArete[(mesh.triangles[i], mesh.triangles[i + 1])];
            x2x3 = pointsParArete[(mesh.triangles[i+1], mesh.triangles[i + 2])];
            x3x1 = pointsParArete[(mesh.triangles[i + 2], mesh.triangles[i])];

            //Face 1
            newFaces.Add(x1);
            newFaces.Add(x2x3);
            newFaces.Add(x3x1);

            //Face 2
            newFaces.Add(x2);
            newFaces.Add(x2x3);
            newFaces.Add(x1x2);

            //Face 3
            newFaces.Add(x3);
            newFaces.Add(x3x1);
            newFaces.Add(x2x3);

            //Face 4
            newFaces.Add(x1x2);
            newFaces.Add(x2x3);
            newFaces.Add(x3x1);

        }
        newVertrices = newPoints;

        mesh.vertices = newPoints.ToArray();
        mesh.triangles = newFaces.ToArray();
        mesh.RecalculateBounds();
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

    float getSommeVoisins(int index) 
    { 
        List<int> voisins = new List<int>();
        float somme = 0;
        for(int i = 0; i < mesh.triangles.Length; i += 3)
        {
            if (mesh.triangles[i] == index)
            {
                if(!voisins.Contains(mesh.triangles[i+1])) voisins.Add(mesh.triangles[i+1]);
                if (!voisins.Contains(mesh.triangles[i + 2])) voisins.Add(mesh.triangles[i + 2]);
            }
            else if (mesh.triangles[i+1] == index)
            {
                if (!voisins.Contains(mesh.triangles[i ])) voisins.Add(mesh.triangles[i ]);
                if (!voisins.Contains(mesh.triangles[i + 2])) voisins.Add(mesh.triangles[i + 2]);
            }
            else if (mesh.triangles[i + 2] == index)
            {
                if (!voisins.Contains(mesh.triangles[i + 1])) voisins.Add(mesh.triangles[i + 1]);
                if (!voisins.Contains(mesh.triangles[i ])) voisins.Add(mesh.triangles[i ]);
            }
        }

        foreach (int ind in voisins) 
        {
            somme += alpha(valence(ind));
        }
        return somme;
    }

    List<Vector3> nouveauxSommets(int i)
    {
        List<Vector3> output = new List<Vector3>();

        output.Add(createNewPoint(mesh.triangles[i], mesh.triangles[i + 1]));
        output.Add(createNewPoint(mesh.triangles[i + 1], mesh.triangles[i + 2]));
        output.Add(createNewPoint(mesh.triangles[i], mesh.triangles[i + 2]));

        return output;  
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 point in newVertrices)
        {
            Gizmos.DrawIcon(point + transform.position, "e");
        }
    }
}
