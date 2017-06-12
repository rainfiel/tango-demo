
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MeshLoader : MonoBehaviour {
    public string path;
    public GameObject agentRes;
    public bool wireframe = true;
    
    private MeshRenderer m_meshRenderer;
    // Use this for initialization
    void Start ()
    {
        NavMeshModifier modifier = GetComponent<NavMeshModifier>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        if (m_meshRenderer != null)
        {
            m_meshRenderer.enabled = false;
        }

        StreamReader sw = new StreamReader(path, Encoding.ASCII);
        
        var buf = new BinaryReader(sw.BaseStream, Encoding.ASCII);
        sw.BaseStream.Position = 0;
        
        var length = sw.BaseStream.Length;
        var idx = 0;
        while (length > 0)
        {
            var s = buf.ReadInt32();
            var b = buf.ReadBytes(s);
            length -= (s + 4);
            var mesh = MeshSerializer.ReadMesh(b);
            var extents = mesh.bounds.extents;
            if (extents.x > 0 && extents.y > 0 && extents.z > 0 && !Enumerable.All(mesh.triangles, v=>v==0))
            {

                GameObject newObj = new GameObject();
                newObj.transform.parent = transform;
                newObj.name = "mesh_" + idx;

                if (wireframe)
                    newObj.AddComponent<Wireframe>();

                MeshFilter meshFilter = newObj.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;
                meshFilter.name = newObj.name;

                MeshRenderer meshRenderer = newObj.AddComponent<MeshRenderer>();
                meshRenderer.shadowCastingMode = m_meshRenderer.shadowCastingMode;
                meshRenderer.receiveShadows = m_meshRenderer.receiveShadows;
                meshRenderer.sharedMaterials = m_meshRenderer.sharedMaterials;
                meshRenderer.reflectionProbeUsage = m_meshRenderer.reflectionProbeUsage;
                meshRenderer.probeAnchor = m_meshRenderer.probeAnchor;

                NavMeshModifier mod = newObj.AddComponent<NavMeshModifier>();
                mod.area = modifier.area;
                mod.m_AffectedAgents = modifier.m_AffectedAgents;
                
                var collider = newObj.AddComponent<MeshCollider>();
                collider.sharedMesh = meshFilter.mesh;
            }
            idx++;
        }
        Debug.Log("read done:" + length + " " + sw.BaseStream.Length);

        var surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
        
        GameObject instance = Instantiate(agentRes) as GameObject;
        instance.transform.parent = transform;
    }
    
    // Update is called once per frame
    void Update () {
		
	}
}
