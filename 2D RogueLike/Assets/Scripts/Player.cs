using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    //Dissolving Controls
    Material material; //Get dissolve Material
    bool isDissolving; //Check if dissolving true||false
    float fade = 1f;

    public int wallDamage = 1;
    public int enemyDamage = 2;
    public int pointsPerFood = 10;
    public float restartLevelDelay = 1f;
    public Text foodText;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip moveSound3;
    public AudioClip moveSound4;
    public AudioClip drinkSound1;
    public AudioClip mineSound1;
    public AudioClip mineSound2;
    public AudioClip gameOverSound;
    public AudioSource hurtSound;
    public float audioTime = 0.5f;

    private bool facingRight = true;
    private Animator animator;
    private int food = 100;

    protected override void Start ()
    {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        foodText.text = "Food " + food;

        GetComponent<AudioSource>().time = GetComponent<AudioSource>().clip.length * audioTime;
        material = GetComponent<SpriteRenderer>().material; //at start, get dissolve material
        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;
        Vector3 characterScale = transform.localScale;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            characterScale.x = -1;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            characterScale.x = 1;
        }

        transform.localScale = characterScale;

        if (horizontal != 0) //prevents player from moving horizontally
        {
            vertical = 0;
            Debug.Log("Cant Move");
        }
        if (horizontal !=0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical); //this expects player will hit a wall.
        }

        if (isDissolving) //Dissolve Character
        {
            fade -= Time.deltaTime;
            if (fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;
            }
            material.SetFloat("_Fade", fade);
        }

        //if (Input.GetKey(KeyCode.A))
        //{
           // food = food - 150;
       // }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food " + food;

        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        if(Move(xDir,yDir,out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2, moveSound3, moveSound4);
            animator.SetTrigger("dwarfWalk");
        }

        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }


    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage); //player will attack wall
        SoundManager.instance.RandomizeSfx(mineSound1, mineSound2);
        animator.SetTrigger("dwarfMine");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit") //restarts level when reaching goal
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (collision.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood  + " Food: " +food;
            SoundManager.instance.RandomizeSfx(drinkSound1);
            collision.gameObject.SetActive(false);//deletes the food on pickup
        }
    }

    private void Restart() //will restart Main level
    {
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("dwarfHurt");
        hurtSound.Play();
        food -= loss;
        foodText.text="-" + loss + " Food: " + food;
        CheckIfGameOver();
    }
    

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            isDissolving = true;
            Invoke("PlayerGameOver", 2f);
            SoundManager.instance.RandomizeSfx(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            
        }
            
    }

    private void PlayerGameOver()
    {
        GameManager.instance.GameOver();
    }
}
