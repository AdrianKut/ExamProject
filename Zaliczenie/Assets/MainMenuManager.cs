using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Title")]
    public TextMeshProUGUI title;

    [Header("Panels")]
    public GameObject panelRegister;
    public GameObject panelMainMenu;
    public GameObject panelLogin;
    public GameObject panelCourses;
    public GameObject panelExams;
    private GameObject[] panels;

    [Header("Button")]
    public GameObject buttonRegister;
    public GameObject buttonLogin;
    public GameObject buttonCourses;
    public GameObject buttonExams;

    private void InitializePanels()
    {
        panels =  new GameObject[]{ panelRegister,panelMainMenu, panelLogin, panelCourses, panelExams };
    }

    void Start()
    {
        InitializePanels();

        buttonCourses.SetActive(false);
        buttonExams.SetActive(false);

        ChangePanel(panelMainMenu, "Menu");
    }

    void Update()
    {
        if (Login.isLogged)
        {
            buttonCourses.SetActive(true);
            buttonExams.SetActive(true);
        }
        else
        {
            buttonCourses.SetActive(false);
            buttonExams.SetActive(false);
        }
    }


    private void ChangePanel(GameObject openedPanel, string arg)
    {
        title.text = arg;
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);

            if (openedPanel.name == panels[i].name)
                panels[i].SetActive(true);
        }
    }

    public void OpenPanelRegister()
    {
        ChangePanel(panelRegister, "Rejestracja");
    }

    public void OpenMainMenu()
    {
        ChangePanel(panelMainMenu, "Menu");
    }

    public void OpenPanelLogin()
    {
        ChangePanel(panelLogin, "Logowanie");
    }

    public void OpenPanelCourses()
    {
        ChangePanel(panelCourses, "Przedmioty");
    }

    public void OpenPanelExams()
    {
        ChangePanel(panelExams,"Egzaminy");
    }


    public void Exit()
    {
        Application.Quit();
    }
}
