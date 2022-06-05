using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Enemy
{
    private GameObject enemyObj;
    private Enemy enemy;

    Animator enemyAnim;

    Rigidbody2D rb;

    Vector2 movement;

    private EnemyBehavior comportamento;

    public Collider2D oggettoColpito;

   

    public EnemyMovement(GameObject _enemyObject)
    {
        enemyObj = _enemyObject;
        enemy = enemyObj.GetComponent<Enemy>();
        rb = enemyObj.GetComponent<Rigidbody2D>();
        enemyAnim = enemyObj.GetComponent<Animator>();
        comportamento = enemyObj.GetComponent<EnemyBehavior>();
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

       

    }

    
    public void MovementEnemy()
    {
        movement = new Vector2();
        if (Mathf.Abs(enemyObj.transform.position.y - enemy.giocatore.transform.position.y) > 0.4f && enemy.jumpTimer >= 2)
        {
            comportamento.state = EnemyBehavior.State.Jump;
        }

        switch (comportamento.state)
        {
            default:
            case EnemyBehavior.State.Idle:
                if ((Mathf.Abs(enemy.giocatore.transform.position.x - enemyObj.transform.position.x) < 1f))
                {
                    comportamento.state = EnemyBehavior.State.Following;
                }
                enemy.speed = 0;
                enemyAnim.Play(enemy.enemyAnimationDict["enemy_idle"]);
                break;
            case EnemyBehavior.State.Following:
                enemy.follow = true;
                enemy.speed = 5f;
                enemyAnim.Play(enemy.enemyAnimationDict["enemy_walk"]);

                if (enemy.giocatore.transform.position.x < enemyObj.transform.position.x)
                {
                    if (Mathf.Abs(enemyObj.transform.position.x - enemy.giocatore.transform.position.x) < 0.2f)
                    {
                        enemy.speed = 0;
                    }
                    else
                    {
                        enemy.speed = 5f;
                        movement = new Vector2(-1, 0);
                        enemyObj.transform.localScale = new Vector3(1, 1, 1);
                    }

                }
                else
                {
                    if (Mathf.Abs(enemyObj.transform.position.x - enemy.giocatore.transform.position.x) < 0.2f)
                    {
                        enemy.speed = 0;
                    }
                    else
                    {
                        enemy.speed = 5f;
                        movement = new Vector2(1, 0);
                        enemyObj.transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
                break;
            case EnemyBehavior.State.Attack:
                enemy.speed = 0;
                enemyAnim.Play(enemy.enemyAnimationDict["enemy_attack"]);             
                break;
            case EnemyBehavior.State.Jump:
                if (enemy.follow)
                {
                    rb.AddForce(new Vector2(0, 1f) * enemy.jump, ForceMode2D.Impulse);
                    enemy.jumpTimer = 0;
                }
                else
                {
                    comportamento.state = EnemyBehavior.State.Idle;
                }
                break;
        }

        if (enemy.jumpTimer < 2 && comportamento.state != EnemyBehavior.State.Idle)
        {
            enemy.jumpTimer += Time.deltaTime;
            comportamento.state = EnemyBehavior.State.Following;
        }
        if (Mathf.Abs(enemy.giocatore.transform.position.x - enemyObj.transform.position.x) < enemy.stoppingDistance && (Mathf.Abs(enemyObj.transform.position.y - enemy.giocatore.transform.position.y) < 0.4f))
        {
            comportamento.state = EnemyBehavior.State.Attack;
        }

        if (comportamento.state == EnemyBehavior.State.Following)
        {
            moveObject(movement);
        }

    }

    void moveObject(Vector2 direzione)
    {
        rb.position = ((Vector2)enemyObj.transform.position + (direzione * enemy.speed * Time.deltaTime));
    }


   

}
