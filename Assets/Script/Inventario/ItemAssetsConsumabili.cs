using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssetsConsumabili : MonoBehaviour
{
    public static ItemAssetsConsumabili Instance { get; private set; }

    public Sprite spriteSoldi;
    public Sprite spritePozioneSalute;
    public Sprite spritePozioneVelocita;
    public Sprite spritePozioneSalto;

    public string soldiNome = "soldi";
    public string pozioneSaluteNome = "pozione salute";
    public string pozioneVelocitaNome = "pozione velocita";
    public string pozioneSaltoNome = "pozione salto";

    public Transform pfItemWorld;

    private void Awake()
    {
        Instance = this;
    }
}
