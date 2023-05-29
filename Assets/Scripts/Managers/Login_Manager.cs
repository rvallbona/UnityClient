using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login_Manager : MonoBehaviour
{
    [SerializeField] Button registerButton;
    [SerializeField] Button loginButton;

    [SerializeField] GameObject registerCanvas;
    [SerializeField] GameObject loginCanvas;
    public void ChangeLoginCanvas(GameObject canvas)
    {
        canvas.gameObject.SetActive(true);
        registerCanvas.gameObject.SetActive(false);
    }
    public void ChangeRegisterCanvas(GameObject canvas)
    {
        canvas.gameObject.SetActive(true);
        loginCanvas.gameObject.SetActive(false);
    }
}
