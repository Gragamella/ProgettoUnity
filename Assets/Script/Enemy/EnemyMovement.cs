using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public float jump = 20f;

    public float followCounter = 0;

    public float stoppingDistance = 0.3f;
    public float jumpTimer = 2f;

    Animator enemyAnim;

    Rigidbody2D rb;

    Vector2 movement;

    public GameObject player;

    public Transform attPos;

    private EnemyBehavior comportamento;

    public float force = 3f;

    public Collider2D oggettoColpito;

    Collider2D scudo;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        comportamento = GetComponent<EnemyBehavior>();
        scudo = gameObject.transform.GetChild(6).GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) > 0.4f && jumpTimer >= 2)
        {
            comportamento.state = EnemyBehavior.State.Jump;
        }

        switch (comportamento.state)
        {
            default:
            case EnemyBehavior.State.Idle:               
                if ((Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < 1f)) {
                    comportamento.state = EnemyBehavior.State.Following;
                }
                speed = 0;
                enemyAnim.SetFloat("speed", speed);
                break;
            case EnemyBehavior.State.Following:
                followCounter ++;
                speed = 0.8f;
                enemyAnim.SetFloat("speed", speed);
                if (player.transform.position.x < gameObject.transform.position.x)
                {
                    if(Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 0.2f)
                    {
                        speed = 0;
                    }
                    else
                    {
                        speed = 0.8f;
                        movement = new Vector2(-1, 0);
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                    }
                    
                }
                else
                {
                    if (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 0.2f)
                    {
                        speed = 0;
                    }
                    else
                    {
                        speed = 0.8f;
                        movement = new Vector2(1, 0);
                        gameObject.transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
                break;
            case EnemyBehavior.State.Attack:
                speed = 0;               
                enemyAnim.SetFloat("speed", speed);
                enemyAnim.SetBool("Attack", true);
                break;
            case EnemyBehavior.State.Jump:
                if(followCounter > 0)
                {
                    rb.AddForce(new Vector2(0, 1f) * jump, ForceMode2D.Impulse);
                    jumpTimer = 0;
                }
                else
                {
                    comportamento.state = EnemyBehavior.State.Idle;
                }           
                break;
        }
      
        if(jumpTimer < 2)
        {
            jumpTimer += Time.deltaTime;
            comportamento.state = EnemyBehavior.State.Following;
        }
        if (Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < stoppingDistance && (Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) < 0.4f))
        {
            comportamento.state = EnemyBehavior.State.Attack;
        }


    }

    


    private void FixedUpdate()
    {
       
        if (comportamento.state == EnemyBehavior.State.Following)
        {
            moveObject(movement);
        }
    }

    void moveObject(Vector2 direzione)
    {
        rb.position = ((Vector2)transform.position + (direzione * speed * Time.deltaTime));
    }


    public void ColpoDiSpada(int contatore, Collider2D oggetto, Vector2 p)
    {
        if (contatore == 0)
        {
            if (oggetto.GetComponentInParent<Enemy>() != null)
            {
               
                if (p.x > oggetto.transform.position.x)
                {
                    oggetto.attachedRigidbody.AddForce(new Vector3(-1.5f, 0f, 0) * force, ForceMode2D.Impulse);
                    oggetto.attachedRigidbody.AddTorque(10);
                }
                else
                {                   
                    oggetto.attachedRigidbody.AddForce(new Vector3(1.5f, 0f, 0) * force, ForceMode2D.Impulse);
                    oggetto.attachedRigidbody.AddTorque(-10);
                }
            }
            else
            {
                if (p.x > oggetto.transform.position.x)
                {
                    oggetto.attachedRigidbody.AddForce(new Vector3(-1.5f, 0f, 0) * force, ForceMode2D.Impulse);
                    oggetto.attachedRigidbody.AddTorque(10);
                }
                else
                {
                    oggetto.attachedRigidbody.AddForce(new Vector3(1.5f, 0f, 0) * force, ForceMode2D.Impulse);
                    oggetto.attachedRigidbody.AddTorque(-10);
                }
            }
        }
        else
        {
            if (p.x > oggetto.transform.position.x)
            {
                oggetto.attachedRigidbody.AddForce(new Vector3(-1.5f, 0, 0) * force, ForceMode2D.Impulse);
                oggetto.attachedRigidbody.AddTorque(10);
            }
            else
            {
                oggetto.attachedRigidbody.AddForce(new Vector3(1.5f, 0, 0) * force, ForceMode2D.Impulse);
                oggetto.attachedRigidbody.AddTorque(-10);
            }
        }
    }

    private void Attack()
    {
        Vector2 objectPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        int count = 0;
        Collider2D[] swordStrike = Physics2D.OverlapBoxAll(attPos.position, attPos.localScale, Vector2.Angle(Vector2.zero, transform.position));
        foreach (Collider2D collider in swordStrike)
        {
            if (collider.tag == "Scudo" && collider.gameObject != this.gameObject)
            {
                count++;
            }
            else if (collider.tag == "Player" && collider.gameObject != this.gameObject)
            {
                oggettoColpito = collider;
            }

            if (collider.tag == "Mobile" && collider.gameObject != this.gameObject)
            {
                if (gameObject.transform.position.x > collider.transform.position.x)
                {
                    collider.attachedRigidbody.AddForce(new Vector3(-1, 1, 0) * force, ForceMode2D.Impulse);
                    collider.attachedRigidbody.AddTorque(10);
                }
                else
                {
                    collider.attachedRigidbody.AddForce(new Vector3(1, 1, 0) * force, ForceMode2D.Impulse);
                    collider.attachedRigidbody.AddTorque(-10);
                }
            }
        }

        ColpoDiSpada(count, oggettoColpito, objectPos);
        oggettoColpito = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(attPos.position, attPos.localScale);
    }

    private void stopAtt()
    {
        enemyAnim.SetBool("Attack", false);
        if (Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) > stoppingDistance)
        {
            comportamento.state = EnemyBehavior.State.Following;
        }
    }

    private void deactivateShied()
    {
        scudo.enabled = false;
    }

    private void activateShied()
    {
        scudo.enabled = true;
    }

}
