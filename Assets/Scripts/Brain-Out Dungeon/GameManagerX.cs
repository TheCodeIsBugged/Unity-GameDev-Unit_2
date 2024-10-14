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
    [SerializeField]
    PlayerCollision player;
    float restartDelay = 2f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Check if the player is active in the scene
        if (player.gameObject.activeInHierarchy)
        {
            UIManager.Instance.SetPlayButtonText("RESUME");
        }
        else
        {
            UIManager.Instance.SetPlayButtonText("PLAY");
        }

        if (player.IsGameover && Input.GetKeyDown(KeyCode.Space)) 
        {
            StartCoroutine(RestartCoroutine());
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator RestartCoroutine()
    {
        UIManager.Instance.GameoverEndCoroutine();
        yield return new WaitForSeconds(restartDelay);
        RestartGame();
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
