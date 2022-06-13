using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Equip : MonoBehaviour
{

    public string nome;
    public bool isCorazza;
    public bool isGambali;
    public bool isBracciali;
    public bool isElmo;
    public int forza;
    public int destrezza;
    public int costituzione;
    public int fortuna;
    public int difesa;

    public Player player;
    public Sprite image;
    public SpriteAtlas atlas;
    public EquipAttPicker attPicker;

    public Dictionary<string, string> equipAnimations;

    // Start is called before the first frame update
    void Start()
    {
        atlas = Resources.Load<SpriteAtlas>("Equip-sprite-Atlas");
        player = FindObjectOfType<Player>();

        equipAnimations = new Dictionary<string, string>();
        equipAnimations.Add("equip_idle", "");
        equipAnimations.Add("equip_walk", "");
        //equipAnimations.Add("equip_attack", "");

        attPicker = new EquipAttPicker(gameObject);
        attPicker.assignEquipAtt();
        gameObject.GetComponent<SpriteRenderer>().sprite = image;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<Collider2D>().IsTouching(player.raccogli_oggetti))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isCorazza)
                {
                    player.corazza = gameObject;
                    gameObject.SetActive(false);
                }

                if (isBracciali)
                {
                    player.bracciali = gameObject;
                    gameObject.SetActive(false);
                }

                if (isGambali)
                {
                    player.gambali = gameObject;
                    gameObject.SetActive(false);
                }

            }
        }
    }
}
