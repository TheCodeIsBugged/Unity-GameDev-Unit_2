using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif 

public class GameManagerX : MonoBehaviour
{
    public static GameManagerX Instance;
    GameObject player;

    private void Awake()
    {
        Instance = this;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (!player.activeInHierarchy && Input.GetKeyDown(KeyCode.Space)) 
        {
            RestartGame();
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
