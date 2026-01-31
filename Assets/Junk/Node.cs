using UnityEngine;
using System.Collections.Generic;



public class Node : MonoBehaviour
{
    [SerializeField] private string nodeID;
    private bool selectable = false;
    private List<string> upstreamNodeIDs = new();
    private List<string> connectedUpstreamNodeIDs = new();

    private bool _visible = true;
    private bool visible
    {
        get => _visible;
        set
        {
            if (value == _visible) return;
            _visible = value;
            GetComponent<SpriteRenderer>().enabled = _visible;
        }
    }

    public string NodeID { get => nodeID; }
    public List<string> UpstreamNodeIDs { get => upstreamNodeIDs; }
    public List<string> ConnectedUpstreamNodeIDs { get => connectedUpstreamNodeIDs; }
}
