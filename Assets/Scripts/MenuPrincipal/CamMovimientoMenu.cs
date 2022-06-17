using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CamMovimientoMenu : MonoBehaviour
{
    public GameObject TextEnter;
    public GameObject MenuPrincipal;

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
        if (currentView == viewsMp[0]) //lugar inicial
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                currentView = viewsMp[1];
                TextEnter.SetActive(false);
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeedMp);

        Vector3 currentAngle = new Vector3(
            Mathf.Lerp(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x,
            Time.deltaTime * transitionSpeedMp),

            Mathf.Lerp(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y,
            Time.deltaTime * transitionSpeedMp),

            Mathf.Lerp(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z,
            Time.deltaTime * transitionSpeedMp)

            );

        transform.eulerAngles = currentAngle;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag ==  "View1")
        {
            MenuPrincipal.SetActive(true);
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //Set a new selected object
            EventSystem.current.SetSelectedGameObject(mainMenuController.menuFirstButton);
        }
        else if (other.gameObject.tag == "View2")
        {
            mainMenuController.optionsMainMenu.SetActive(true);
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //Set a new selected object
            EventSystem.current.SetSelectedGameObject(mainMenuController.optionsFirstButtonMainMenu);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag ==  "View1")
        {
            MenuPrincipal.SetActive(false);
        }
        else if (other.gameObject.tag == "View2")
        {
            mainMenuController.optionsMainMenu.SetActive(false);
        }
    }
}
