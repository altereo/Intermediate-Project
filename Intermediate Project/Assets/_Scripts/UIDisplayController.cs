using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class UIDisplayController : MonoBehaviour
{
    public Canvas m_SongSelectCanvas;
    public Canvas m_HealthDisplay;
    public Canvas m_PauseMenuCanvas;
    public Animator m_PlayButtonAnimator;
    public Animator m_SongDetailAninmator;
    public Animator m_SongListAnimator;
    public Animator m_TitleButtonAnimator;
    public Animator m_TitleCardAnimator;
    public GameManager m_gameManager;
    public GameObject m_BackButton;
    private UIState m_UIState;

    public UIState uiState { get { return m_UIState; } }
    public enum UIState
    {
        MainTitle,
        SongSelect,
        Playing
    }

    // Start is called before the first frame update
    void Start()
    {
        m_TitleButtonAnimator.Play("showTitleButtons", -1, 0f);
        m_HealthDisplay.gameObject.SetActive(false);
        m_SongSelectCanvas.gameObject.SetActive(false);
        m_UIState = UIState.MainTitle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackButtonPressed()
    {
        switch (uiState) {
            case UIState.SongSelect:
                // Time to reset things...
                m_UIState = UIState.MainTitle;
                m_PlayButtonAnimator.Play("hidePlayButton", m_PlayButtonAnimator.GetLayerIndex("Forward"), 0f);
                m_BackButton.SetActive(false);
                m_SongListAnimator.Play("reverseSongSelect", -1, 0f);
                m_SongDetailAninmator.Play("hideSongInfoGroup", -1, 0f);
                m_TitleButtonAnimator.Play("showTitleButtons", -1, 0f);
                m_TitleCardAnimator.Play("showTitle", -1, 0f);
                break;
            case UIState.Playing:
                // This should only be triggerable from the pause menu.
                break;
            case UIState.MainTitle:
                // Also shouldn't happen.
                break;
        }
    }

    public void ShowSongSelect()
    {
        m_BackButton.SetActive(true);
        m_UIState = UIState.SongSelect;
        m_TitleButtonAnimator.Play("hideTitleButtons", -1, 0f);
        m_TitleCardAnimator.Play("hideTitle", -1, 0f);
        m_SongSelectCanvas.gameObject.SetActive(true);
        m_PlayButtonAnimator.enabled = true;
        m_PlayButtonAnimator.Play("showPlayButton", m_PlayButtonAnimator.GetLayerIndex("Forward"), 0f);
        m_SongDetailAninmator.enabled = true;
        m_SongDetailAninmator.Play("showSongInfoGroup", -1, 0f);
        m_SongListAnimator.Play("songSelectAnimator", -1, 0f);
        m_SongSelectCanvas.GetComponent<SongSelectDisplay>().UpdateSongs();
    }

    public void SwitchToGameMode()
    {
        m_gameManager.m_GameState = GameManager.GameState.Playing;
        m_UIState = UIState.Playing;
        m_PlayButtonAnimator.Play("hidePlayButton", m_PlayButtonAnimator.GetLayerIndex("Forward"), 0f);
        m_BackButton.SetActive(false);
        m_SongListAnimator.Play("reverseSongSelect", -1, 0f);
        m_SongDetailAninmator.Play("hideSongInfoGroup", -1, 0f);
        m_HealthDisplay.gameObject.SetActive(true);
    }

    public void ShowHidePauseMenu()
    {
        if (m_gameManager.m_GameState == GameManager.GameState.Playing) {
            // If we're supposed to hide the pause menu.
            m_PauseMenuCanvas.gameObject.SetActive(false);
        } else if (m_gameManager.m_GameState == GameManager.GameState.Paused) {
            // If we're supposed to show the pause menu.
            m_PauseMenuCanvas.gameObject.SetActive(true);
        }
    }
}
