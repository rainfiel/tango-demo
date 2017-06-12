using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

// Build and update a localized navmesh from the sources marked by NavMeshSourceTag
[DefaultExecutionOrder(-102)]
public class LocalNavMeshBuilder : MonoBehaviour
{
    // The center of the build
    public Transform m_Tracked;

    // The size of the build bounds
    public Vector3 m_Size = new Vector3(80.0f, 20.0f, 80.0f);
    public Color lineColor = new Color(0.0f, 1.0f, 1.0f);

    NavMeshData m_NavMesh;
    AsyncOperation m_Operation;
    NavMeshDataInstance m_Instance;
    List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();

    private Vector3[] vertex = new Vector3[8];
    private Material lineMaterial;
    
    IEnumerator Start()
    {
        while (true)
        {
            UpdateNavMesh(true);
            yield return m_Operation;
        }
    }

    void OnEnable()
    {
        //setup boundingbox draw
        if (lineMaterial == null)
        {
            lineMaterial = new Material(Shader.Find("Lines/Colored_Blended"));
        }
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;

        // Construct and add navmesh
        m_NavMesh = new NavMeshData();
        m_Instance = NavMesh.AddNavMeshData(m_NavMesh);
        if (m_Tracked == null)
            m_Tracked = transform;
        UpdateNavMesh(false);
    }

    void OnDisable()
    {
        // Unload navmesh and clear handle
        m_Instance.Remove();
    }

    void UpdateNavMesh(bool asyncUpdate = false)
    {
        NavMeshSourceTag.Collect(ref m_Sources);
        var defaultBuildSettings = NavMesh.GetSettingsByID(1);
        var bounds = QuantizedBounds();
        UpdateBDVertex(bounds);
        
        if (asyncUpdate)
            m_Operation = NavMeshBuilder.UpdateNavMeshDataAsync(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
        else
            NavMeshBuilder.UpdateNavMeshData(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
    }

    void UpdateBDVertex(Bounds bounds)
    {
        var min = bounds.min;
        var max = bounds.max;
        vertex[0] = min;
        vertex[1] = max;
        vertex[2] = new Vector3(min.x, min.y, max.z);
        vertex[3] = new Vector3(min.x, max.y, min.z);
        vertex[4] = new Vector3(max.x, min.y, min.z);
        vertex[5] = new Vector3(min.x, max.y, max.z);
        vertex[6] = new Vector3(max.x, min.y, max.z);
        vertex[7] = new Vector3(max.x, max.y, min.z);
    }

    static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }

    Bounds QuantizedBounds()
    {
        // Quantize the bounds to update only when theres a 10% change in size
        var center = m_Tracked ? m_Tracked.position : transform.position;
        return new Bounds(Quantize(center, 0.1f * m_Size), m_Size);
    }

    void OnDrawGizmosSelected()
    {
        if (m_NavMesh)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(m_NavMesh.sourceBounds.center, m_NavMesh.sourceBounds.size);
        }

        Gizmos.color = Color.yellow;
        var bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        var center = m_Tracked ? m_Tracked.position : transform.position;
        Gizmos.DrawWireCube(center, m_Size);
    }

    public void OnGUI()
    {
        var bounds = QuantizedBounds();
        GUI.Label(new Rect(160, Screen.height - 220, 340, 80), string.Format("<size=30>{0} {1} {2}</size>", bounds.size.x, bounds.size.y, bounds.size.z));
    }

    void OnRenderObject()
    {
        lineMaterial.SetPass(0);
        GL.Color(lineColor);
            
        GL.Begin(GL.LINES);
        GL.Vertex(vertex[5]);
        GL.Vertex(vertex[1]);

        GL.Vertex(vertex[1]);
        GL.Vertex(vertex[7]);

        GL.Vertex(vertex[7]);
        GL.Vertex(vertex[3]);

        GL.Vertex(vertex[3]);
        GL.Vertex(vertex[5]);

        GL.Vertex(vertex[2]);
        GL.Vertex(vertex[6]);

        GL.Vertex(vertex[6]);
        GL.Vertex(vertex[4]);

        GL.Vertex(vertex[4]);
        GL.Vertex(vertex[0]);

        GL.Vertex(vertex[0]);
        GL.Vertex(vertex[2]);

        GL.Vertex(vertex[5]);
        GL.Vertex(vertex[2]);

        GL.Vertex(vertex[1]);
        GL.Vertex(vertex[6]);

        GL.Vertex(vertex[7]);
        GL.Vertex(vertex[4]);

        GL.Vertex(vertex[3]);
        GL.Vertex(vertex[0]);
        
        GL.End();
    }
}
