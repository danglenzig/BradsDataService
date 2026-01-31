using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Matrix5x3 : MonoBehaviour, IMatrix
{
    [SerializeField] private Node[] row0 = new Node[3];
    [SerializeField] private Node[] row1 = new Node[3];
    [SerializeField] private Node[] row2 = new Node[3];
    [SerializeField] private Node[] row3 = new Node[3];
    [SerializeField] private Node[] row4 = new Node[3];

    private List<Node> nodesRow0;
    private List<Node> nodesRow1;
    private List<Node> nodesRow2;
    private List<Node> nodesRow3;
    private List<Node> nodesRow4;
    private List<List<Node>> matrix;

    private Dictionary<string, Node> nodeDict = new();

    private void Awake()
    {
        nodesRow0 = row0.ToList<Node>();
        nodesRow1 = row1.ToList<Node>();
        nodesRow2 = row2.ToList<Node>();
        nodesRow3 = row3.ToList<Node>();
        nodesRow4 = row4.ToList<Node>();

        matrix.Add(nodesRow0);
        matrix.Add(nodesRow1);
        matrix.Add(nodesRow2);
        matrix.Add(nodesRow3);
        matrix.Add(nodesRow4);
    }


    public void InitializeNodes()
    {
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 2; column++)
            {
                List<string> upstreamNodeIDs = new();
                Node thisNode = matrix[row][column];
                nodeDict[thisNode.NodeID] = thisNode;
                List<int> upstreamRows = GetUpstreamRows(row);
                foreach(int upRow in upstreamRows)
                {
                    Node upNode = matrix[upRow][column + 1];
                    thisNode.UpstreamNodeIDs.Add(upNode.NodeID);
                }
            }
        }
    }

    private List<int> GetUpstreamRows(int thisRow)
    {
        List<int> outList = new();
        if (thisRow == 0) // top
        {
            outList.Add(0);
            outList.Add(1);
        }
        else if (thisRow == 4) // bottom
        {
            outList.Add(3);
            outList.Add(4);
        }
        else
        {
            outList.Add(thisRow - 1);
            outList.Add(thisRow);
            outList.Add(thisRow + 1);
        }
        return outList;
    }



}
