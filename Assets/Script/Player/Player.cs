using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject arma;
    public GameObject corazza;
    public GameObject gambali;
    public GameObject bracciali;

    public SpadaManager spadaManagerSlot;
    public EquipManager equipManager;
    private PlayerActions playerActions;

    private Dictionary<string, string> dictWeaponAnim;

    private Animator spadaAnim;
    private Animator playerAnim;
    private Rigidbody2D rgb2DPlayer;

    public int salute;
    public int saluteMassima;
    public int attacco;
    public int livello;
    public float velocita = 5f;
    public float peso;
    public float pesoMassimo;
    public float jumpForce = 400f;

    public int forza;
    public int destrezza;
    public int costituzione;
    public int fortuna;

    private int groundMask;

    private float xAxis;
    private float yAxis;
    private bool isGrounded;
    private bool isJumpPressed;
    public bool isCrouch;
    private bool isCrouchPressed;
    private bool isAttackPressed;
    private string currentAnimation;

    public bool down = false;
    public bool isWalking = false;
    public bool isCrouching = false;
    public bool isJumping = false;
    public bool isAttacking = false;
    public bool isCrouchAttacking = false;
    public bool isFalling = false;
    public bool isIdle = false;

    public enum PlayerState
    {
        Idle,
        Walking,
        Attack,
        Jump,
        Crouch,
        CrouchAttack,
        Falling,
        Empty,
    }

    public PlayerState state;

    public Transform swordAttPos;  
    public Transform crouchAttPos;
    
    public Collider2D raccogli_oggetti;

    public float crouchAttackRange;
    public float attackRange;
    public float force;

    string PLAYER_IDLE = "Player_idle";
    string PLAYER_WALKING = "Player_walking";
    string PLAYER_CROUCH = "Player_crouch";
    string PLAYER_AIR = "Player_air";
    string PLAYER_ATTACK = "Player_attack";
    string PLAYER_CROUCH_ATTACK = "Player_crouch_attack";

    public Inventory inventario;

    [SerializeField] private InterfacciaInventario interfacciaInventario;

    public Enemy nemico;
    Collider2D colliderNemico;

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerState.Idle;

        dictWeaponAnim = spadaManagerSlot.dictSpadaAnim;

        if (gameObject.GetComponent<Collider2D>().isTrigger)
        {
            raccogli_oggetti = gameObject.GetComponent<Collider2D>();
        }
      
        playerAnim = GetComponent<Animator>();
        rgb2DPlayer = GetComponent<Rigidbody2D>();
        groundMask = 1 << LayerMask.NameToLayer("Pavimento");

        attacco = 10;
        livello = 1;
        forza = 5;
        destrezza = 5;
        costituzione = 5;
        fortuna = 5;
        saluteMassima = costituzione * 10;
        salute = saluteMassima;

        playerActions = new PlayerActions(this, swordAttPos, crouchAttPos);

       // ItemWorld.SpawnItemWorld(new Vector3(7, 2), new Items { tipoOggetto = Items.ItemType.PozioneSalute, quantità = 2 });
       // ItemWorld.SpawnItemWorld(new Vector3(10, 2), new Items { tipoOggetto = Items.ItemType.Monete, quantità = 1 });
       // ItemWorld.SpawnItemWorld(new Vector3(12, 2), new Items { tipoOggetto = Items.ItemType.PozioneVelocita, quantità = 1 });
    }

    private void Awake()
    {
        spadaManagerSlot = GetComponentInChildren<SpadaManager>();
        equipManager = GetComponentInChildren<EquipManager>();
        spadaAnim = spadaManagerSlot.gameObject.GetComponent<Animator>();
        inventario = new Inventory();
        interfacciaInventario.SetInventario(inventario);

    }

    // Update is called once per frame
    void Update()
    {                 
        xAxis = Input.GetAxisRaw("Horizontal");      

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

        if (!isGrounded)
        {
            state = PlayerState.Falling;
        }
      

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            state = PlayerState.Jump;
        }

        if (Input.GetKeyDown("k") && state != PlayerState.Attack)
        {
            if (state == PlayerState.Crouch)
            {
                state = PlayerState.CrouchAttack;
            }
            else
            {
                state = PlayerState.Attack;
            }           
        }

       

        if (isGrounded && state != PlayerState.Attack && state != PlayerState.Jump && state != PlayerState.CrouchAttack && state != PlayerState.Crouch)
        {
            if (xAxis != 0)
            {
                state = PlayerState.Walking;
            }
            else
            {
               if(!isAttacking && !isCrouchAttacking)
                {
                    state = PlayerState.Idle;
                }                                         
            }
        }

        if (Input.GetKeyDown("s"))
        {
            if (isGrounded && !isAttacking)
            {
                state = PlayerState.Crouch;
            }
        }
        else if (Input.GetKeyUp("s") && !isAttacking && !isCrouchAttacking)
        {
            state = PlayerState.Empty;
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
      
        //ANIMAZIONI EQUIP
        EquipAnimationSelect();

        //CALCOLO INPUT VELOCITA
        Vector2 vel = MovementSpeedCalc();

        //WALKING
        if (state == PlayerState.Walking)
        {
            CambiaStatoAnimazione(PLAYER_WALKING);
            if (arma != null)
            {
                CambiaStatoAnimazioneBetter(dictWeaponAnim["walking"]);
            }
        }

        //IDLE
        if (state == PlayerState.Idle)
        {
            CambiaStatoAnimazione(PLAYER_IDLE);
            if (arma != null)
            {
                CambiaStatoAnimazioneBetter(dictWeaponAnim["idle"]);
            }
        }


        //CROUCH
               
        if (state == PlayerState.Crouch)
        {      
            CambiaStatoAnimazione(PLAYER_CROUCH);
            vel.x = 0;
             if (arma != null)
             {
                 CambiaStatoAnimazioneBetter(dictWeaponAnim["crouch"]);
             }
        }
        



        //SALTO        
        
        if (state == PlayerState.Jump)
        {
            rgb2DPlayer.AddForce(new Vector2(0, jumpForce));
            CambiaStatoAnimazione(PLAYER_AIR);
            if (arma != null)
            {
                CambiaStatoAnimazioneBetter(dictWeaponAnim["air"]);
            }
            state = PlayerState.Empty;
        }


        //FALLING
        if (state == PlayerState.Falling)
        {
            CambiaStatoAnimazione(PLAYER_AIR);
            if (arma != null)
            {
                CambiaStatoAnimazioneBetter(dictWeaponAnim["air"]);
            }
        }
        
        //ATTACCO
        if (state == PlayerState.Attack && !isAttacking)
        {                                    
                if (isGrounded)
                {
                                        
                    CambiaStatoAnimazione(PLAYER_ATTACK);
                    if (arma != null)
                    {
                        CambiaStatoAnimazioneBetter(dictWeaponAnim["attack"]);                          
                    }
                    Invoke("PlayerActions_Attack", 0.4f);
                                      
                }
                else
                {
                   
                    CambiaStatoAnimazione(PLAYER_ATTACK);
                    if (arma != null)
                    {
                        CambiaStatoAnimazioneBetter(dictWeaponAnim["attack"]);                        
                    }
                    Invoke("PlayerActions_Attack", 0.4f);                  
                }
            isAttacking = true;
            Invoke("AttaccoCompleto", 1f);
            
        }

        //CROUCH-ATTACCO
        if (state == PlayerState.CrouchAttack && !isCrouchAttacking)
        {
                                                
           CambiaStatoAnimazione(PLAYER_CROUCH_ATTACK);
           if (arma != null)
           {
               CambiaStatoAnimazioneBetter(dictWeaponAnim["crouch_attack"]);
           }
           Invoke("PlayerActions_Attack", 0.4f);

           isCrouchAttacking = true;                                   
           Invoke("AttaccoCompletoCrouch", 1f);
        }
        

        if (state == PlayerState.Attack || state == PlayerState.CrouchAttack || state == PlayerState.Crouch)
        {           
            vel.x = 0;
        }

      
        rgb2DPlayer.velocity = vel;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(swordAttPos.position, swordAttPos.localScale);
        Gizmos.DrawWireCube(crouchAttPos.position, crouchAttPos.localScale);       
    }

    void EquipAnimationSelect()
    {
        if (arma != null)
        {
            attacco = forza * 2 + arma.GetComponent<Spada>().attPowerCalcolato;

            if (arma.GetComponent<Spada>().Image != null && spadaManagerSlot.gameObject.GetComponent<SpriteRenderer>().sprite != arma.GetComponent<Spada>().Image)
            {
                spadaManagerSlot.gameObject.GetComponent<SpriteRenderer>().sprite = arma.GetComponent<Spada>().Image;
            }
        }

        if (corazza != null)
        {

            if (corazza.GetComponent<Equip>().image != null && equipManager.corazza.gameObject.GetComponent<SpriteRenderer>().sprite != corazza.GetComponent<Equip>().image)
            {
                equipManager.corazza.gameObject.GetComponent<SpriteRenderer>().sprite = corazza.GetComponent<Equip>().image;
                equipManager.animationsCorazza["equip_idle"] = corazza.GetComponent<Equip>().equipAnimations["equip_idle"];
                equipManager.animationsCorazza["equip_walk"] = corazza.GetComponent<Equip>().equipAnimations["equip_walk"];
                equipManager.animationsCorazza["equip_crouch"] = corazza.GetComponent<Equip>().equipAnimations["equip_crouch"];
                equipManager.animationsCorazza["equip_air"] = corazza.GetComponent<Equip>().equipAnimations["equip_air"];
                equipManager.animationsCorazza["equip_attack"] = corazza.GetComponent<Equip>().equipAnimations["equip_attack"];
                equipManager.animationsCorazza["equip_crouch_attack"] = corazza.GetComponent<Equip>().equipAnimations["equip_crouch_attack"];


            }
        }

        if (gambali != null)
        {

            if (gambali.GetComponent<Equip>().image != null && equipManager.gambali.gameObject.GetComponent<SpriteRenderer>().sprite != gambali.GetComponent<Equip>().image)
            {
                equipManager.gambali.gameObject.GetComponent<SpriteRenderer>().sprite = gambali.GetComponent<Equip>().image;
                equipManager.animationsGambali["equip_idle"] = gambali.GetComponent<Equip>().equipAnimations["equip_idle"];
                equipManager.animationsGambali["equip_walk"] = gambali.GetComponent<Equip>().equipAnimations["equip_walk"];
                equipManager.animationsGambali["equip_crouch"] = gambali.GetComponent<Equip>().equipAnimations["equip_crouch"];
                equipManager.animationsGambali["equip_air"] = gambali.GetComponent<Equip>().equipAnimations["equip_air"];
                equipManager.animationsGambali["equip_attack"] = gambali.GetComponent<Equip>().equipAnimations["equip_attack"];
                equipManager.animationsGambali["equip_crouch_attack"] = gambali.GetComponent<Equip>().equipAnimations["equip_crouch_attack"];
            }
        }

        if (bracciali != null)
        {

            if (bracciali.GetComponent<Equip>().image != null && equipManager.bracciali.gameObject.GetComponent<SpriteRenderer>().sprite != bracciali.GetComponent<Equip>().image)
            {
                equipManager.bracciali.gameObject.GetComponent<SpriteRenderer>().sprite = bracciali.GetComponent<Equip>().image;
                equipManager.animationsBracciali["equip_idle"] = bracciali.GetComponent<Equip>().equipAnimations["equip_idle"];
                equipManager.animationsBracciali["equip_walk"] = bracciali.GetComponent<Equip>().equipAnimations["equip_walk"];
                equipManager.animationsBracciali["equip_crouch"] = bracciali.GetComponent<Equip>().equipAnimations["equip_crouch"];
                equipManager.animationsBracciali["equip_air"] = bracciali.GetComponent<Equip>().equipAnimations["equip_air"];
                equipManager.animationsBracciali["equip_attack"] = bracciali.GetComponent<Equip>().equipAnimations["equip_attack"];
                equipManager.animationsBracciali["equip_crouch_attack"] = bracciali.GetComponent<Equip>().equipAnimations["equip_crouch_attack"];
            }
        }
    }

    Vector2 MovementSpeedCalc()
    {
       Vector2 vel =  new Vector2(0, rgb2DPlayer.velocity.y);

        if (xAxis < 0)
        {
            vel.x = -velocita;
            transform.localScale = new Vector2(-1, 1);
        }
        else if (xAxis > 0)
        {
            vel.x = velocita;
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            vel.x = 0;
        }

        return vel;
    }


    void AttaccoCompleto()
    {
        state = PlayerState.Empty;
        isAttacking = false;
    }

    void AttaccoCompletoCrouch()
    {
        state = PlayerState.Empty;
        isCrouchAttacking = false;
    }

    void PlayerActions_Attack()
    {
        playerActions.Attack();
    }

    public InterfacciaInventario getIntInventario()
    {
        return this.interfacciaInventario; 
    
    }

    void CambiaStatoAnimazione(string animazione)
    {
        playerAnim.Play(animazione);
    }

    void CambiaStatoAnimazioneBetter(string animazione)
    {
        spadaAnim.Play(animazione);
    }
}
