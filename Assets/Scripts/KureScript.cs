using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class KureScript : MonoBehaviour
{
    // Start is called before the first frame update
    bool Direction,IsStop;
    TextMeshProUGUI ScoreText,BestScoreText;
    int Score;
    GameObject  GameOverPanel, GameName; float Timer;
    Rigidbody rigidbody;
    AudioSource audioSource;
    public AudioClip CoinSound,DirectionSound,Failed;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        Score = 0;
        Direction = true;
        IsStop = false;
        ScoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        BestScoreText = GameObject.Find("BestScoreText").GetComponent<TextMeshProUGUI>();
        BestScoreText.text = "BEST SCORE: " + PlayerPrefs.GetInt("BestScore");

        GameOverPanel = GameObject.Find("GameOverPanel");
        GameOverPanel.SetActive(false);
        GameName = GameObject.Find("GameName");
        Time.timeScale=0;
        ScoreText.gameObject.SetActive(false);
        Timer = 0;
      
        if (!PlayerPrefs.HasKey("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }
    }
  
    // Update is called once per frame
    void Update()
    {
      

        print(Timer);
        GameOver();
        if (!IsStop)
        {
            if (Direction)
            {
                ToForward();
            }
            else
            {
                ToLeft();
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)//  || Input.GetKeyDown(KeyCode.Space)
            {


                Time.timeScale = 1; 
                audioSource.Stop();
                Direction = !Direction;
                Score += 10;
                ScoreText.text = Score.ToString();
                if (Score>PlayerPrefs.GetInt("BestScore"))
                {
                    PlayerPrefs.SetInt("BestScore", Score);

                }
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(DirectionSound);
                }
               
                if (GameName.activeSelf)
                {
                    BestScoreText.gameObject.SetActive(false);
                    GameName.SetActive(false);
                    
                    ScoreText.gameObject.SetActive(true);

                }
            }






            
        }
      
    }
    void GameOver()
    {
        if (transform.position.y < -0.87f)
        {
            IsStop = true;
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(Failed);
            }
            GameOverPanel.SetActive(true);
            BestScoreText.gameObject.SetActive(true);
            BestScoreText.text = "BEST SCORE: " + PlayerPrefs.GetInt("BestScore");
        }

    }
  
  
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
           other.gameObject.SetActive(false);
            Score += 10;
            ScoreText.text = Score.ToString();
            audioSource.PlayOneShot(CoinSound);
        }
        
    }

    void ToForward()
    {
       transform.Translate(new Vector3(-15 * Time.deltaTime, 0, 0));
    

    }
    void ToLeft()
    {
        transform.Translate(new Vector3(0, 0, -15 * Time.deltaTime));
     
    }
    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
}
