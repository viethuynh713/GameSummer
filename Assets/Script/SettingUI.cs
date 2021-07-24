using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pnlSetting;


    private AudioSource audioSourceSound;
    private AudioSource audioSource;

    private float volumeBeat = 1;
    private float volume = 1;

    public Slider sliderSound;
    public Slider sliderMusic;
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSourceSound = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        pnlSetting.SetActive(false);

        audioSourceSound.Play();
        audioSource.Play();
        volume = PlayerPrefs.GetFloat("volume");
        volumeBeat = PlayerPrefs.GetFloat("volumeBeat");
        audioSource.volume = volume;
        sliderMusic.value = -volume;
        audioSourceSound.volume = volumeBeat;
        sliderSound.value = -volumeBeat;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volume;
        audioSourceSound.volume = volumeBeat;
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volumeBeat", volumeBeat);
    }
    public void ClickIconSeeting()
    {
        Time.timeScale = 0;
        pnlSetting.SetActive(true);
    }
    public void Resume()
    {
        pnlSetting.SetActive(false);
        Time.timeScale = 1;
    }
    public void Home()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        ResetVolume();
        PlayerPrefs.DeleteKey("level");
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void SetVolume(float volume)
    {
        this.volume = -volume;
    }
    public void SetVolumeBeat(float volume)
    {
        this.volumeBeat = -volume;
    }
    public void FullSceen(bool t)
    {
        Screen.fullScreen = t;
    }
    public void ResetVolume()
    {
        PlayerPrefs.DeleteKey("volume");
        PlayerPrefs.DeleteKey("volumeBeat");
        audioSource.volume = 1;
        sliderMusic.value = -1;
        audioSourceSound.volume = 1;
        sliderSound.value = -1;
    }
}
