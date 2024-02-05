using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNodesFromTerrain : MonoBehaviour
{

    private Terrain terrain;

    [SerializeField] private int yCount;

    [SerializeField] private int xCount;

    [SerializeField] private bool gizmoActive;

    Vector3[,] drawGrid;

    // Start is called before the first frame update
    void Start()
    {
        terrain = this.gameObject.GetComponent<Terrain>();

        Vector3[,] terrainGrid = GetTerrainHeightAtGridPoints();

        Vector3[,] walkableGrid = FilterTerrainPointsByHeight(terrainGrid, 15f);

        walkableGrid = RemoveUnconnectedClusters(walkableGrid);

        drawGrid = walkableGrid;


        //Send Grids to NodeNetwork
        NodeNetwork.current.SetGridVariables(terrainGrid, walkableGrid);

    }

    private void OnDrawGizmos()
    {
        if (drawGrid == null)
        {
            return;
        }
        if (gizmoActive == false)
        {
            return;
        }

        for (int x = 0; x < drawGrid.GetLength(0); x++)
        {
            for (int y = 0; y < drawGrid.GetLength(1); y++)
            {
                float heightAt = drawGrid[x, y].y;

                if (drawGrid[x,y] != Vector3.zero)
                {
                    if (heightAt > 15f)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(drawGrid[x, y], 1);
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(drawGrid[x, y], 1);
                    }
                }

            }
        }
    }


    private Vector3[,] GetTerrainNormalsAtGridPoints()
    {

        Vector3[,] terrainGrid = new Vector3[xCount, yCount];
        Vector3[,] terrainGridNormals = new Vector3[xCount, yCount];

        float[,] terrainGridHeightValues;

        float yInterval = 1f / (float)(yCount - 1);
        float xInterval = 1f / (float)(xCount - 1);
        
        terrainGridHeightValues = terrain.terrainData.GetInterpolatedHeights(0, 0, xCount, yCount, xInterval, yInterval);

        for (int x = 0; x < terrainGridHeightValues.GetLength(0); x++)
        {
            for (int y = 0; y < terrainGridHeightValues.GetLength(1); y++)
            {
                terrainGridNormals[x,y] = terrain.terrainData.GetInterpolatedNormal(x * xInterval,y * yInterval);
                terrainGrid[x, y] = new Vector3(terrain.terrainData.size.z * yInterval * y, terrainGridHeightValues[x, y], terrain.terrainData.size.x * xInterval * x);
            }
        }

        return terrainGrid;
    }

    private Vector3[,] RemoveUnconnectedClusters(Vector3[,] walkableGrid)
    {
        Nodes setMatrixNode = new Nodes((-1,-1), Vector3.zero);
        setMatrixNode.SetNodeMatrixSize(walkableGrid.GetLength(0), walkableGrid.GetLength(1));

        for (int x = 0; x < walkableGrid.GetLength(0); x++)
        {
            for (int y = 0; y < walkableGrid.GetLength(1); y++)
            {
                if (walkableGrid[x,y] != Vector3.zero)
                {
                    Nodes node = new Nodes((x, y), walkableGrid[x, y]);
                    node.AddSelfToNodeMatrix(node, (walkableGrid.GetLength(0), walkableGrid.GetLength(1)));
                }
            }
        }

        List <List<Nodes>> NodeLists = new List<List<Nodes>>();

        for (int x = 0; x < walkableGrid.GetLength(0); x++)
        {
            for (int y = 0; y < walkableGrid.GetLength(1); y++)
            {
                Nodes node = Nodes.GetNodeFromNodesMatrix((x,y));
                if ((node != null) && (node.visited == false))
                {
                    NodeLists.Add(node.TraverseAllConnectedNodes());
                }
            }
        }

        List<Nodes> largestNodeList = new List<Nodes>();

        foreach (List<Nodes> nodeList in NodeLists)
        {
            //Debug.Log(nodeList.Count);
            if (largestNodeList.Count < nodeList.Count)
            {

                for (int n = 0; n < largestNodeList.Count; n++)
                {
                    walkableGrid[largestNodeList[n].MatrixPosition.x, largestNodeList[n].MatrixPosition.y] = Vector3.zero;
                    largestNodeList[n].Dispose();
                    largestNodeList[n] = null;
                }

                largestNodeList = nodeList;
            }
            else
            {
                for (int n = 0; n < nodeList.Count; n++)
                {
                    walkableGrid[nodeList[n].MatrixPosition.x, nodeList[n].MatrixPosition.y] = Vector3.zero;
                    nodeList[n].Dispose();
                    nodeList[n] = null;
                }
            }
        }


        return walkableGrid;
    }


    private class Nodes
    {
        static Nodes[,] nodeMatrix;

        (int x, int y) matrixPosition;
        Vector3 worldPosition;

        Nodes northNeighbor;
        Nodes eastNeighbor;
        Nodes southNeighbor;
        Nodes westNeighbor;

        public bool visited;

        public (int x, int y) MatrixPosition { get => matrixPosition; }

        public Nodes((int x, int y) matrixPosition, Vector3 worldPosition)
        {
            this.matrixPosition = matrixPosition;
            this.worldPosition = worldPosition;

            visited = false;

            SetNeighbors();
        }

        public void Dispose()
        {
            nodeMatrix[matrixPosition.x, matrixPosition.y] = null;
        }

        private void SetNeighbors()
        {

            if (matrixPosition.x > 0)
            {
                if (nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y)] != null)
                {
                    nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y)].eastNeighbor = this;
                    westNeighbor = nodeMatrix[(matrixPosition.x - 1), (matrixPosition.y)];
                }
            }

            if (matrixPosition.y > 0)
            {
                if (nodeMatrix[(matrixPosition.x), (matrixPosition.y - 1)] != null)
                {
                    nodeMatrix[(matrixPosition.x), (matrixPosition.y - 1)].northNeighbor = this;
                    southNeighbor = nodeMatrix[(matrixPosition.x), (matrixPosition.y - 1)];
                }
            }

        }

        public List<Nodes> TraverseAllConnectedNodes()
        {

            List<Nodes> nodesInList = new List<Nodes>();

            nodesInList.Add(this);

            visited = true;

            if ((northNeighbor != null) && (northNeighbor.visited == false))
            {
                nodesInList.AddRange(northNeighbor.TraverseAllConnectedNodes());
            }

            if ((eastNeighbor != null) && (eastNeighbor.visited == false))
            {
                nodesInList.AddRange(eastNeighbor.TraverseAllConnectedNodes());
            }

            if ((southNeighbor != null) && (southNeighbor.visited == false))
            {
                nodesInList.AddRange(southNeighbor.TraverseAllConnectedNodes());
            }

            if ((westNeighbor != null) && (westNeighbor.visited == false))
            {
                nodesInList.AddRange(westNeighbor.TraverseAllConnectedNodes());
            }

            return nodesInList;
        }

        public void SetNodeMatrixSize(int xSize, int ySize)
        {
            if (nodeMatrix != null)
            {
                nodeMatrix = new Nodes[xSize, ySize];
            }
        }

        public static Nodes GetNodeFromNodesMatrix((int x, int y) matrixPosition)
        {
            return nodeMatrix[matrixPosition.x, matrixPosition.y];
        }

        public void AddSelfToNodeMatrix(Nodes thisNode, (int xSize, int ySize) mapSize)
        {
            if (nodeMatrix == null)
            {
                nodeMatrix = new Nodes[mapSize.xSize, mapSize.ySize];
            }
            nodeMatrix[matrixPosition.x, matrixPosition.y] = thisNode;
        }
    }

    //To visit later maybe

    #region cluster Removal
    /*
    private Vector3[,] RemoveUnconnectedClusters(Vector3[,] walkableGrid)
    {
        List<List<(int x, int y)>> listOfAllClusters = new List<List<(int x, int y)>>();

        listOfAllClusters = FindAllClusters(walkableGrid);

        Vector3[,] bestGrid = RemoveClusters( walkableGrid, listOfAllClusters);



        return bestGrid;
    }

    private List<List<(int x, int y)>> FindAllClusters(Vector3[,] walkableGrid)
    {

        List<List<(int x, int y)>> listOfAllClusters = new List<List<(int x, int y)>>();

        bool[,] visited = new bool[walkableGrid.GetLength(0), walkableGrid.GetLength(1)];

        for (int x = 0; x < visited.GetLength(0); x++)
        {
            for (int y = 0; y < visited.GetLength(1); y++)
            {
                visited[x, y] = new bool();
                visited[x, y] = false;
            }
        }

        for (int x = 0; x < walkableGrid.GetLength(0); x++)
        {
            for (int y = 0; y < walkableGrid.GetLength(1); y++)
            {
                if ((walkableGrid[x, y] != Vector3.zero) && (visited[x, y] == false))
                {
                    listOfAllClusters.Add(FindConnectedGridPoints((x, y)));
                }
            }
        }

        return listOfAllClusters;


        List<(int x, int y)> FindConnectedGridPoints((int x, int y) currentGridPoint)
        {
            List<(int x, int y)> connectedGridPoints = new List<(int x, int y)>();

            visited[currentGridPoint.x, currentGridPoint.y] = true;

            connectedGridPoints.Add(currentGridPoint);

            if (currentGridPoint.x < walkableGrid.GetLength(0))
            {
                if ((walkableGrid[currentGridPoint.x + 1, currentGridPoint.y] != Vector3.zero) && (visited[currentGridPoint.x + 1, currentGridPoint.y] == false))
                {
                    connectedGridPoints.AddRange(FindConnectedGridPoints((currentGridPoint.x + 1, currentGridPoint.y)));
                }
            }

            if (currentGridPoint.y < walkableGrid.GetLength(1))
            {
                if ((walkableGrid[currentGridPoint.x, currentGridPoint.y + 1] != Vector3.zero) && (visited[currentGridPoint.x, currentGridPoint.y + 1] == false))
                {
                    connectedGridPoints.AddRange(FindConnectedGridPoints((currentGridPoint.x, currentGridPoint.y + 1)));
                }
            }

            return connectedGridPoints;
        }

    }



    private Vector3[,] RemoveClusters(Vector3[,] walkableGrid, List<List<(int x, int y)>> listOfAllClusters)
    {

        List<(int x, int y)> longestCluster = new List<(int x, int y)>();

        foreach (List<(int x, int y)> cluster in listOfAllClusters)
        {
            if (longestCluster.Count < cluster.Count)
            {
                foreach ((int x, int y) point in longestCluster)
                {
                    walkableGrid[point.x, point.y] = Vector3.zero;
                }

                longestCluster = cluster;

            }
            else
            {
                foreach ((int x, int y) point in cluster)
                {
                    walkableGrid[point.x, point.y] = Vector3.zero;
                }
            }
        }


        return walkableGrid;
    }
    */
    #endregion


    #region By Heights


    private Vector3[,] GetTerrainHeightAtGridPoints()
    {

        Vector3[,] terrainGrid = new Vector3[xCount, yCount];

        float[,] terrainGridHeightValues;

        float yInterval = 1f / (float)(yCount - 1);
        float xInterval = 1f / (float)(xCount - 1);

        terrainGridHeightValues = terrain.terrainData.GetInterpolatedHeights(0, 0, xCount, yCount, xInterval, yInterval);

        for (int x = 0; x < terrainGridHeightValues.GetLength(0); x++)
        {
            for (int y = 0; y < terrainGridHeightValues.GetLength(1); y++)
            {
                terrainGrid[x, y] = new Vector3(terrain.terrainData.size.z * yInterval * y, terrainGridHeightValues[x, y], terrain.terrainData.size.x * xInterval * x);
            }
        }


        return terrainGrid;
    }


    private Vector3[,] FilterTerrainPointsByHeight(Vector3[,] terrainGrid, float height)
    {
        Vector3[,] walkableGrid = null;

        walkableGrid = new Vector3[terrainGrid.GetLength(0), terrainGrid.GetLength(1)];


        for (int x = 0; x < terrainGrid.GetLength(0); x++)
        {
            for (int y = 0; y < terrainGrid.GetLength(1); y++)
            {
                float heightAt = terrainGrid[x, y].y;
                if (heightAt > height)
                {
                    walkableGrid[x, y] = Vector3.zero;
                }
                else
                {
                    walkableGrid[x, y] = terrainGrid[x, y];
                }
            }
        }

        return walkableGrid;
    }


    #endregion

    #region By Normals

    //to use normals instead to visit later
    /*
    private Vector3[,] FilterTerrainPointsByNormals(float normalThreshHold)
    {
        Vector3[,] walkableGrid = null;

        Vector3[,] terrainGrid = GetTerrainGridPoints();

        walkableGrid = new Vector3[terrainGrid.GetLength(0), terrainGrid.GetLength(1)];


        for (int x = 0; x < terrainGrid.GetLength(0); x++)
        {
            for (int y = 0; y < terrainGrid.GetLength(1); y++)
            {
                terrain.terrainData.GetInterpolatedNormal(x,y);
                float heightAt = terrainGrid[x, y].y;
                if (heightAt > height)
                {
                    walkableGrid[x, y] = Vector3.zero;
                }
                else
                {
                    walkableGrid[x, y] = terrainGrid[x, y];
                }
            }
        }

        return walkableGrid;
    }
    */

    #endregion
}
