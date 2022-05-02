using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float healt = 100;

    int counter = 0;

    public GameObject testa;
    public GameObject busto;
    public GameObject gambe;
    public GameObject oggetto1;

    public GameObject bottoMorte;

    public GameObject giocatore;

    Transform head;
    Transform chest;
    Transform leg;
    Transform item1;

    Transform morteDx;
    Transform morteSx;

    SpriteRenderer EnemySpriteRenderer;
    

    PolygonCollider2D enemyCollider;
    

    // Start is called before the first frame update
    void Start()
    {
        EnemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<PolygonCollider2D>();

        head = gameObject.transform.GetChild(0).GetComponent<Transform>();
        chest = gameObject.transform.GetChild(1).GetComponent<Transform>();
        leg = gameObject.transform.GetChild(2).GetComponent<Transform>();
        item1 = gameObject.transform.GetChild(3).GetComponent<Transform>();
        morteDx = gameObject.transform.GetChild(4).GetComponent<Transform>();
        morteSx = gameObject.transform.GetChild(5).GetComponent<Transform>();


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
                Instantiate(testa, head);
                Instantiate(busto, chest);
                Instantiate(gambe, leg);
                Instantiate(oggetto1, item1);

                if(giocatore.transform.position.x > gameObject.transform.position.x)
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

}
