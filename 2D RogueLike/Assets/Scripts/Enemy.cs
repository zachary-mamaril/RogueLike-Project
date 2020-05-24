using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;
    private Animator animator;
    private Transform target;
    private bool skipMove;
    public AudioSource audio;
    public int health = 2;


    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this); //Gamemanager will now be able to call the MoveEnemy function in the Enemy Class;

        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove) //enemy will move every other turn
        {
            skipMove = false;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy() //move to the player
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) //if enemy is in the same column as player
            yDir = target.position.y > transform.position.y ? 1 : -1; // will move up, if not, will move down
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitplayer = component as Player;

        animator.SetTrigger("New Trigger"); //unity wont let me save a parameter name sooooo...
        audio.Play();

        hitplayer.LoseFood(playerDamage);
    }

    public void DamageEnemy(int loss)
    {
        //spriteRenderer.sprite = dmgSprite;
        health -= loss;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }

    }
}
