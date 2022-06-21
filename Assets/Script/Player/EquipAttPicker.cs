using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipAttPicker : MonoBehaviour
{

    public GameObject equipObj;
    public Equip equip;

    public EquipAttPicker(GameObject _equipObj)
    {
        equipObj = _equipObj;
        equip = equipObj.GetComponent<Equip>();
        
    }


    public void assignEquipAtt()
    {
        switch (equipObj.name)
        {
            default:
            case "corazza-rossa":
                equip.isCorazza = true;
                equip.image = equip.atlas.GetSprite(equipObj.name);
                equip.equipAnimations["equip_idle"] = "red-temple-corazza-Idle";
                equip.equipAnimations["equip_walk"] = "red-temple-corazza";
                equip.equipAnimations["equip_crouch"] = "red-temple-corazza-crouch";
                equip.equipAnimations["equip_air"] = "red-temple-corazza-air";
                equip.equipAnimations["equip_attack"] = "red-temple-corazza-attack";
                equip.equipAnimations["equip_crouch_attack"] = "red-temple-corazza-crouch-attack";

                equip.forza = 2;
                equip.costituzione = 3;
                equip.destrezza = 3;
                equip.fortuna = 2;
                equip.difesa = 10;
                break;
            case "bracciali-rossa":
                equip.isBracciali = true;
                equip.image = equip.atlas.GetSprite(equipObj.name);
                equip.equipAnimations["equip_idle"] = "red-temple-bracciali-idle";
                equip.equipAnimations["equip_walk"] = "red-temple-bracciali-walking";
                equip.equipAnimations["equip_crouch"] = "red-temple-bracciali-crouch";
                equip.equipAnimations["equip_air"] = "red-temple-bracciali-air";
                equip.equipAnimations["equip_attack"] = "red-temple-bracciali-attack";
                equip.equipAnimations["equip_crouch_attack"] = "red-temple-bracciali-crouch-attack";

                equip.forza = 2;
                equip.costituzione = 3;
                equip.destrezza = 3;
                equip.fortuna = 2;
                equip.difesa = 10;
                break;
            case "gambali-rossa":
                equip.isGambali = true;
                equip.image = equip.atlas.GetSprite(equipObj.name);
                equip.equipAnimations["equip_idle"] = "red-temple-gambali-idle";
                equip.equipAnimations["equip_walk"] = "red-temple-gambali-walking";
                equip.equipAnimations["equip_crouch"] = "red-temple-gambali-crouch";
                equip.equipAnimations["equip_air"] = "red-temple-gambali-air";
                equip.equipAnimations["equip_attack"] = "red-temple-gambali-attack";
                equip.equipAnimations["equip_crouch_attack"] = "red-temple-gambali-crouch-attack";

                equip.forza = 2;
                equip.costituzione = 3;
                equip.destrezza = 3;
                equip.fortuna = 2;
                equip.difesa = 10;
                break;
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
