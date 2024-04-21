using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public LayerMask wallLayer;
    public float wallJumpForce = 10f;
    private bool isTouchingWall = false;

    public bool isWallSliding;
    public float wallSlidingSpeed = 2f;

    [SerializeField] private Transform wallCheck;

    public GameObject LavaDeath;
    public GameObject DeathPanel;

    public float jumpHeight;
    private Animator _anim;

    private Rigidbody2D r2d;

    private float lavaSpeed = 1f;

    private Rigidbody2D spikes;

    private bool isDead = false;

    public Text ScoreText;
    public Text DeathScoreText;

    public int Score = 0;
    public int HighScore = 0;

    public string difficult;

    public AudioSource source;
    public AudioClip[] music;
    public AudioClip jump;
    public AudioClip gameOver;

    // Start is called before the first frame update
    void Start()
    {
        var rnd = Random.Range(1, 4);

        switch (rnd)
        {
            case 1:
                source.clip = music[0];
                source.Play();
                break;
            case 2:
                source.clip = music[1];
                source.Play();
                break;
            case 3:
                source.clip = music[2];
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

        if (PlayerPrefs.HasKey("Highscore"))
        {
            HighScore = PlayerPrefs.GetInt("Highscore");
        }
        else
        {
            HighScore = 0;
        }

        StartCoroutine(scoreAdd());

        _anim = GetComponent<Animator>();
        spikes = GameObject.Find("Lava").GetComponent<Rigidbody2D>();
        r2d = GetComponent<Rigidbody2D>();
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(0.2f, 0.5f), wallLayer);
    }

    private void WallSlide()
    {
        if (isTouchingWall == true)
        {
            isWallSliding = true;
            r2d.velocity = new Vector2(r2d.velocity.x, Mathf.Clamp(r2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void FixedUpdate()
    {
        spikes.velocity = new Vector3(0, lavaSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = Score.ToString();
        DeathScoreText.text = "Score: " + Score.ToString() + "\nHighscore: " + HighScore.ToString();

        if(difficult == "Easy")
        {
            if (Score > 20)
            {
                lavaSpeed = 2f;
            }
            else if (Score > 40)
            {
                lavaSpeed = 3f;
            }
            else if (Score > 80)
            {
                lavaSpeed = 4f;
            }
        }
        else if(difficult == "Middle")
        {
            if (Score > 20)
            {
                lavaSpeed = 3f;
            }
            else if (Score > 40)
            {
                lavaSpeed = 4f;
            }
            else if (Score > 80)
            {
                lavaSpeed = 5f;
            }
        }
        else if (difficult == "Hard")
        {
            if (Score > 20)
            {
                lavaSpeed = 4f;
            }
            else if (Score > 40)
            {
                lavaSpeed = 5f;
            }
            else if (Score > 80)
            {
                lavaSpeed = 6f;
            }
        }

        WallSlide();

        RaycastHit2D wallHitLeft = Physics2D.Raycast(transform.position, Vector2.left, 1f, wallLayer);
        RaycastHit2D wallHitRight = Physics2D.Raycast(transform.position, Vector2.right, 1f, wallLayer);

        if (wallHitLeft.collider != null || wallHitRight.collider != null)
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall = false;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && isTouchingWall)
            {
                source.PlayOneShot(jump);
                _anim.Play("Monkey_Jump");
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
                if (wallHitLeft.collider != null)
                {
                    r2d.AddForce(new Vector2(wallJumpForce, 0)); // Push to the right when wall is on the left
                    GameObject.Find("Wall Check").transform.localPosition = new Vector3(1.1f, 0, 0);
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (wallHitRight.collider != null)
                {
                    r2d.AddForce(new Vector2(-wallJumpForce, 0)); // Push to the left when wall is on the right
                    GameObject.Find("Wall Check").transform.localPosition = new Vector3(-1.1f, 0, 0);
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && isTouchingWall)
        {
            source.PlayOneShot(jump);
            _anim.Play("Monkey_Jump");
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            if (wallHitLeft.collider != null)
            {
                r2d.AddForce(new Vector2(wallJumpForce, 0)); // Push to the right when wall is on the left
                GameObject.Find("Wall Check").transform.localPosition = new Vector3(1.1f, 0, 0);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (wallHitRight.collider != null)
            {
                r2d.AddForce(new Vector2(-wallJumpForce, 0)); // Push to the left when wall is on the right
                GameObject.Find("Wall Check").transform.localPosition = new Vector3(-1.1f, 0, 0);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Lava")
        {
            OnDead();
        }
    }

    public void OnDead()
    {
        isDead = true;
        source.clip = null;
        source.PlayOneShot(gameOver);
        StartCoroutine(lavaDeathWait());
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        lavaSpeed = 0;
        StopCoroutine(scoreAdd());
        LavaDeath.SetActive(true);
        LavaDeath.GetComponent<Animator>().Play("LavaDeath");
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt("Highscore", HighScore);
        }
    }

    IEnumerator lavaDeathWait()
    {
        yield return new WaitForSeconds(1.35f);
        DeathPanel.SetActive(true);
    }

    public void OnBGPressed()
    {
        Time.timeScale = 1;
        GameObject.Find("BG_2").SetActive(false);
        GameObject.Find("Press").SetActive(false);
    }

    IEnumerator scoreAdd()
    {
        if(isDead == false)
        {
            Score += 1;
            yield return new WaitForSeconds(1f);
            StartCoroutine(scoreAdd());
        }
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
