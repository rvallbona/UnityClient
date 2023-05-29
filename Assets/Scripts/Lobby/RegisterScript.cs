using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScript : MonoBehaviour
{

    [SerializeField] private Button registerButton;
    [SerializeField] private TMP_Text registerText;
    [SerializeField] private TMP_Text passwordText;
    [SerializeField] private TMP_Dropdown dropDownRaces;

    private void Awake()
    {
        registerButton.onClick.AddListener(SendCredentialsRegister);

        dropDownRaces.options.Clear();

        List<string> items = new List<string>();

        foreach (KeyValuePair<int, Race> race in Session_Manager._SESSION_MANAGER.Races)
        {
            items.Add(race.Value.Name);
        }

        foreach (var item in items)
        {
            dropDownRaces.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }
    }

    private void SendCredentialsRegister()
    {
        Network_Manager._NETWORK_MANAGER.Register(registerText.text.Replace("?", ""), passwordText.text.Replace("?", ""), dropDownRaces.value + 1);
        ClearInputs();
    }

    public void ClearInputs()
    {
        registerText.text = string.Empty;
        passwordText.text = string.Empty;
    }

    public void ValueDropdown()
    {
        Debug.Log(dropDownRaces.value);
    }
}