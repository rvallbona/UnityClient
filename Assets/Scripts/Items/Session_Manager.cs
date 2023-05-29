using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Session_Manager : MonoBehaviour
{
    public static Session_Manager _SESSION_MANAGER;
    private Users currentUser;
    private Dictionary<int, Race> races = new Dictionary<int, Race>();

    private void Awake()
    {
        if (_SESSION_MANAGER != null && _SESSION_MANAGER != this)
        {
            Destroy(_SESSION_MANAGER);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _SESSION_MANAGER = this;
        }
    }

    private void Start()
    {
        Network_Manager._NETWORK_MANAGER.GetRaces();
    }

    public Users CurrentUser { get { return currentUser; } set { currentUser = value; } }
    public Dictionary<int, Race> Races { get { return races; } set { races = value; } }
}