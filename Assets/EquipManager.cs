using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    Player player;
    public GameObject corazza;
    public GameObject elmo;
    public GameObject gambali;
    public GameObject bracciali;

    public Dictionary<string, string> animationsGambali;
    public Dictionary<string, string> animationsBracciali;
    public Dictionary<string, string> animationsCorazza;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
        corazza = gameObject.transform.GetChild(2).gameObject;
      //  elmo = gameObject.transform.GetChild(2).gameObject;
        gambali = gameObject.transform.GetChild(3).gameObject;
        bracciali = gameObject.transform.GetChild(4).gameObject;

        animationsGambali = new Dictionary<string, string>();
        animationsBracciali = new Dictionary<string, string>();
        animationsCorazza = new Dictionary<string, string>();
        animationsGambali.Add("equip_idle", "");
        animationsGambali.Add("equip_walk", "");
        animationsCorazza.Add("equip_idle", "");
        animationsCorazza.Add("equip_walk", "");
        animationsBracciali.Add("equip_idle", "");
        animationsBracciali.Add("equip_walk", "");
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
            if(player.corazza != null)
                corazza.GetComponent<Animator>().Play(animationsCorazza["equip_walk"]);
            if (player.gambali != null)
                gambali.GetComponent<Animator>().Play(animationsGambali["equip_walk"]);
            if (player.bracciali != null)
                bracciali.GetComponent<Animator>().Play(animationsBracciali["equip_walk"]);
            
        }
        else
        {
            if (player.corazza != null)
                corazza.GetComponent<Animator>().Play(animationsCorazza["equip_idle"]);
            if (player.gambali != null)
                gambali.GetComponent<Animator>().Play(animationsGambali["equip_idle"]);
            if (player.bracciali != null)
                bracciali.GetComponent<Animator>().Play(animationsBracciali["equip_idle"]);
        }
    }
}
