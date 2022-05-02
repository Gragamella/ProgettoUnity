using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{

    public Player inventarioGiocatore;

    private SpriteRenderer spriteRenderer;
    private Items item;

    private TMPro.TextMeshProUGUI quantitaOggetto;
    public static ItemWorld SpawnItemWorld(Vector3 position, Items item)
    {
       Transform transform = Instantiate(ItemAssetsConsumabili.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();

        itemWorld.SetItem(item);

        return itemWorld;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventarioGiocatore = FindObjectOfType<Player>();
        Transform transformCanvas = transform.Find("Canvas");
        quantitaOggetto = transformCanvas.Find("Oggetto-quantita").GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Update()
    {
        if (this.gameObject.GetComponent<Collider2D>().IsTouching(inventarioGiocatore.gameObject.GetComponent<Collider2D>()))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inventarioGiocatore.inventario.addItems(GetItem());
                Destroy(this.gameObject);
            }
        }
    }

    public void SetItem(Items item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        quantitaOggetto.SetText(item.quantità.ToString());
    }


    public Items GetItem()
    {
        return item;
    }

}
