using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject winner;
    private GameObject gameController;
    public Image countStar;
    public Sprite star0;
    public Sprite star1;
    public Sprite star2;
    public Sprite star3;

    public GameObject starParticle;
    public Text screenTime;
    public Text screenMedicine;
    private bool check = true;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        winner.SetActive(false);
       // Instantiate(starParticle, transform.position, transform.rotation);

    }

    // Update is called once per frame
    void Update()
    {

        if(gameController.GetComponent<GameController>().nextlv || Input.GetKeyDown(KeyCode.Z))
        {

            winner.SetActive(true);
            Time.timeScale = 0;
        }
        if (winner.active == true && check == true)
        {
            Debug.Log(transform.rotation);
            Instantiate(starParticle, transform.position, transform.rotation);
            check = false;
        }
        UpdateStar();
        ScreeMedicine();
        ScreenTime();
    }
    public void NextLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    public void UpdateStar()
    {
        

        float time = gameController.GetComponent<GameUI>().timeControl;
        if (time <= 60)
        {
            countStar.GetComponent<Image>().sprite = star3;
        }
        else if (time <= 100 && time > 60)
        {
            countStar.GetComponent<Image>().sprite = star2;
        }
        else if (time <= 150 && time > 100)
        {
            countStar.GetComponent<Image>().sprite = star1;
        }
        else
        {
            countStar.GetComponent<Image>().sprite = star0;
        }
    }
    public void ScreenTime()
    {
        float timeControl = gameController.GetComponent<GameUI>().timeControl;
        int temp = Mathf.RoundToInt(timeControl);
        int minute = temp / 60;
        int seconds = temp - minute * 60;
        if (minute < 10)
        {
            if (seconds < 10)
            {
               screenTime.text = "0" + minute.ToString() + " : " + "0" + seconds.ToString();
            }
            else
            {

               screenTime.text = "0" + minute.ToString() + " : " + seconds.ToString();
            }
        }
        else
        {
            if (seconds < 10)
            {
               screenTime.text = minute.ToString() + " : " + "0" + seconds.ToString();
            }
            else
            {

               screenTime.text = minute.ToString() + " : " + seconds.ToString();
            }
        }
    }
    public void ScreeMedicine()
    {
        screenMedicine.text = "X " + GameObject.FindGameObjectWithTag("Player").GetComponent<TakeMedicine>().countMedicine.ToString();
    }
}
