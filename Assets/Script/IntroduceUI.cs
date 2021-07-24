using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroduceUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] pnlIntro;
    private int i;
    void Start()
    {
        pnlIntro[0].SetActive(true);
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Space))
        {
            i++;
            if (i == pnlIntro.Length)
                SkipButton();
            else
            {
                pnlIntro[i].SetActive(true);
                pnlIntro[i-1].SetActive(false);
            }
        }
    } 
    public void SkipButton()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }
}
