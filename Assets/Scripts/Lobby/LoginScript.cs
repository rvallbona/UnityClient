using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text loginText;
    [SerializeField] private TMP_Text passwordText;
    private void Awake()
    {
        loginButton.onClick.AddListener(SendCredentialsLoging);
    }

    private void SendCredentialsLoging()
    {
        Network_Manager._NETWORK_MANAGER.ConnectToServer(loginText.text.Replace("?", ""), passwordText.text.Replace("?", ""));
        ClearInputs(false);
    }

    public void ClearInputs(bool value)
    {
        loginText.text = string.Empty;
        passwordText.text = string.Empty;
    }
    private void Log()
    {
        Network_Manager._NETWORK_MANAGER.ConnectToServer(loginText.text.ToString(), passwordText.text.ToString());
    }
}
