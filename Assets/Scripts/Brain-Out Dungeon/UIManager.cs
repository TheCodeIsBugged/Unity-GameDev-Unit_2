using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; 

    public Animator menu;
    public Animator hud;
    public Animator gameover;
    private float delay = 2f;

    private void Awake()
    {
        Instance = this;
    }

    public void MenuStartCoroutine() 
    {
        string trigger = "notPlaying";
        StartCoroutine(FadesInUI(menu, trigger)); 
    }

    public void MenuEndCoroutine()
    {
        string trigger = "isPlaying";   
        StartCoroutine(FadesOutUI(menu, trigger));
    }

    public void HUDStartCoroutine()
    {
        string trigger = "isPlaying";
        StartCoroutine(FadesInUI(hud, trigger));
    }

    public void HUDEndCoroutine()
    {
        string trigger = "notPlaying";
        StartCoroutine(FadesOutUI(hud, trigger));
    }

    public void GameoverStartCoroutine()
    {
        string trigger = "hasWon";
        StartCoroutine(FadesInUI(gameover, trigger));

        HUDEndCoroutine();
    }

    public void GameoverEndCoroutine()
    {
        string trigger = "isRestarting";
        StartCoroutine(FadesOutUI(gameover, trigger));
        
        MenuStartCoroutine();
    }

    IEnumerator FadesOutUI(Animator animator, string trigger)
    {
        // Enable the animator before playing the animation
        animator.enabled = true;
        animator.SetTrigger(trigger);
        // Deactivate the UIgameobject after the animation
        yield return new WaitForSeconds(delay);
        animator.gameObject.SetActive(false);
    }

    IEnumerator FadesInUI(Animator animator, string trigger)
    {
        // Activate the UIgameobject before the animation
        animator.gameObject.SetActive(true);
        // Play the animation
        animator.SetTrigger(trigger);
        // Disable the animator after the animation
        yield return new WaitForSeconds(delay);
        animator.enabled = false;
    }
}
