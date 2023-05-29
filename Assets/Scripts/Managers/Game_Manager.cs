using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager _GAME_MANAGER;

    [SerializeField]
    private GameObject spawnPlayer1;

    [SerializeField]
    private GameObject spawnPlayer2;

    private void Awake()
    {
        if (_GAME_MANAGER != null && _GAME_MANAGER != this)
        {
            Destroy(_GAME_MANAGER);
        }
        else
        {
            _GAME_MANAGER = this;
            DontDestroyOnLoad(gameObject);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Player", spawnPlayer1.transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Player", spawnPlayer2.transform.position, Quaternion.identity);
        }
    }

    //------GETTERS
    public Vector3 GetPositionSpawnPlayer1() => this.spawnPlayer1.transform.position;
    public Vector3 GetPositionSpawnPlayer2() => this.spawnPlayer2.transform.position;
}
