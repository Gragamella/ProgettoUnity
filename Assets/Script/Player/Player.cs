using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject papas;

    private Animator playerAnim;
    private Rigidbody2D rgb2DPlayer;

    public int salute = 100;
    public int attacco = 20;
    public float velocita = 5f;
    public float jumpForce = 400f;

    private int groundMask;

    private float xAxis;
    private float yAxis;
    private bool isGrounded;
    private bool isJump;
    private bool isJumpPressed;
    private bool isCrouch;
    private bool isCrouchPressed;
    private bool isAttacking;
    private bool isAttackPressed;
    private string currentAnimation;

    public bool down = false;

    public Transform swordAttPos;
    public Transform UpswordAttPos;
    public Transform DownswordAttPos;
    public Transform crouchAttPos;

    public float crouchAttackRange;
    public float attackRange;
    public float force;

    string PLAYER_IDLE = "Player_idle";
    string PLAYER_WALKING = "Player_walking";
    string PLAYER_CROUCH = "Player_crouch";
    string PLAYER_AIR = "Player_air";
    string PLAYER_ATTACK = "Player_attack";
    string PLAYER_CROUCH_ATTACK = "Player_crouch_attack";
    string PLAYER_AIR_ATTACK = "Player_air_attack";

    public Inventory inventario;

    [SerializeField] private InterfacciaInventario interfacciaInventario;

    public Enemy nemico;
    Collider2D colliderNemico;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        rgb2DPlayer = GetComponent<Rigidbody2D>();
        groundMask = 1 << LayerMask.NameToLayer("Pavimento");


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
        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJumpPressed = true;
        }

        if (Input.GetKeyDown("k"))
        {
            isAttackPressed = true;
        }

        if (Input.GetKeyDown("s"))
        {
            if (isGrounded)
            {
                isCrouch = true;
            }
        }
        else if (Input.GetKeyUp("s"))
        {
            if (isGrounded)
            {
                isCrouch = false;
            }
        }       


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
       

    }

    private void FixedUpdate()
    {
        //GROUNDED
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, groundMask);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded)
        {
            isJump = false;
        }

        //CALCOLO INPUT VELOCITA
        Vector2 vel = new Vector2(0, rgb2DPlayer.velocity.y);

        if (xAxis < 0)
        {
            vel.x = -velocita;
            transform.localScale = new Vector2(-1, 1);
        }
        else if(xAxis > 0)
        {
            vel.x = velocita;
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            vel.x = 0;
        }

        //IDLE-MOVING
        if (isGrounded && !isAttacking && !isCrouch)
        {
            if (xAxis != 0)
            {
                CambiaStatoAnimazione(PLAYER_WALKING);
            }
            else
            {
                CambiaStatoAnimazione(PLAYER_IDLE);
            }
        }

        //CROUCH
               
            if (isCrouch && !isAttacking)
            {               
                CambiaStatoAnimazione(PLAYER_CROUCH);
                vel.x = 0;
            }
        
           

        //SALTO
        if(isJumpPressed && isGrounded)
        {
            isJumpPressed = false;

            if (!isJump)
            {
                isJump = true;
                rgb2DPlayer.AddForce(new Vector2(0, jumpForce));             
                CambiaStatoAnimazione(PLAYER_AIR);
            }         
        }

       


        //ATTACCO
        if (isAttackPressed)
        {           
            isAttackPressed = false;

            if (!isAttacking)
            {
                isAttacking = true;

                if (isGrounded)
                {
                    if (isCrouch)
                    {
                        CambiaStatoAnimazione(PLAYER_CROUCH_ATTACK);
                      
                    }                  
                    else
                    {
                        CambiaStatoAnimazione(PLAYER_ATTACK);
                        
                    }

                    Invoke("AttaccoCompleto", 1f);

                }
                else
                {
                    CambiaStatoAnimazione(PLAYER_ATTACK);
                    Invoke("AttaccoCompleto", 1f);
                }              
            }
        }

        if (isCrouch && !isAttacking)
        {           
            vel.x = 0;
        }

        if (isAttacking && !isJump)
        {
            vel.x = 0;
        }


        rgb2DPlayer.velocity = vel;

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


    void AttaccoCompleto()
    {
        isAttacking = false;
    }

    public InterfacciaInventario getIntInventario()
    {
        return this.interfacciaInventario; 
    
    }

    void CambiaStatoAnimazione(string animazione)
    {
        playerAnim.Play(animazione);
    }
}
