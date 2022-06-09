using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.SimpleLUT;

public class LogicaBrillo : MonoBehaviour
{
    public Slider sliderBrillo;
    public float sliderBrilloValue;
    //public Image PanelBrillo;

    public SimpleLUT simpleLUT;

    void Start()
    {
        sliderBrillo.value = PlayerPrefs.GetFloat("brillo", 0.5f);

        simpleLUT.Brightness = sliderBrilloValue;

        //PanelBrillo.color = new Color(PanelBrillo.color.r, PanelBrillo.color.g, PanelBrillo.color.b, sliderBrillo.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSlider(float valor)
    {
        sliderBrilloValue = valor;
        PlayerPrefs.SetFloat("brillo", sliderBrilloValue);
        simpleLUT.Brightness = sliderBrilloValue;
        //PanelBrillo.color = new Color(PanelBrillo.color.r, PanelBrillo.color.g, PanelBrillo.color.b, sliderBrillo.value);
    }
}
