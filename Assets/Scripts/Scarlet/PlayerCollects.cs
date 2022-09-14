using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollects : MonoBehaviour
{
    public int collectables = 1;
    public int counterCollectables = 0;
    //public TMP_Text collecText;
    //public GameObject collecTextGo;
    public float counterNum;

    void Start()
    {
        //collecTextGo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        counterNum += Time.deltaTime;

        if(collectables == counterCollectables)
        {
            Collects();
            counterNum = 0;
        }

        if(counterNum > 3)
        {
            //collecTextGo.SetActive(false);
            counterNum = 0;
        }
    }

    private void Collects()
    {
        //collecTextGo.SetActive(true);
        collectables++;
    }
}
