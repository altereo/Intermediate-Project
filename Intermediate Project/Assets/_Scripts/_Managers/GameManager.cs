using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Just some game state stuff for now.
    public enum GameState
    {
        Title,
        Playing,
        Paused,
        GameOver
    };
    public GameState m_GameState;
    public GameState State { get { return m_GameState; } }

    public BeatmapAudioLoader beatmapAudioLoader;
    public PatternManager patternManager;
    public AudioSource m_audioSource;
    public PlayerGridlockController playerController;

    public UIDisplayController m_UIController;

    // Start is called before the first frame update
    void Start()
    {
        // Start the gamestate at title.
        m_GameState = GameState.Title;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_GameState) {
            case GameState.GameOver:
                beatmapAudioLoader.StopClip();
                break;
        }

        // Here's our pausing logic.
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (m_GameState == GameState.Playing) {
                // Set the timescale to 0 and change the gamestate.
                Time.timeScale = 0;
                m_GameState = GameState.Paused;

                m_UIController.ShowHidePauseMenu();

                // Pause the audio as well.
                m_audioSource.Pause();
            } else if (m_GameState == GameState.Paused) {
                // Set the timescale to its default (1) and change the gamestate.
                Time.timeScale = 1;
                m_GameState = GameState.Playing;

                m_UIController.ShowHidePauseMenu();

                // Resume the audio.
                m_audioSource.UnPause();
            }
        }
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void ShowSongSelect()
    {
        m_UIController.ShowSongSelect();
    }
}
