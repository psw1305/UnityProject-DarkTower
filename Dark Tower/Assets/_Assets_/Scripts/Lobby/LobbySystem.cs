using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySystem : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject prevBtn;
    [SerializeField] int uiLayer;

    private void Start()
    {
        uiLayer = 0;
    }

    public void NextLayer()
    {
        panels[uiLayer].SetActive(false);
        uiLayer += 1;
        panels[uiLayer].SetActive(true);

        if (uiLayer == 1)
        {
            prevBtn.SetActive(true);
        }
    }

    public void PrevLayer()
    {
        if (uiLayer > 0)
        {
            panels[uiLayer].SetActive(false);
            uiLayer -= 1;
            panels[uiLayer].SetActive(true);
        }

        if (uiLayer == 0)
        {
            prevBtn.SetActive(false);
        }
    }

    public void CharacterSelect()
    {

    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }
}
