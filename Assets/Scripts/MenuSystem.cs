using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    public string difficult = "Easy";
    public Text difText;
    public Image difButton;

    public AudioSource source;
    public AudioClip[] clip;
    
    void Start()
    {
        var rnd = Random.Range(1, 4);

        switch (rnd)
        {
            case 1:
                source.clip = clip[0];
                source.Play();
                break;
            case 2:
                source.clip = clip[1];
                source.Play();
                break;
            case 3:
                source.clip = clip[2];
                source.Play();
                break;
        }

        if (PlayerPrefs.HasKey("Difficult"))
        {
            difficult = PlayerPrefs.GetString("Difficult");
        }
        else
        {
            difficult = "Easy";
        }

        if (difficult == "Easy")
        {
            difficult = "Middle";
            difText.text = "Middle";
            difButton.color = new Color(1f, 0.9607843f, 0.4784314f, 1f);
            PlayerPrefs.SetString("Difficult", difficult);
        }
        else if (difficult == "Middle")
        {
            difficult = "Hard";
            difText.text = "Hard";
            difButton.color = new Color(0.9098039f, 0.1372549f, 0.2156863f, 1f);
            PlayerPrefs.SetString("Difficult", difficult);
        }
        else if (difficult == "Hard")
        {
            difficult = "Easy";
            difText.text = "Easy";
            difButton.color = new Color(0.172549f, 0.7882353f, 0.3058824f, 1f);
            PlayerPrefs.SetString("Difficult", difficult);
        }
    }

    void Update()
    {
        
    }

    public void OnClickDifficult()
    {
        if(difficult == "Easy")
        {
            difficult = "Middle";
            difText.text = "Middle";
            difButton.color = new Color(1f, 0.9607843f, 0.4784314f, 1f);
            PlayerPrefs.SetString("Difficult", difficult);
        }
        else if(difficult == "Middle")
        {
            difficult = "Hard";
            difText.text = "Hard";
            difButton.color = new Color(0.9098039f, 0.1372549f, 0.2156863f, 1f);
            PlayerPrefs.SetString("Difficult", difficult);
        }
        else if(difficult == "Hard")
        {
            difficult = "Easy";
            difText.text = "Easy";
            difButton.color = new Color(0.172549f, 0.7882353f, 0.3058824f, 1f);
            PlayerPrefs.SetString("Difficult", difficult);
        }
    }

    public void OnPlay()
    {
        SceneManager.LoadScene(1);
    }
}
