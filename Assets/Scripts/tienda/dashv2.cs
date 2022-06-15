using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashv2 : MonoBehaviour
{
    public Player plyr;
    public bool isBought;

    void Start()
    {
        plyr = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isBought = false;
    }

    // Update is called once per frame
    void Update()
    {

        Buy();
    }

    void Buy()
    {
        if (plyr.almas >= 5)
        {
            Destroy(gameObject);
            isBought = true;
        }
    }
}
