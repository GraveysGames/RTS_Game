using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNodeNetwork : MonoBehaviour
{

    [SerializeField] GameObject groundPlane;

    List<Vector3> groundPlaneGlobalVerticies;


    Dictionary<Vector3, Node> Nodes;

    private class Node
    {
        Vector3 position;
        List<Node> adjacentNodes;

        public Node(Vector3 position)
        {
            this.position = position;
        }

        public void AddNode(Node adjacentNode)
        {
            adjacentNodes.Add(adjacentNode);
        }

    }


    // Start is called before the first frame update
    void Start()
    {

        GetPlaneVerticies();

    }


    private void GetPlaneVerticies()
    {
        List<Vector3> groundPlaneLocalVerticies = new List<Vector3>();
        groundPlaneGlobalVerticies = new List<Vector3>();

        groundPlaneLocalVerticies = new List<Vector3>(groundPlane.GetComponent<MeshFilter>().mesh.vertices);


        foreach (Vector3 point in groundPlaneLocalVerticies)
        {
            groundPlaneGlobalVerticies.Add(groundPlane.transform.TransformPoint(point));
        }

    }


    private void OnDrawGizmos()
    {
        if (groundPlaneGlobalVerticies == null)
        {
            return;
        }

        foreach (Vector3 point in groundPlaneGlobalVerticies)
        {
            Gizmos.DrawSphere(point, 0.4f);
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
