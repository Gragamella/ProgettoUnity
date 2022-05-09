using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Transform prova;
    public GameObject papas;

    public CharacterController2D controller;

    public int salute = 100;
    public int attacco = 20;

    public Enemy nemico;
    Collider2D colliderNemico;

    public Animator playerAnim;
    public GameObject player;

    public bool down = false;

    public Transform swordAttPos;
    public Transform UpswordAttPos;
    public Transform DownswordAttPos;
    public Transform crouchAttPos;

    public float crouchAttackRange;
    public float attackRange;

    public float force;

    public Inventory inventario;

    [SerializeField] private InterfacciaInventario interfacciaInventario;
    


    // Start is called before the first frame update
    void Start()
    {
        ItemWorld.SpawnItemWorld(new Vector3(7, 2), new Items { tipoOggetto = Items.ItemType.PozioneSalute, quantità = 2 });
        ItemWorld.SpawnItemWorld(new Vector3(10, 2), new Items { tipoOggetto = Items.ItemType.Monete, quantità = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(12, 2), new Items { tipoOggetto = Items.ItemType.PozioneVelocita, quantità = 1 });
    }

    private void Awake()
    {
        inventario = new Inventory();
        interfacciaInventario.SetInventario(inventario);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            if (interfacciaInventario.gameObject.activeInHierarchy)
            {
                interfacciaInventario.gameObject.SetActive(false);
            }
            else
            {
                interfacciaInventario.gameObject.SetActive(true);
                interfacciaInventario.RefreshInventoryItems();
            }
        }

       
        
            if (Input.GetKeyDown("s") && !controller.m_Grounded)
            {                
                down = true;                     
            }

            if (Input.GetKeyDown("k") && controller.m_Grounded)
            {
                if (gameObject.GetComponent<playerMovement>().crouch)
                {
                    playerAnim.SetBool("crouchAttack", true);
                }
                else if (Input.GetAxisRaw("Vertical") > 0f)
                {
                    playerAnim.SetBool("Attack-UpThrust-Air", true);
                }
                else
                {
                    playerAnim.SetBool("Attack", true);
                }


            }

            if (Input.GetKeyDown("k") && !controller.m_Grounded)
            {
                if (Input.GetAxisRaw("Vertical") > 0f)
                {
                    playerAnim.SetBool("Attack-UpThrust-Air", true);
                }
                else if (down)
                {
                    playerAnim.SetBool("Attack-DownThrust-Air", true);
                }
                else
                {
                    playerAnim.SetBool("Attack", true);
                }
            }
    }

    public void DownAttack()
    {
        Vector2 playerPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        int count = 0;

        Collider2D[] swordStrike = Physics2D.OverlapBoxAll(DownswordAttPos.position, DownswordAttPos.localScale, Vector2.Angle(Vector2.zero, transform.position));
        foreach (Collider2D collider in swordStrike)
        {
            if (collider.tag == "Scudo" && collider.gameObject != this.gameObject)
            {
                count++;
            }
            else if (collider.tag == "Enemies" && collider.gameObject != this.gameObject)
            {
                colliderNemico = collider;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 0.75f, 0) * force, ForceMode2D.Impulse);
            }

            if (collider.tag == "Mobile" && collider.gameObject != this.gameObject)
            {
                if (playerPos.x > collider.transform.position.x)
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

        ColpoDiSpada(count, colliderNemico, playerPos);
        colliderNemico = papas.GetComponent<Collider2D>();
    }


    public void UpAttack()
    {
        Vector2 playerPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        int count = 0;

        Collider2D[] swordStrike = Physics2D.OverlapBoxAll(UpswordAttPos.position, UpswordAttPos.localScale, Vector2.Angle(Vector2.zero, transform.position));
        foreach (Collider2D collider in swordStrike)
        {
            if (collider.tag == "Scudo" && collider.gameObject != this.gameObject)
            {
                count++;
            }
            else if (collider.tag == "Enemies" && collider.gameObject != this.gameObject)
            {
                colliderNemico = collider;
            }

            if (collider.tag == "Mobile" && collider.gameObject != this.gameObject)
            {
                if (playerPos.x > collider.transform.position.x)
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

        ColpoDiSpada(count, colliderNemico, playerPos);
        colliderNemico = papas.GetComponent<Collider2D>();
    }

    public void Attack()
    {
        Vector2 playerPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        int count = 0;

        Collider2D[] swordStrike = Physics2D.OverlapBoxAll(swordAttPos.position, swordAttPos.localScale, Vector2.Angle(Vector2.zero, transform.position));
        foreach (Collider2D collider in swordStrike)
        {
            if(collider.tag == "Scudo" && collider.gameObject != this.gameObject)
            {
                count++;
            }
            else if (collider.tag == "Enemies" && collider.gameObject != this.gameObject)
            {
                colliderNemico = collider;
            }

            if (collider.tag == "Mobile" && collider.gameObject != this.gameObject)
                    {
                        if (playerPos.x > collider.transform.position.x)
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

        ColpoDiSpada(count, colliderNemico, playerPos);
        colliderNemico = papas.GetComponent<Collider2D>();
    }

    public void CrouchAttack()
    {
        Vector2 playerPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        int count = 0;

        Collider2D[] swordStrike = Physics2D.OverlapBoxAll(crouchAttPos.position, crouchAttPos.localScale, Vector2.Angle(Vector2.zero, transform.position));
        foreach (Collider2D collider in swordStrike)
        {
            if (collider.tag == "Scudo" && collider.gameObject != this.gameObject)
            {
                count++;
            }
            else if (collider.tag == "Enemies" && collider.gameObject != this.gameObject)
            {
                colliderNemico = collider;
            }

            if (collider.tag == "Mobile" && collider.gameObject != this.gameObject)
            {
                if (playerPos.x > collider.transform.position.x)
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

        ColpoDiSpada(count, colliderNemico, playerPos);
        colliderNemico = papas.GetComponent<Collider2D>();
    }


    public void ColpoDiSpada(int contatore, Collider2D oggetto, Vector2 p)
    {
        if (contatore == 0)
        {
            if(oggetto.GetComponentInParent<Enemy>() != null)
            {
                nemico = oggetto.GetComponentInParent<Enemy>();
                nemico.healt -= attacco;
                if (p.x > oggetto.transform.position.x)
                {
                    oggetto.attachedRigidbody.AddForce(new Vector3(-1.5f, 1, 0) * force, ForceMode2D.Impulse);
                    oggetto.attachedRigidbody.AddTorque(10);
                }
                else
                {
                    oggetto.attachedRigidbody.AddForce(new Vector3(1.5f, 1, 0) * force, ForceMode2D.Impulse);
                    oggetto.attachedRigidbody.AddTorque(-10);
                }
            }
            else
            {
                if (p.x > oggetto.transform.position.x)
                {
                    oggetto.attachedRigidbody.AddForce(new Vector3(-1.5f, 1, 0) * force, ForceMode2D.Impulse);
                    oggetto.attachedRigidbody.AddTorque(10);
                }
                else
                {
                    oggetto.attachedRigidbody.AddForce(new Vector3(1.5f, 1, 0) * force, ForceMode2D.Impulse);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(swordAttPos.position, swordAttPos.localScale);
        Gizmos.DrawWireCube(crouchAttPos.position, crouchAttPos.localScale);
        Gizmos.DrawWireCube(UpswordAttPos.position, UpswordAttPos.localScale);
        Gizmos.DrawWireCube(DownswordAttPos.position, DownswordAttPos.localScale);
    }


    void StopAttacking()
    {
       playerAnim.SetBool("Attack", false);
       playerAnim.SetBool("crouchAttack", false);
       playerAnim.SetBool("Attack-UpThrust-Air", false);
    }

    public InterfacciaInventario getIntInventario()
    {
        return this.interfacciaInventario; 
    
    }
}
