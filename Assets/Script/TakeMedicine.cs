using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeMedicine : MonoBehaviour
{
    // Start is called before the first frame update
    public int life = 3;
    public int countMedicine = 0;
    public bool isfullMedicine = false;
    public bool isFaceMask = false;
    private AudioSource audioSource;
    public AudioClip die;
    public AudioClip takeMedicine;
    public AudioClip killVirus;
    public AudioClip power;
    public GameObject boom;

    public GameObject protect;
    public GameObject particlePower;

    public GameObject gameContrler;
    void Start()
    {
        protect.SetActive(false);
        particlePower.SetActive(false);
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = takeMedicine;
        gameContrler = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        if (isfullMedicine)
        {
            GameObject[] t = GameObject.FindGameObjectsWithTag("virus");
            foreach (GameObject o in t)
            {
                o.GetComponent<VirusScript>().ChangeModeToFri();
            }
        }
        GameObject[] medicines = GameObject.FindGameObjectsWithTag("medicine");
        if (medicines.Length == 0)
            isfullMedicine = true;
        if(isfullMedicine)
        {
            particlePower.SetActive(true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "medicine")
        {
            countMedicine++;
            Destroy(collision.gameObject);
            audioSource.clip = takeMedicine; 
            audioSource.Play();
        }


        if(collision.collider.tag == "facemask")
        {
            isFaceMask = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().canMove = false;
            protect.SetActive(true);
            protect.GetComponent<SpriteRenderer>().enabled = true;
            GameObject[] t = GameObject.FindGameObjectsWithTag("virus");
            foreach(GameObject o in t)
            {
                o.GetComponent<VirusScript>().ChangeModeToFri();
            }
            Destroy(collision.gameObject);
            audioSource.clip = power;
            audioSource.Play();
            StartCoroutine(Deylay());
        }
        
    }
    IEnumerator Deylay()
    {
        yield return new WaitForSeconds(1);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().canMove = true;
    }    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "virus")
        {

            if (isfullMedicine)
            {
                Destroy(collision.gameObject);
                Instantiate(boom, collision.transform.position, collision.transform.rotation);
                audioSource.clip = killVirus;
                audioSource.Play();
            }
            else if (!isFaceMask && !isfullMedicine)
            {
                life--;
                gameContrler.GetComponent<GameController>().ResetGame();
                audioSource.clip = die;
                audioSource.Play();
            }
            if(life == 0)
            {
                gameContrler.GetComponent<GameController>().EndGame();
            }

        }
    }
}
