
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshLoader : MonoBehaviour {
    public string path;

    private MeshRenderer m_meshRenderer;
    // Use this for initialization
    void Start ()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        if (m_meshRenderer != null)
        {
            m_meshRenderer.enabled = false;
        }

        StreamReader sw = new StreamReader(path, Encoding.ASCII);
        
        var buf = new BinaryReader(sw.BaseStream, Encoding.ASCII);
        sw.BaseStream.Position = 0;

        Debug.Log(sw.BaseStream.Length);

        var length = sw.BaseStream.Length;
        while(length > 0)
        {
            var s = buf.ReadInt32();
            var b = buf.ReadBytes(s);
            length -= (s + 4);

            GameObject newObj = new GameObject();
            newObj.transform.parent = transform;
            MeshFilter meshFilter = newObj.AddComponent<MeshFilter>();
            meshFilter.mesh = MeshSerializer.ReadMesh(b);

            MeshRenderer meshRenderer = newObj.AddComponent<MeshRenderer>();
            meshRenderer.shadowCastingMode = m_meshRenderer.shadowCastingMode;
            meshRenderer.receiveShadows = m_meshRenderer.receiveShadows;
            meshRenderer.sharedMaterials = m_meshRenderer.sharedMaterials;
            meshRenderer.reflectionProbeUsage = m_meshRenderer.reflectionProbeUsage;
            meshRenderer.probeAnchor = m_meshRenderer.probeAnchor;
        }
        Debug.Log("read done:" + length + " " + sw.BaseStream.Length);
    }

    void OnPreRender()
    {
        GL.wireframe = true;
    }
    void OnPostRender()
    {
        GL.wireframe = false;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
