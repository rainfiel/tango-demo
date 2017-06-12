using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshView : MonoBehaviour {

    public Material material;

    void Start()
    {
    }

    void OnRenderObject()
    {
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
     //   Debug.Log(triangulation.indices.Length);

        if (material == null)
        {
            return;
        }
     //   GL.PushMatrix();

        material.SetPass(0);
        GL.Begin(GL.TRIANGLES);
        for (int i = 0; i < triangulation.indices.Length; i += 3)
        {
            var triangleIndex = i / 3;
            var i1 = triangulation.indices[i];
            var i2 = triangulation.indices[i + 1];
            var i3 = triangulation.indices[i + 2];
            var p1 = triangulation.vertices[i1];
            var p2 = triangulation.vertices[i2];
            var p3 = triangulation.vertices[i3];
            var areaIndex = triangulation.areas[triangleIndex];
            Color color;
            switch (areaIndex)
            {
                case 0:
                    color = Color.green; break; //walkable
                case 1:
                    color = Color.red; break; //unwalkable
                default:
                    color = Color.white; break; //unknown
            }
            GL.Color(color);
            GL.Vertex(p1);
            GL.Vertex(p2);
            GL.Vertex(p3);
        }
        GL.End();

    //    GL.PopMatrix();
    }
}
