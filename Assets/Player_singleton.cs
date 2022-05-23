using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_singleton : MonoBehaviour
{

    public static Player player_instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(player_instance == null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
