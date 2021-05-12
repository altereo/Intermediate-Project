using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongSelectDisplay : MonoBehaviour
{
    public PatternManager m_PatternManager;
    public UIDisplayController m_UIDisplayController;

    public Dictionary<string, string> m_BeatmapNameFileDict = new Dictionary<string, string>();
    public MainListBank m_MainListBank;
    public GameObject m_CircularList;
    public GameObject m_ButtonPrefab;
    public ListPositionCtrl m_ListPositionControl;

    public List<ListBox> m_ButtonListboxes = new List<ListBox>();
    public List<string> m_BeatmapNames = new List<string>();

    public TextMeshProUGUI m_SongTitleText;
    public TextMeshProUGUI m_SongDescriptionText;
    public TextMeshProUGUI m_SongAttributionText;

    private int previouslySelectedIndex = -1;

    public bool hasAlreadyUpdated = false;

    public void UpdateSongs()
    {
        if (hasAlreadyUpdated) return;

        // Store a copy for ease of use.
        m_BeatmapNameFileDict = m_PatternManager.m_BeatmapNameFileDict;
        foreach (string songName in m_BeatmapNameFileDict.Keys) {
            // For each song name, create a new prefab for it and add its name to the listbox.
            var button = Instantiate(m_ButtonPrefab);
            button.transform.SetParent(m_CircularList.gameObject.transform);
            button.transform.localPosition = Vector3.zero;
            button.name = songName;
            m_ButtonListboxes.Add(button.GetComponent<ListBox>());
            m_BeatmapNames.Add(songName);
        }

        // Assign the variables that external script needs for reasons that
        // I do not want to know.
        m_MainListBank.contents = m_BeatmapNames.ToArray();
        m_ListPositionControl.listBoxes = m_ButtonListboxes.ToArray();

        m_ListPositionControl.enabled = true;
        hasAlreadyUpdated = true;
    }

    public string GetSelectedSong()
    {
        int centeredID = m_ListPositionControl.GetCenteredContentID();
        string centeredText = m_MainListBank.GetListContent(centeredID);
        return centeredText;
    }

    public void PlaySong()
    {
        // If we're not already playing,
        if (m_UIDisplayController.m_gameManager.m_GameState == GameManager.GameState.Title) {
            // Tell the UI controller to hide any unecessary elements and switch the
            // gamemanager's state.
            m_UIDisplayController.SwitchToGameMode();
            m_UIDisplayController.m_gameManager.m_GameState = GameManager.GameState.Playing;
            m_PatternManager.LoadBeatmap(m_BeatmapNameFileDict[GetSelectedSong()]);
        }
    }

    private void LateUpdate()
    {
        // Update the current song data.

        if (m_ListPositionControl.GetCenteredContentID() != previouslySelectedIndex) {
            previouslySelectedIndex = m_ListPositionControl.GetCenteredContentID();
            Beatmap selectedBeatmap = m_PatternManager.beatmapObjectCollection[GetSelectedSong()];
            // Now we've got the beatmap's data and we're sure we've changed beatmaps, we can
            // start updating things in the UI.
            m_SongTitleText.text = selectedBeatmap.beatmapName;

            if (selectedBeatmap.beatmapDescription != "" && selectedBeatmap.beatmapAttribution.Count > 0) {
                m_SongDescriptionText.text = selectedBeatmap.beatmapDescription;
                m_SongAttributionText.text = string.Join("\n", selectedBeatmap.beatmapAttribution.ToArray());
            } else {
                m_SongDescriptionText.text = "";
                m_SongAttributionText.text = "";
            }
            
        }
    }
}
