using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusScript : MonoBehaviour
{
    // Start is called before the first frame update
    protected float moveSpeed;
    protected float frightenedModeMoveSpeed;
    protected float previousMoveSpeed;

    protected float scatterModeTimeMin = 10;
    protected float scatterModeTimeMax = 12;
    protected float chaseModeTime = 20f;
    protected float frightenedModeTime;

    private float modeChangeTime = 0;
    private float modeChangeTimeToFri = 0;

    public enum Mode
    {
        Scatter,
        Chase,
        Fringtened
    }
    
    Mode previousMode;

    Mode currentMode;
    private GameObject player;
    

    private Node currentNode, targetNode, previousNode;

    private Vector2 direct, prevDirect;

    public enum VirusType
    {
        Red,
        Blue,
        Yellow
    }
    public VirusType virustype;
    private Animator ani;
    public Node ScatterNode;

    public Vector2 oldPosition;
    protected float rateSpawn = 20;
    public GameObject[] viruss;
    public float timeSpawn = 10;
    public float t;

    public void Start()
    {
        moveSpeed = 3f;
        frightenedModeTime = 10;
    GameObject[] nodes = GameObject.FindGameObjectsWithTag("node");
        if(ScatterNode == null)
        {
            ScatterNode = GetNodeAtPos(nodes[Random.RandomRange(0, nodes.Length)].transform.position);
        }

        oldPosition = transform.position;
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Node node = GetNodeAtPos(transform.localPosition);
        if (node != null)
        {
            currentNode = node;
        }
        previousNode = currentNode;
        targetNode = ChooseNextNode();
        //direct = Vector2.left;
        currentMode = Mode.Scatter;


        moveSpeed += moveSpeed * PlayerPrefs.GetInt("level") / 10;
        frightenedModeMoveSpeed = (float)(0.6 * moveSpeed);

        frightenedModeTime -= frightenedModeTime * PlayerPrefs.GetInt("level") / 10;
        if (frightenedModeTime < 3) frightenedModeTime = 3;

        rateSpawn += rateSpawn * PlayerPrefs.GetInt("level") / 10;
        if (rateSpawn > 80)
            rateSpawn = 80;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMode();
        Move();
        SetAinmation();
        SpawnEnemy();

    }
    Node GetNodeAtPos(Vector2 pos)
    {
        int x = pos.x > 0 ? Mathf.RoundToInt(pos.x) : Mathf.RoundToInt(pos.x) * (-2) + 1;
        int y = pos.y > 0 ? Mathf.RoundToInt(pos.y) : Mathf.RoundToInt(pos.y) * (-2) + 1;

        GameObject tile = GameObject.Find("GameController").GetComponent<GameBoard>().board[x, y];
        if (tile != null)
        {
            return tile.GetComponent<Node>();
        }
        return null;
    }

    public void ChangeModeToFri()
    {
        modeChangeTimeToFri = 0;
        ChangeMode(Mode.Fringtened);
    }
    void ChangeMode(Mode m)
    {
        if(currentMode == Mode.Fringtened)
        {
            moveSpeed = previousMoveSpeed;
        }
        if(m == Mode.Fringtened)
        {
            previousMoveSpeed = moveSpeed;
            moveSpeed = frightenedModeMoveSpeed;
        }
        if (currentMode != m)
        {
            previousMode = currentMode;
            currentMode = m;
        }
    }
    void UpdateMode()
    {
        if (currentMode != Mode.Fringtened)
        {
            modeChangeTime += Time.deltaTime;
            float scatterModeTime = Random.RandomRange(scatterModeTimeMin, scatterModeTimeMax);
            
                if (modeChangeTime > scatterModeTime && currentMode == Mode.Scatter)
                {
                    modeChangeTime = 0;
                    ChangeMode(Mode.Chase);
                }
                if (modeChangeTime > chaseModeTime && currentMode == Mode.Chase)
                {
                    modeChangeTime = 0;
                    ChangeMode(Mode.Scatter);

                }
            
           
        }
        else if (currentMode == Mode.Fringtened)
        {
            modeChangeTimeToFri += Time.deltaTime;
            GameObject p = player.GetComponent<TakeMedicine>().protect;
            if (modeChangeTimeToFri > 0.7 * frightenedModeTime && modeChangeTimeToFri < frightenedModeTime)
            {
                StartCoroutine(Protect(p));
            }
            if(modeChangeTimeToFri > frightenedModeTime)
            {
                
                modeChangeTimeToFri = 0;
                ChangeMode(previousMode);
                player.GetComponent<TakeMedicine>().isFaceMask = false;
                p.SetActive(false);
            }
           
        }
    }
    IEnumerator Protect(GameObject p)
    {
       
        yield return new WaitForSeconds(1f);
        
        p.GetComponent<SpriteRenderer>().enabled = !p.GetComponent<SpriteRenderer>().enabled;
        
    }
    float LenghtFromNode(Vector2 TargetPos)
    {
        if (TargetPos == null) Debug.Log("Target");
        if (previousNode == null)
        {
            Debug.Log("Pre");
            Destroy(gameObject);
        }
        Vector2 temp = TargetPos - (Vector2)previousNode.transform.position;
        return temp.sqrMagnitude;
    }
    bool OvershotTarget()
    {
        float nodetoTarget = LenghtFromNode((Vector2)targetNode.transform.position);
        float nodetoSelf = LenghtFromNode((Vector2)transform.position);
        return nodetoSelf >= nodetoTarget;
    }
    Vector2 GetTargetTile()
    {
        //chia 3 lớp
        // Lớp 1 : Khoảng cách lớn hơn
        // Lớp 2 : càng gần càng chính xác
        // Lớp 3 : Hướng vào Player
        Vector2 targetTile = Vector2.zero;

        Vector2 PlayerPos = player.transform.position;
        Vector2 DirectionPlayer = player.GetComponent<PlayerMove>().direct;
        if (virustype == VirusType.Red)
        {
            targetTile = new Vector2(Mathf.RoundToInt(PlayerPos.x), Mathf.RoundToInt(PlayerPos.y));
        }
        if(virustype == VirusType.Blue)
        {
            targetTile = new Vector2(Mathf.RoundToInt(PlayerPos.x), Mathf.RoundToInt(PlayerPos.y));
            targetTile += 4 * DirectionPlayer;
        }
        if(virustype == VirusType.Yellow)
        {
            targetTile = new Vector2(Mathf.RoundToInt(PlayerPos.x), Mathf.RoundToInt(PlayerPos.y));
            targetTile -= 4 * DirectionPlayer;
        }
        return targetTile;
    }
    Node ChooseNextNode()
    {
        Vector2 targetTile = Vector2.zero;

        if (currentMode == Mode.Chase)
            targetTile = GetTargetTile();
        else if (currentMode == Mode.Scatter)
            targetTile = ScatterNode.transform.position;
        else if(currentMode == Mode.Fringtened)
        {
            GameObject[] nodes = GameObject.FindGameObjectsWithTag("node");
            foreach (GameObject n in nodes)
            {
                if (Vector2.Distance(player.transform.position,n.transform.position)>12f)
                {
                    targetTile =(Vector2)n.transform.position;
                }
            }
        }

        Node moveToNode = null;

        float leastDistance = 100000f;
        for (int i = 0; i < currentNode.neighbors.Length; i++)
        {
            float distance = Vector2.Distance(targetTile, currentNode.neighbors[i].transform.position);
            if (distance < leastDistance && prevDirect != currentNode.direct[i]*-1)
            {
                leastDistance = distance;
                moveToNode = currentNode.neighbors[i];
                direct = currentNode.direct[i];

            }
        }
        return moveToNode;
    }
    void Move()
    {
        if (targetNode != currentNode && targetNode != null)
        {

            if (OvershotTarget())
            {
                currentNode = targetNode;
                targetNode = ChooseNextNode();
                previousNode = currentNode;
                prevDirect = direct;
                currentNode = null;
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().canMove)
                {
                    transform.localPosition += (Vector3)direct * moveSpeed * Time.deltaTime;
                }
            }
        }
    }
    void SetAinmation()
    {
        if (direct == Vector2.up) ani.SetFloat("move", 1f);
        else if (direct == Vector2.down) ani.SetFloat("move", 0);
        else if (direct == Vector2.left)
        {
            ani.SetFloat("move", 0.5f);

            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direct == Vector2.right)
        {
            ani.SetFloat("move", 0.5f);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else ani.SetFloat("move", 0);
        
    }

    void SpawnEnemy()
    {
        float rate = Random.RandomRange(0, 100);
        t += Time.deltaTime;
        if(rate < rateSpawn && currentMode != Mode.Fringtened && t > timeSpawn && OvershotTarget()&&GetNodeAtPos(player.transform.position) !=null)
        {
            t = 0;
            GameObject p = Instantiate(viruss[Random.Range(0, viruss.Length)], transform.position,transform.rotation);
            if (p.GetComponent<VirusScript>().ScatterNode == null)
            {
                p.GetComponent<VirusScript>().ScatterNode = ScatterNode;
            }
            if (p.GetComponent<VirusScript>().targetNode== null)
            {
                p.GetComponent<VirusScript>().targetNode = targetNode;
            }

        }
            
    }
   
}    
 

