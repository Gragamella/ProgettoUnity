using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items 
{
   public enum ItemType
    {
        Spada,
        PozioneSalute,
        PozioneVelocita,
        PozioneSalto,
        Lancia,
        Frusta,
        Monete,
        Gioelli
    }

    public ItemType tipoOggetto;
    public int quantità;

    public Sprite GetSprite()
    {
        switch (tipoOggetto)
        {
            default:         
            case ItemType.PozioneSalute: return ItemAssetsConsumabili.Instance.spritePozioneSalute;
            case ItemType.Monete: return ItemAssetsConsumabili.Instance.spriteSoldi;      
            case ItemType.PozioneVelocita: return ItemAssetsConsumabili.Instance.spritePozioneVelocita;
            case ItemType.PozioneSalto: return ItemAssetsConsumabili.Instance.spritePozioneSalto;
        }
    }

    public string GetNomeOggetto()
    {
        switch (tipoOggetto)
        {
            default:
            case ItemType.PozioneSalute: return ItemAssetsConsumabili.Instance.pozioneSaluteNome;
            case ItemType.Monete: return ItemAssetsConsumabili.Instance.soldiNome;
            case ItemType.PozioneVelocita: return ItemAssetsConsumabili.Instance.pozioneVelocitaNome;
            case ItemType.PozioneSalto: return ItemAssetsConsumabili.Instance.pozioneSaltoNome;
        }
    }

}
