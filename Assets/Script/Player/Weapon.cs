using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public string nome;
    public string tipo;
    public int attPower;
    public string affinitą;
    public int valoreAffinitą;
    public int attPowerCalcolato;
    public Player player;

    public string FORZA = "forza";
    public string DESTREZZA = "destrezza";



    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
