using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // Start is called before the first frame update
    public Node[] neighbors;
    public Vector2[] direct;
    void Start()
    {
        direct = new Vector2[neighbors.Length];
        for(int i = 0; i<neighbors.Length; i++)
        {
            Node neighbor = neighbors[i];
            Vector2 temp = neighbor.transform.localPosition - transform.localPosition;
            direct[i] = temp.normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
