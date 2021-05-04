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
        GameOver
    };
    public GameState m_GameState;
    public GameState State { get { return m_GameState; } }

    public BeatmapAudioLoader beatmapAudioLoader;
    public PatternManager patternManager;
    public PlayerGridlockController playerController;

    public UIDisplayController m_UIController;

    // Start is called before the first frame update
    void Start()
    {
        // For the time being, it will simply start at playing.
        m_GameState = GameState.Playing;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_GameState) {
            case GameState.GameOver:
                beatmapAudioLoader.StopClip();
                break;
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
