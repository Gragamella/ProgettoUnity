using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spada : Weapon
{

    public Sprite Image;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        nome = "spada-test";
        tipo = "spada";
        valoreAffinit� = 1;
        affinit� = FORZA;
        attPower = 5;

        if (affinit�.Equals(FORZA))
        {
            attPowerCalcolato = attPower + player.forza * valoreAffinit�;
        }

        if (affinit�.Equals(DESTREZZA))
        {
            attPowerCalcolato = attPower + player.destrezza * valoreAffinit�;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<Collider2D>().IsTouching(player.raccogli_oggetti))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                player.arma = gameObject;
                gameObject.SetActive(false);
            }
        }       
    }
}
