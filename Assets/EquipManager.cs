using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    Player player;
    GameObject corazza;
    GameObject elmo;
    GameObject gambali;
    GameObject bracciali;

    Dictionary<string, string> animations;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
        corazza = gameObject.transform.GetChild(2).gameObject;
        elmo = gameObject.transform.GetChild(2).gameObject;
        gambali = gameObject.transform.GetChild(3).gameObject;
        bracciali = gameObject.transform.GetChild(4).gameObject;

        animations = new Dictionary<string, string>();
        animations.Add("idle", "red-temple-gambali-idle");
        animations.Add("walking", "red-temple-gambali-walking");
        //animations.Add("","");
        //animations.Add("","");
        //animations.Add("","");
        //animations.Add("","");

    }

    // Update is called once per frame
    void Update()
    {
        if (player.isWalking)
        {
            gambali.GetComponent<Animator>().Play(animations["walking"]);
        }
        else
        {
            gambali.GetComponent<Animator>().Play(animations["idle"]);
        }
    }
}
