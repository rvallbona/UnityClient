using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE { MAIN_MENU, LEVEL, VICTORY, LOSE, LAST_NO_USE }

public class Scene_Manager : MonoBehaviour
{

    static Scene_Manager m_instance;
    Scene m_currentScene;
    SCENE m_currentSceneID;
    int m_numberOfTotalScenes;


    static public Scene_Manager Instance
    {
        get { return m_instance; }
        private set { }
    }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            Initialize();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Initialize()
    {
        m_numberOfTotalScenes = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        m_currentSceneID = (SCENE)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }

    public void SetCurrentScene(Scene p_scene)
    {
        m_currentScene = p_scene;
    }

    public void LoadScene(int p_scene)
    {
        if (p_scene >= m_numberOfTotalScenes)
        {
            Debug.LogError("Scene index not found. Check if all the scenes have been added to Unity Scene Manager (File -> Build Settings).");
            return;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)p_scene);
        m_currentSceneID = (SCENE)p_scene;
    }

    public void Exit()
    {
        if (!Application.isEditor) System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    public SCENE Scene { get { return m_currentSceneID; } }

}