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
        valoreAffinitą = 1;
        affinitą = FORZA;
        //attPower = 5;

        if (affinitą.Equals(FORZA))
        {
            attPowerCalcolato = attPower + player.forza * valoreAffinitą;
        }

        if (affinitą.Equals(DESTREZZA))
        {
            attPowerCalcolato = attPower + player.destrezza * valoreAffinitą;
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
