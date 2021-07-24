using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool nextlv = false;
    private AudioSource audiosource;
    public AudioClip backGround;
    public AudioClip gameOver;
    public AudioClip win;
    public GameObject pnlFail;
    private bool isnext;


    public GameObject pnlLevel;
    public int level = 1;
    public Text txtlevel;
   


    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().canMove = false;

        level = PlayerPrefs.GetInt("level");
        txtlevel.text = "Level " + (level + 1).ToString();
        pnlLevel.SetActive(true);

        StartCoroutine(Setlevel(pnlLevel));
        Time.timeScale = 1;
        isnext = false;
        pnlFail.SetActive(false);
        audiosource = gameObject.GetComponent<AudioSource>();
        audiosource.clip = backGround;
        audiosource.loop = true;
        audiosource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] virus = GameObject.FindGameObjectsWithTag("virus");
        if (virus.Length == 0 && !isnext)
        {
            isnext = true;
            NextLevel();
        }
        if(pnlFail.active == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
            }
        }
    }
    IEnumerator Setlevel(GameObject pnlLevel)
    {
        yield return new WaitForSeconds(2);
        pnlLevel.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().canMove = true;
    }
    public void EndGame()
    {
        PlayerPrefs.SetInt("level", 0);
        Time.timeScale = 0;
        pnlFail.SetActive(true);
        audiosource.clip = gameOver;
        audiosource.Play();
        audiosource.loop = false;

    }
    public void ResetGame()
    {
        GameObject[] virus = GameObject.FindGameObjectsWithTag("virus");
        foreach(GameObject v in virus)
        {
            v.transform.position = v.GetComponent<VirusScript>().oldPosition;
            v.GetComponent<VirusScript>().Start();
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = player.GetComponent<PlayerMove>().oldPosition;
        player.GetComponent<PlayerMove>().Start();

    }
    public void NextLevel()
    {
        PlayerPrefs.SetInt("level", level + 1);
        nextlv = true;
        audiosource.clip = win;
        audiosource.Play();
        audiosource.loop = false;
    }
}
