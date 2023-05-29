using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    [SerializeField] private GameObject lobby;
    [SerializeField] private GameObject login;
    [SerializeField] private GameObject register;
    private bool loginPanelFunc;
    private bool registerPanelFunc;

    public void Login()
    {
        loginPanelFunc = !loginPanelFunc;
        lobby.SetActive(!lobby.activeSelf);
        login.SetActive(!login.activeSelf);
    }

    public void Register()
    {
        registerPanelFunc = !registerPanelFunc;
        lobby.SetActive(!lobby.activeSelf);
        register.SetActive(!register.activeSelf);
    }

    public void GameLevel() 
    {
        if (Network_Manager._NETWORK_MANAGER.GetIsLoggedInt())
        {
            Scene_Manager.Instance.LoadScene(2);
        }        
    }

    public void Exit() 
    {
        Scene_Manager.Instance.Exit();
    }
}
