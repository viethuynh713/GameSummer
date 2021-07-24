using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    // Start is called before the first frame update
    private static int width = 60;
    private static int height = 35;
    public GameObject[,] board = new GameObject[width, height];

    void Start()
    {
        Object[] objects = GameObject.FindGameObjectsWithTag("node");
        foreach(GameObject o in objects)
        {
            Vector2 pos = o.transform.position;
            int x = pos.x > 0 ? (int)pos.x : (int)pos.x * (-2) + 1;
            int y = pos.y > 0 ? (int)pos.y : (int)pos.y * (-2) + 1;
               
            board[x,y] = o;
            
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
