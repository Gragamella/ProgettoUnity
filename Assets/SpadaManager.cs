using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadaManager : MonoBehaviour
{

    public Dictionary<string, string> dictSpadaAnim;
    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        dictSpadaAnim = new Dictionary<string, string>();

        dictSpadaAnim.Add("idle", "spada_Idle");
        dictSpadaAnim.Add("walking", "spada_walking");
    }

    private void Awake()
    {
        sprite = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
