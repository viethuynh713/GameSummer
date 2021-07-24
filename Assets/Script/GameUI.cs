using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Text time;
    public float timeControl = 0;
    public Image Star0;
    public Image Star1;
    public Image Star2;

    public Text Medicines;
    public Image lives1;
    public Image lives0;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().canMove)
        {
            timeControl += Time.deltaTime;
        }
        ScreenTime();
        UpdataStar();
        UpdateLives();
        Medicines.text = "X " + GameObject.FindGameObjectWithTag("Player").GetComponent<TakeMedicine>().countMedicine.ToString();
    }
    void ScreenTime()
    {
        int temp = Mathf.RoundToInt(timeControl);
        int minute = temp / 60;
        int seconds = temp - minute * 60;
        if(minute<10)
        {
            if(seconds<10)
            {
                time.text = "0" + minute.ToString() + " : " + "0" + seconds.ToString();
            }
            else
            {

                time.text = "0" + minute.ToString() + " : "  + seconds.ToString();
            }
        }
        else
        {
            if (seconds < 10)
            {
                time.text = minute.ToString() + " : " + "0" + seconds.ToString();
            }
            else
            {

                time.text = minute.ToString() + " : " + seconds.ToString();
            }
        }
    }
    void UpdataStar()
    {
        if(timeControl>60)
            Star2.enabled = false;
        if(timeControl>100)
            Star1.enabled = false;
        if(timeControl>150)
            Star0.enabled = false;

    }
    void UpdateLives()
    {
        int life = GameObject.FindGameObjectWithTag("Player").GetComponent<TakeMedicine>().life;
        if(life == 2)
        {
            lives1.enabled = false;
        }
        if (life == 1)
        {
            lives0.enabled = false;
        }
    }
}
