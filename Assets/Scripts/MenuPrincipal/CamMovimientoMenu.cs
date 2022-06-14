using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamMovimientoMenu : MonoBehaviour
{
    public Transform[] viewsMp;
    //public GameObject[] luces;

    public float transitionSpeedMp;

    public Transform currentView;

    public MainMenuController mainMenuController;

    void Start()
    {
        currentView = viewsMp[0];
    }


    void Update()
    {

        if (currentView == viewsMp[0]) //jugar
        {
            /*luces[0].SetActive(true);
            luces[1].SetActive(false);*/
            if (Input.GetKeyDown(KeyCode.A))
            {
                //currentView = views[4];
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                currentView = viewsMp[1];
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("Loading");
            }
        }

        else if (currentView == viewsMp[1]) //CARGAR
        {
            /*luces[0].SetActive(false);
            luces[1].SetActive(true);¨*/
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentView = viewsMp[0];
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                currentView = viewsMp[2];
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1;
                //SceneManager.LoadScene("Sala de Trofeos");
            }
        }

        else if (currentView == viewsMp[2]) //TESOROS
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentView = viewsMp[1];
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                currentView = viewsMp[3];
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("Sala de Trofeos");
            }
        }

        else if (currentView == viewsMp[3]) //OPCIONES
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentView = viewsMp[2];
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                currentView = viewsMp[4];
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentView = viewsMp[5];
                mainMenuController.OpenOptions();
            }
        }

        else if (currentView == viewsMp[4]) //SALIR
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentView = viewsMp[3];
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                //currentView = viewsMp[0];
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Saliste del infierno");
                Application.Quit();
            }
        }

        else if (currentView == viewsMp[6]) //condicional 3_7
        {
            currentView = viewsMp[3];
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeedMp);
    }
}
