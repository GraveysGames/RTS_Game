using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNetwork : MonoBehaviour
{
    public static NodeNetwork current;

    private Nodes[,] mapNodes;

    private Vector3[,] walkableGrid;
    private Vector3[,] terrainGrid;

    private float xInterval;
    private float yInterval;

    public bool DrawGizmos;

    public List<Vector3> GizmosDrawPath;

    private Vector3 GizmoDrawPosition;
    private Vector3 GizmoDrawMousePosition;

    Nodes[,] nodeMatrix;

    private void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (terrainGrid != null)
        {
            Debug.Log("terrainGrid 1,1:" + terrainGrid[1, 1]);

            Debug.Log("xInterval: " + xInterval);

            Debug.Log("yInterval: " + yInterval);
        }

        if (walkableGrid != null)
        {
            Debug.Log("walkableGrid 0,0:" + walkableGrid[0, 0]);
        }
        */
    }

    public void OnDrawGizmos()
    {
        if (DrawGizmos == false)
        {
            return;
        }

        for (int x = 0; x < mapNodes.GetLength(0); x++)
        {
            for (int y = 0; y < mapNodes.GetLength(1); y++)
            {
                if (mapNodes[x,y].IsWalkable)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(mapNodes[x, y].WorldPosition, 1);
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                //Gizmos.DrawSphere(mapNodes[x, y].WorldPosition, 1);

            }
        }

        foreach (Vector3 position in GizmosDrawPath)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(position, 1.1f);
        }

    }

    public void SetGridVariables(Vector3[,] terrainGrid, Vector3[,] walkableGrid)
    {

        this.walkableGrid = walkableGrid;
        this.terrainGrid = terrainGrid;

        yInterval = (terrainGrid[(terrainGrid.GetLength(0)-1), (terrainGrid.GetLength(1) - 1)].x / (terrainGrid.GetLength(0) - 1));

        xInterval = (terrainGrid[(terrainGrid.GetLength(0) - 1), (terrainGrid.GetLength(1) - 1)].x / (terrainGrid.GetLength(1) - 1));

        Component GetNodes = GetComponent<GetNodesFromTerrain>();
        Destroy(GetNodes);

        FillNodeNetwork();

        DrawGizmos = true;
    }

    public Vector3 FindNearestGridPointToPosition(Vector3 mouseInput)
    {

        float xIndexAproximation = mouseInput.z / xInterval;
        float yIndexAproximation = mouseInput.x / yInterval;

        int xTruncatedIndex = (int)xIndexAproximation;
        int yTruncatedIndex = (int)yIndexAproximation;

        if ((xIndexAproximation % 1) > 0.5f)
        {
            if ((yIndexAproximation % 1) > 0.5f)
            {
                return terrainGrid[xTruncatedIndex + 1, yTruncatedIndex + 1];  
            }
            else
            {
                return terrainGrid[xTruncatedIndex + 1, yTruncatedIndex];
            }
        }
        else
        {
            if ((yIndexAproximation % 1) > 0.5f)
            {
                return terrainGrid[xTruncatedIndex, yTruncatedIndex + 1];
            }
            else
            {
                return terrainGrid[xTruncatedIndex, yTruncatedIndex];
            }
        }

    }

    private Nodes FindNearestNodeToPosition(Vector3 position)
    {

        GizmoDrawMousePosition = position;

        float xIndexAproximation = position.z / xInterval;
        float yIndexAproximation = position.x / yInterval;

        int xTruncatedIndex = (int)xIndexAproximation;
        int yTruncatedIndex = (int)yIndexAproximation;

        if ((xIndexAproximation % 1) > 0.5f)
        {
            if ((yIndexAproximation % 1) > 0.5f)
            {
                return current.mapNodes[xTruncatedIndex + 1, yTruncatedIndex + 1];
            }
            else
            {
                return current.mapNodes[xTruncatedIndex + 1, yTruncatedIndex];
            }
        }
        else
        {
            if ((yIndexAproximation % 1) > 0.5f)
            {
                return current.mapNodes[xTruncatedIndex, yTruncatedIndex + 1];
            }
            else
            {
                return current.mapNodes[xTruncatedIndex, yTruncatedIndex];
            }
        }
    }

    private void FillNodeNetwork()
    {
        mapNodes = new Nodes[walkableGrid.GetLength(0), walkableGrid.GetLength(1)];

        for (int x = 0; x < walkableGrid.GetLength(0); x++)
        {
            for (int y = 0; y < walkableGrid.GetLength(1); y++)
            {
                if (walkableGrid[x,y] != Vector3.zero)
                {
                    mapNodes[x, y] = new Nodes((x,y), walkableGrid[x,y], true);
                }
                else
                {
                    mapNodes[x, y] = new Nodes((x, y), terrainGrid[x, y], false);
                }

                mapNodes[x, y].SetNeighbors(mapNodes);

            }
        }


    }


    #region pathing algo
    public List<Vector3> GetPath(Vector3 currentPosition, Vector3 movePosition)
    {
        GizmosDrawPath.Clear();
        ResetAllVisited();
        List<Vector3> path = new List<Vector3>();

        Nodes startingNode = FindNearestNodeToPosition(currentPosition);
        Nodes endingNode = FindNearestNodeToPosition(movePosition);

        List<Nodes> nodePath;

        if (endingNode.IsWalkable == false)
        {
            nodePath = findPath(startingNode, endingNode, true);
        }
        else
        {
            nodePath = findPath(startingNode, endingNode, false);
        }

        if (nodePath.Count < 1)
        {
            return path;
        }

        foreach (Nodes node in nodePath)
        {
            path.Add(node.WorldPosition);
        }

        if (endingNode.IsWalkable == true)
        {
            path.Add(endingNode.WorldPosition);
        }

        if (endingNode.IsWalkable == false)
        {
            if (nodePath.Count < 3)
            {
                return new();
            }
            path.RemoveAt(0);
        }

        //remove
        path.RemoveAt(0);
        path.RemoveAt(path.Count-1);

        if (endingNode.IsWalkable == true)
        {
            path.Add(movePosition);
        }


        GizmosDrawPath = new List<Vector3>();
        GizmosDrawPath.Add(currentPosition);
        GizmosDrawPath.AddRange(path);

        


        return path;
    }

    private void ResetVisited(List<Nodes> path)
    {
        foreach (Nodes node in path)
        {
            node.visited = false;
        }
    }

    private void ResetAllVisited()
    {
        for (int x = 0; x < mapNodes.GetLength(0); x++)
        {
            for (int y = 0; y < mapNodes.GetLength(1); y++)
            {
                mapNodes[x, y].visited = false;
            }
        }
    }

    private List<Nodes> findPath(Nodes startingNode, Nodes ending, bool endOutOfBounds)
    {

        startingNode.visited = true;
        List<Nodes> path = new List<Nodes>();

        path.Add(startingNode);

        List<(Nodes node, float cost)> nodes = new List<(Nodes node, float cost)>();

        nodes.Add((startingNode, 0f));

        //List<(Nodes node, float cost)> nodes = startingNode.GetNeighbors(ending.WorldPosition);
        int count = 0;
        while (nodes.Count != 0)
        {
            int index = 0;
            (Nodes node, float cost) holdingLowest = (null, float.MaxValue);
            for (int i = 0;  i < nodes.Count; i++)
            {
                if (nodes[i].cost < holdingLowest.cost)
                {
                    index = i;
                    holdingLowest = nodes[i];
                }
            }
            
            Nodes currentNode = holdingLowest.node;
            currentNode.visited = true;
            nodes.RemoveAt(index);
            path.Add(currentNode);

            if (endOutOfBounds)
            {
                if (Vector3.Distance(currentNode.WorldPosition, ending.WorldPosition) > Vector3.Distance(path[path.Count-2].WorldPosition, ending.WorldPosition))
                {
                    return path;
                }
            }

            if (currentNode == ending)
            {
                //Debug.Log("found end");
                path.RemoveAt(0);
                path.RemoveAt(path.Count-1);
                return path;
            }

            nodes.AddRange(currentNode.GetNeighbors(ending.WorldPosition));

            if (count == 100)
            {
                return path;
            }
            else
            {
                count++;
            }

        }

        //Debug.Log("Couldnt find end");
        return path;
        
    }

    #endregion


    public void BuildingBuilt(Vector3 centerNodeCords, (int w, int h) buildingSize)
    {
        List<Nodes> nodesUnderBuilding = GetNodesUnderBuilding(centerNodeCords, buildingSize);
        foreach (Nodes node in nodesUnderBuilding)
        {
            node.IsWalkable = false;
        }
    }

    public List<Vector3> NodesAroundBuilding(Vector3 centerNodeCords, (int w, int h) buildingSize)
    {
        List<Nodes> nodesUnderBuilding = GetNodesUnderBuilding(centerNodeCords, buildingSize);
        List<Nodes> nodesAroundBuilding = new List<Nodes>();

        foreach (Nodes node in nodesUnderBuilding)
        {
            List<Nodes> neighbors = node.Neighbors;
            foreach (Nodes neighbor in neighbors)
            {
                if (neighbor.IsWalkable)
                {
                    nodesAroundBuilding.Add(neighbor);
                }
            }
        }

        List<Vector3> vectorsAroundBuilding = new();

        foreach (Nodes node in nodesAroundBuilding)
        {
            vectorsAroundBuilding.Add(node.WorldPosition);
        }


        return vectorsAroundBuilding;
    }

    private List<Nodes> GetNodesUnderBuilding(Vector3 centerNodeCords, (int w, int h) buildingSize)
    {
        List<Nodes> nodesUnderBuilding = new();
        Nodes centerNode = FindNearestNodeToPosition(centerNodeCords);

        nodesUnderBuilding.Add(centerNode);

        (int x, int y) globalNodeCord = centerNode.MatrixPosition;

        for (int i = -(buildingSize.w / 2); i < (buildingSize.w / 2) + 1; i++)
        {
            for (int j = -(buildingSize.h / 2); j < (buildingSize.h / 2) + 1; j++)
            {
                nodesUnderBuilding.Add(mapNodes[globalNodeCord.x + i, globalNodeCord.y + j]);
            }
        }
        return nodesUnderBuilding;
    }

    public Vector3 AddUnitToNodeGraph(Vector3 position, Vector3 oldPosition)
    {
        Nodes nearestNode = FindNearestNodeToPosition(position);

        if (nearestNode.WorldPosition == oldPosition)
        {
            nearestNode.IsWalkable = false;
            return Constants.current.rayCastMiss;
        }
        else
        {
            FindNearestNodeToPosition(oldPosition).IsWalkable = true;
            nearestNode.IsWalkable = false;
            return nearestNode.WorldPosition;
        }
    }

    private class Nodes
    {

        (int x, int y) matrixPosition;
        Vector3 worldPosition;

        bool isWalkable;

        Nodes northNeighbor;
        Nodes eastNeighbor;
        Nodes southNeighbor;
        Nodes westNeighbor;

        Nodes northEastNeighbor;
        Nodes northWestNeighbor;
        Nodes southEastNeighbor;
        Nodes southWestNeighbor;

        List<Nodes> neighbors;
        public List<Nodes> Neighbors { get => (neighbors = new List<Nodes>() {northEastNeighbor, northWestNeighbor, northNeighbor, southEastNeighbor, southWestNeighbor, southNeighbor, eastNeighbor, westNeighbor }); }

        public bool visited;


        public (int x, int y) MatrixPosition { get => matrixPosition; }
        public Vector3 WorldPosition { get => worldPosition; }
        public bool IsWalkable { get => isWalkable; set => isWalkable = value; }

        public Nodes((int x, int y) matrixPosition, Vector3 worldPosition)
        {
            this.matrixPosition = matrixPosition;
            this.worldPosition = worldPosition;

            visited = false;

            //SetNeighbors();
        }

        public Nodes((int x, int y) matrixPosition, Vector3 worldPosition, bool walkable)
        {
            this.matrixPosition = matrixPosition;
            this.worldPosition = worldPosition;

            this.isWalkable = walkable;

            visited = false;
        }

        public void SetNeighbors(Nodes[,] nodeMatrix)
        {

            if (matrixPosition.x > 0)
            {
                if (nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y)] != null)
                {
                    nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y)].eastNeighbor = this;
                    westNeighbor = nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y)];
                }

                if (matrixPosition.y < (nodeMatrix.GetLength(1) - 1))
                {
                    if (nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y + 1)] != null)
                    {
                        nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y + 1)].southEastNeighbor = this;
                        northWestNeighbor = nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y + 1)];
                    }
                }


            }

            if (matrixPosition.y > 0)
            {
                if (nodeMatrix[(matrixPosition.x), (matrixPosition.y - 1)] != null)
                {
                    nodeMatrix[(matrixPosition.x), (matrixPosition.y - 1)].northNeighbor = this;
                    southNeighbor = nodeMatrix[(matrixPosition.x), (matrixPosition.y - 1)];
                }

                if (matrixPosition.x > 0)
                {
                    if (nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y - 1)] != null)
                    {
                        nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y - 1)].northEastNeighbor = this;
                        southWestNeighbor = nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y - 1)];
                    }
                }

            }

        }

        public List<(Nodes node, float cost)> GetNeighbors(Vector3 endPosition)
        {

            List<(Nodes node, float cost)> neighbors = new List<(Nodes node, float cost)>();
            (Nodes node, float cost) holdNeighbor;

            holdNeighbor = GetNeighbor(northNeighbor, endPosition);

            if (holdNeighbor.node != null)
            {
                neighbors.Add(holdNeighbor);
            }

            holdNeighbor = GetNeighbor(eastNeighbor, endPosition);

            if (holdNeighbor.node != null)
            {
                neighbors.Add(holdNeighbor);
            }

            holdNeighbor = GetNeighbor(southNeighbor, endPosition);

            if (holdNeighbor.node != null)
            {
                neighbors.Add(holdNeighbor);
            }

            holdNeighbor = GetNeighbor(westNeighbor, endPosition);

            if (holdNeighbor.node != null)
            {
                neighbors.Add(holdNeighbor);
            }

            holdNeighbor = GetNeighbor(northEastNeighbor, endPosition);

            if (holdNeighbor.node != null)
            {
                neighbors.Add(holdNeighbor);
            }

            holdNeighbor = GetNeighbor(northWestNeighbor, endPosition);

            if (holdNeighbor.node != null)
            {
                neighbors.Add(holdNeighbor);
            }

            holdNeighbor = GetNeighbor(southEastNeighbor, endPosition);

            if (holdNeighbor.node != null)
            {
                neighbors.Add(holdNeighbor);
            }

            holdNeighbor = GetNeighbor(southWestNeighbor, endPosition);

            if (holdNeighbor.node != null)
            {
                neighbors.Add(holdNeighbor);
            }



            return neighbors;
        }

        private (Nodes node, float cost) GetNeighbor(Nodes neighbor, Vector3 endPosition)
        {
            if ((neighbor != null) && (neighbor.isWalkable == true) && (neighbor.visited == false))
            {
                return (neighbor, Vector3.Distance(neighbor.WorldPosition, endPosition));
            }
            else
            {
                return (null, float.MaxValue);
            }
        }
    }

}
