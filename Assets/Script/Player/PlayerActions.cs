using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public Player player;
    Vector2 playerPos;

    public Transform attPos;
    public Transform crouchAttPos;

   public PlayerActions(Player _player, Transform _attPos, Transform _crouchAttPos)
    {
        this.player = _player;
        this.attPos = _attPos;
        this.crouchAttPos = _crouchAttPos;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void Attack()
    {        
        bool shield = false;
        bool inanimateObj = false;
        bool enemy = false;
        Collider2D colliderNemico = null;
        Collider2D[] strike = null;

        playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

        if (player.isCrouch)
        {
            strike = Physics2D.OverlapBoxAll(crouchAttPos.position, crouchAttPos.localScale * player.attackRange, Vector2.Angle(Vector2.zero, player.transform.position));
        }
        else
        {
            strike = Physics2D.OverlapBoxAll(attPos.position, attPos.localScale * player.attackRange, Vector2.Angle(Vector2.zero, player.transform.position));
        }

        foreach (Collider2D collider in strike)
        {
            if (collider.tag == "Scudo" && collider.gameObject != player.gameObject)
            {
                shield = true;
            }

            if (collider.tag == "Enemies" && collider.gameObject != player.gameObject)
            {
                enemy = true;
                colliderNemico = collider;
            }

            if (collider.tag == "Mobile" && collider.gameObject != player.gameObject)
            {
                inanimateObj = true;
            }
        } 

        if (colliderNemico != null)
        {
            Hit(shield, enemy, inanimateObj, colliderNemico, playerPos);
            colliderNemico = null;
        }
    }

    public void Hit(bool isShield, bool isEnemy, bool isInanimateObject, Collider2D oggettoColpito, Vector2 playerPos)
    {
        if (isEnemy)
        {
            if (isShield)
            {
                ApplyHitAndForce(true, oggettoColpito, new Vector3(-1.5f, 0, 0), new Vector3(1.5f, 0, 0), new Vector3(), player.force, ForceMode2D.Impulse);
                return;
            }

            Enemy nemico = oggettoColpito.GetComponentInParent<Enemy>();
            if(nemico != null)
            {
                nemico.healt -= player.attacco;
            }
           
            ApplyHitAndForce(true, oggettoColpito, new Vector3(-1.5f, 1, 0), new Vector3(1.5f, 1, 0), new Vector3(), player.force, ForceMode2D.Impulse);
            return;
        }

        if (isInanimateObject)
        {
            ApplyHitAndForce(true, oggettoColpito, new Vector3(-1f, 1, 0), new Vector3(1f, 1, 0), new Vector3(), player.force, ForceMode2D.Impulse);
            return;
        }     
    }

    private void ApplyHitAndForce(bool isPlayerHit, Collider2D oggetto, Vector3 forceDirectionLeft, Vector3 forceDirectionRight, Vector3 forceDirection, float force, ForceMode2D forceType)
    {
        if (isPlayerHit)
        {
            if (playerPos.x > oggetto.transform.position.x)
            {
                oggetto.attachedRigidbody.AddForce(forceDirectionLeft * force, forceType);
                oggetto.attachedRigidbody.AddTorque(10);
            }
            else
            {
                oggetto.attachedRigidbody.AddForce(forceDirectionRight * force, forceType);
                oggetto.attachedRigidbody.AddTorque(-10);
            }
        }
        else
        {
            oggetto.attachedRigidbody.AddForce(forceDirection, forceType);
            oggetto.attachedRigidbody.AddTorque(10);
        }
    }
}
