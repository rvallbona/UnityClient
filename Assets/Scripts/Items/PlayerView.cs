using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerView
{
    TMPro.TextMeshProUGUI name;

    public PlayerView(GameObject parent)
    {
        Transform canvas = parent.transform.Find("Canvas");

        name = canvas.Find("name").GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void SetName(string value) { name.text = value; }
}
