using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplayController : MonoBehaviour
{
    public Canvas m_MainCanvas;
    public Canvas m_SongSelectCanvas;
    public Canvas m_HealthDisplay;
    public Animator m_PlayButtonBackAnimator;
    public Animator m_PlayButtonAnimator;
    public GameManager m_gameManager;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCanvas.gameObject.SetActive(true);
        m_HealthDisplay.gameObject.SetActive(false);
        m_SongSelectCanvas.gameObject.SetActive(false);

        // Set the positions up.
        m_PlayButtonAnimator.gameObject.transform.position = new Vector3(-180, 0, 0);
        m_PlayButtonBackAnimator.gameObject.transform.position = new Vector3(-180, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSongSelect()
    {
        m_MainCanvas.gameObject.SetActive(false);
        m_SongSelectCanvas.gameObject.SetActive(true);
        m_PlayButtonBackAnimator.enabled = true;
        m_PlayButtonAnimator.enabled = true;
        m_SongSelectCanvas.GetComponent<SongSelectDisplay>().UpdateSongs();
    }

    public void SwitchToGameMode()
    {
        m_gameManager.m_GameState = GameManager.GameState.Playing;
        m_MainCanvas.gameObject.SetActive(false);
        m_SongSelectCanvas.gameObject.SetActive(false);
        m_PlayButtonAnimator.enabled = false;
        m_HealthDisplay.gameObject.SetActive(true);
        m_PlayButtonAnimator.gameObject.transform.position = new Vector3(-180, 0, 0);
        m_PlayButtonBackAnimator.gameObject.transform.position = new Vector3(-180, 0, 0);
    }
}
