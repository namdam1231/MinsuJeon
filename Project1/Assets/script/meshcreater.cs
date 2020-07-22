using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshcreater : MonoBehaviour
{
    [SerializeField]
    Texture m_texture;
    Vector3[] m_vertices = new Vector3[] { new Vector3(-1f, 1f, 0f), new Vector3(1f, 1f, 0f), new Vector3(1f, -1f, 0f), new Vector3(-1f, -1f, 0f)};

    int[] m_triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    Mesh m_mesh;
    Vector2[] m_uvs = new Vector2[] { new Vector2(0f,1f), new Vector2(1f,1f), new Vector2(1f,0f), new Vector2(0f,0f)};
    // Start is called before the first frame update
    void Start()
    {
        m_mesh = new Mesh();
        m_mesh.vertices = m_vertices;
        m_mesh.triangles = m_triangles;
        m_mesh.uv = m_uvs;
        m_mesh.RecalculateBounds();
        m_mesh.RecalculateNormals();
        var meshfilter = gameObject.AddComponent<MeshFilter>();
        meshfilter.mesh = m_mesh;

        var meshRen = gameObject.AddComponent<MeshRenderer>();
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.mainTexture = m_texture;
        meshRen.material = mat;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
