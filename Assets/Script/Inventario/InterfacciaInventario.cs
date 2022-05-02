using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfacciaInventario : MonoBehaviour
{

    private Inventory inventario;
    private Transform posizioneSlotContainerInventario;
    private Transform posizioneOggettoInventario;
    private Transform posizioneCursore;
    private Transform posizioneFinestraInventario;

    // Start is called before the first frame update
    void Awake()
    {
        posizioneSlotContainerInventario = transform.Find("ItemSlot Container");
        posizioneOggettoInventario = posizioneSlotContainerInventario.Find("ItemSlot");
        posizioneFinestraInventario = posizioneSlotContainerInventario.parent.transform;
        posizioneCursore = posizioneFinestraInventario.Find("Cursore Inventario");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    public void SetInventario(Inventory _inventario)
    {
        inventario = _inventario;
        inventario.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    public void RefreshInventoryItems()
    {
        if (this.gameObject.activeInHierarchy)
        {
            if (inventario.GetItemsList().Count > 0)
            {
                posizioneSlotContainerInventario.gameObject.SetActive(true);
                posizioneCursore.gameObject.SetActive(true);
            }
            else
            {
                posizioneCursore.gameObject.SetActive(false);
            }

            foreach (Transform child in posizioneSlotContainerInventario)
            {
                if (child == posizioneOggettoInventario)
                {
                    continue;
                }
                else
                {
                    Destroy(child.gameObject);
                }
            }
            float x = posizioneOggettoInventario.position.x;
            float y = posizioneOggettoInventario.position.y;
            float cellSpan = 32f;
            foreach (Items oggetto in inventario.GetItemsList())
            {
                RectTransform itemSlotRectTransform = Instantiate(posizioneOggettoInventario, posizioneSlotContainerInventario).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.position = new Vector2(x, y);
                Image image = itemSlotRectTransform.Find("Immagine Oggetto").GetComponent<Image>();
                TMPro.TextMeshProUGUI nomeOggetto = itemSlotRectTransform.Find("Nome Oggetto").GetComponent<TMPro.TextMeshProUGUI>();
                TMPro.TextMeshProUGUI quantitaOggetto = itemSlotRectTransform.Find("Quantita Oggetto").GetComponent<TMPro.TextMeshProUGUI>();
                image.sprite = oggetto.GetSprite();
                nomeOggetto.text = oggetto.GetNomeOggetto();
                quantitaOggetto.text = oggetto.quantità.ToString();
                y = y - cellSpan;
            }
        }
    } 
}
