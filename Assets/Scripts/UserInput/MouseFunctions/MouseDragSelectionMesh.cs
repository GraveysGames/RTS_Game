using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MouseDragSelectionMesh : MonoBehaviour
{

    //Stores the selected objects into a diction by there:
        //Key: instance ID 
        //Values: Gameobject pointer
    private Dictionary<int, GameObject> selectedUnits;

    private MeshFilter meshFilter;
    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        selectedUnits = new Dictionary<int, GameObject>();

        meshFilter = gameObject.AddComponent<MeshFilter>();

    }



    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        //Debug.Log(other.gameObject.name);

        //Check if it has Object_Info if so then its an interactable object other wise it isnt
        if (other.gameObject.GetComponent<Object_Info>() == null)
        {
            return;
        }


        if (!selectedUnits.ContainsKey(other.gameObject.GetComponent<CapsuleCollider>().GetInstanceID()))
        {
            selectedUnits.Add(other.gameObject.GetComponent<CapsuleCollider>().GetInstanceID(), other.gameObject);
        }

        
    }

    public void SelectionBox(Vector3 cameraPosition, Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight)
    {
        Vector3[] vertices = new Vector3[5]
        {
            cameraPosition,
            topLeft,
            topRight,
            bottomLeft,
            bottomRight
        };

        SetMesh(vertices);

        //return selectedUnits;

    }

    private void SetMesh(Vector3[] vertices)
    {
        //Debug.Log(vertices[0] + " " + vertices[1] + " " + vertices[2] + " " + vertices[3] + " " + vertices[4]);

        if (mesh == null)
        {
            mesh = new Mesh();
            return;
        }

        mesh.Clear();
        mesh.vertices = vertices;

        int[] tris = new int[18]
        {

            0, 1, 2,

            0, 3, 1,

            0, 4, 3,

            0, 2, 4,

            1, 3, 2,

            2, 3, 4

        };
        mesh.triangles = tris;

        mesh.RecalculateNormals();

        Vector2[] uv = new Vector2[5]
        {
            new Vector2(0, 0),
            new Vector2(0.5f, 0),
            new Vector2(0, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0, 0)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;


        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

    }


    public Dictionary<int, GameObject> GetSelectedUnits()
    {
        return selectedUnits;
    }


    public void DestroySelectionBox()
    {
        GameObject.Destroy(this);
    }

}
