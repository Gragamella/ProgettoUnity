using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public string nome;
    public string tipo;
    public int attPower;
    public string affinit�;
    public int valoreAffinit�;
    public int attPowerCalcolato;
    public Player player;

    public string FORZA = "forza";
    public string DESTREZZA = "destrezza";



    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        attPower = 0;
        attPowerCalcolato = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (affinit�.Equals(FORZA))
        {
            attPowerCalcolato = attPower + player.forza * valoreAffinit�;
        }

        if (affinit�.Equals(DESTREZZA))
        {
            attPowerCalcolato = attPower + player.destrezza * valoreAffinit�;
        }
    }
}
