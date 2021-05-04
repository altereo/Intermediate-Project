using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void UpdateSongs()
    {
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
    }

    public string GetSelectedSong()
    {
        int centeredID = m_ListPositionControl.GetCenteredContentID();
        string centeredText = m_MainListBank.GetListContent(centeredID);
        return centeredText;
    }

    public void PlaySong()
    {
        // Tell the UI controller to hide any unecessary elements and switch the
        // gamemanager's state.
        m_UIDisplayController.SwitchToGameMode();
        m_PatternManager.LoadBeatmap(m_BeatmapNameFileDict[GetSelectedSong()]);
    }
}
