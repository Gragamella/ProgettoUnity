using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Enemy : MonoBehaviour
{

    public EnemyAttPicker enemyAtt;
    public EnemyMovement enemyMov;
    public Dictionary<string, string> enemyAnimationDict;
    public SpriteAtlas atlas;
    private EnemyBehavior comportamento;


    public float healt;
    public float speed;
    public float jump;
    public float stoppingDistance;
    public float jumpTimer;
    public float force;

    public int forza;
    public int destrezza;
    public int costituzione;
    public int fortuna;

    int counter = 0;
    public bool follow = false;

    public GameObject testa;
    public GameObject busto;
    public GameObject gambe;
    public GameObject oggetto1;
    public GameObject armaEquip;

    public GameObject bottoMorte;

    public GameObject giocatore;

    Transform head;
    Transform chest;
    Transform leg;
    Transform item1;
    Transform arma;
    public Transform attPos;

    Transform morteDx;
    Transform morteSx;

    SpriteRenderer EnemySpriteRenderer;


    BoxCollider2D enemyCollider;
    Collider2D oggettoColpito;
    Collider2D scudo;


    // Start is called before the first frame update
    void Start()
    {
        giocatore = FindObjectOfType<Player>().gameObject;
        atlas = Resources.Load<SpriteAtlas>("Skelly-Atlas");

        enemyAnimationDict = new Dictionary<string, string>();
        enemyAnimationDict.Add("enemy_idle", "");
        enemyAnimationDict.Add("enemy_walk", "");
        enemyAnimationDict.Add("enemy_attack", "");
        enemyMov = new EnemyMovement(gameObject);
        enemyAtt = new EnemyAttPicker(gameObject);
        enemyAtt.assignEnemyAtt();
        
        EnemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<BoxCollider2D>();
        comportamento = GetComponent<EnemyBehavior>();

        head = gameObject.transform.GetChild(0).GetComponent<Transform>();
        chest = gameObject.transform.GetChild(1).GetComponent<Transform>();
        leg = gameObject.transform.GetChild(2).GetComponent<Transform>();
        item1 = gameObject.transform.GetChild(3).GetComponent<Transform>();
        morteDx = gameObject.transform.GetChild(4).GetComponent<Transform>();
        morteSx = gameObject.transform.GetChild(5).GetComponent<Transform>();
        arma = gameObject.transform.GetChild(6).GetComponent<Transform>();
        scudo = gameObject.transform.GetChild(6).GetComponent<BoxCollider2D>();
        attPos = gameObject.transform.GetChild(7).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(healt <= 0)
        {
            EnemySpriteRenderer.enabled = false;
            enemyCollider.enabled = false;

            if(counter < 1)
            {
                testa.GetComponent<SpriteRenderer>().sprite = enemyAtt.spritesMorte[0];
                busto.GetComponent<SpriteRenderer>().sprite = enemyAtt.spritesMorte[1];
                gambe.GetComponent<SpriteRenderer>().sprite = enemyAtt.spritesMorte[2];
                Instantiate(testa, head);
                Instantiate(busto, chest);
                Instantiate(gambe, leg);
                Instantiate(oggetto1, item1);
                armaEquip.SetActive(true);
                Instantiate(armaEquip, arma);
                Destroy(armaEquip);



                if (giocatore.transform.position.x > gameObject.transform.position.x)
                {
                    Instantiate(bottoMorte, morteDx.position, Quaternion.identity);

                }
                else
                {
                    Instantiate(bottoMorte, morteSx.position, Quaternion.identity);
                }
              
                counter++;
            }
            Destroy(gameObject,0.5f);
        }

        if(comportamento.state == EnemyBehavior.State.Attack)
        {
            Invoke("Attack", 0.4f);
            Invoke("stopAtt", 0.6f);
            Invoke("deactivateShied", 0.4f);
            Invoke("activateShied", 0.7f);
        }
        enemyMov.MovementEnemy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
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

    public void Attack()
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

        if (oggettoColpito != null)
        {
            ColpoDiSpada(count, oggettoColpito, objectPos);
            oggettoColpito = null;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(attPos.position, attPos.localScale);
    }

    public void stopAtt()
    {
        if (Mathf.Abs(giocatore.transform.position.x - transform.position.x) > stoppingDistance)
        {
            comportamento.state = EnemyBehavior.State.Following;
        }
    }

    public void deactivateShied()
    {
        scudo.enabled = false;
    }

    public void activateShied()
    {
        scudo.enabled = true;
    }

}
