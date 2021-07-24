using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 4;
   public Vector2 direct = Vector2.zero;

    private Vector2 nextdirect;

    private Node currentNode, targetNode, previusNode;

    public Vector2 oldPosition;

    Animator ani;
    public bool canMove =false;
    public void Start()
    {
        oldPosition = transform.position;
        ani = GetComponent<Animator>();
        ani.SetFloat("move", 0);
        Node node = GetNodeAtPos(transform.localPosition);
        if (node != null)
        {
            currentNode = node;
        }
        direct = Vector2.right;
        ChangePos(direct);

    }

    // Update is called once per frame
    void Update()
    {
        CheckInputAndMove();
        Animation();
    }
    void CheckInputAndMove()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
           ChangePos(Vector3.right);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            ChangePos(Vector3.left);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            ChangePos(Vector3.up);
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            ChangePos(Vector3.down);
        }


        if(targetNode !=currentNode && targetNode != null)
        {
            if(nextdirect ==-1* direct)
            {
                direct *= -1;
                Node temp = targetNode;
                targetNode = previusNode;
                previusNode = temp;
            }
            if(OvershotTarget())
            {
                currentNode = targetNode;
                Node Movetonode = CanMove(nextdirect);

                if (Movetonode != null)
                    direct = nextdirect;
                if (Movetonode == null)
                    Movetonode = CanMove(direct);
                if (Movetonode != null)
                {
                    targetNode = Movetonode;
                    previusNode = currentNode;
                    currentNode = null;
                }
                else
                {
                    direct = Vector2.zero;
                }

            }
            else
            {
                if (canMove)
                {
                    transform.localPosition += (Vector3)(direct) * speed * Time.deltaTime;
                }
            }
        }

    }    
    Node GetNodeAtPos(Vector2 pos)
    {
        int x = pos.x > 0 ? (int)pos.x : (int)pos.x * (-2) + 1;
        int y = pos.y > 0 ? (int)pos.y : (int)pos.y * (-2) + 1;

        GameObject tile = GameObject.Find("GameController").GetComponent<GameBoard>().board[x,y];
      
        if(tile != null)
        {
            return tile.GetComponent<Node>();
        }
        return null;
    }
    Node CanMove(Vector2 d)
    {
        Node MovetoNode = null;
        for(int i = 0; i< currentNode.neighbors.Length; i++)
        {
            if(currentNode.direct[i] == d)
            {
                MovetoNode = currentNode.neighbors[i];
                break;
            }
        }
        return MovetoNode;
        
    }
  
    void ChangePos(Vector2 d)
    {
        if (d != direct) nextdirect = d;
        if (currentNode != null)
        {
            Node MovetoNode = CanMove(d);
            if (MovetoNode != null)
            {
                direct = d;
                targetNode = MovetoNode;
                previusNode = currentNode;
                currentNode = null;
            }
        }
    }
    float LenghtFromNode(Vector2 TargetPos)
    {
        Vector2 temp = TargetPos - (Vector2)previusNode.transform.position;
        return temp.sqrMagnitude;
    }
    bool OvershotTarget()
    {
        float nodetoTarget = LenghtFromNode(targetNode.transform.position);
        float nodetoSelf = LenghtFromNode(transform.position);
        return nodetoSelf > nodetoTarget;
    }
    void Animation()
    {
        if (direct == Vector2.up) ani.SetFloat("move", 1f);
        else if (direct == Vector2.down) ani.SetFloat("move", 0.25f);
        else if (direct == Vector2.left) ani.SetFloat("move", 0.5f);
        else if (direct == Vector2.right) ani.SetFloat("move", 0.75f);
        else ani.SetFloat("move", 0);
    }
}
