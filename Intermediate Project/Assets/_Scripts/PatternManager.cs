using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Timers;

public class Beatmap
{
    public string beatmapName;
    public string beatmapAudioFile;
    public int tempo;
    public int difficultyIndex;
    public List<string> beatmapNotes = new List<string>();
    public List<string> beatmapLasers = new List<string>();
    public List<string> beatmapAttribution = new List<string>();
    public string beatmapDescription;
}

public class PatternManager : MonoBehaviour
{
    public BeatmapAudioLoader m_MainAudioPlayer;
    public GameManager m_GameManager;
    public bool m_AudioFileLoaded = false;
    
    public BulletSpawnController[] m_SpawnerArrayNorth;
    public BulletSpawnController[] m_SpawnerArrayEast;
    public BulletSpawnController[] m_SpawnerArraySouth;
    public BulletSpawnController[] m_SpawnerArrayWest;

    public LaserController[] m_VerticalLasers;
    public LaserController[] m_HorizontalLasers;

    string currentDirectory;
    public string m_BeatmapStorageDirectory;
    public string m_BeatmapStoragePath;
    private string[] m_BeatmapArray;
    public string[] beatmapNameArray { get { return m_BeatmapArray; } }

    private string[] m_BeatmapContents;
    public Beatmap m_CurrentBeatmap;
    public bool m_PatternLoaded = false;
    private float m_CurrentTime = 0f;

    public Dictionary<string, Beatmap> beatmapObjectCollection = new Dictionary<string, Beatmap>();

    // A place to store every characteristic of our notes.
    public List<string> m_NoteSpawnAreas = new List<string>();
    public List<int> m_NoteSpawnerIndexes = new List<int>();
    public List<int> m_NoteSpeeds = new List<int>();
    public List<float> m_NoteMsDelays = new List<float>();

    // A place to store every characteristic of the lasers.
    public List<string> m_LaserSpawnAreas = new List<string>();
    public List<int> m_LaserSpawnerIndexes = new List<int>();
    public List<float> m_LaserDurations = new List<float>();
    public List<float> m_LaserMsDelays = new List<float>();

    // Linking beatmap names to other stuff.
    public Dictionary<string, string> m_BeatmapNameFileDict = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        // Where are we?
        currentDirectory = Application.dataPath;
        Debug.Log("We are currently in: " + currentDirectory);
        m_BeatmapStoragePath = currentDirectory + "/" + m_BeatmapStorageDirectory;
        // Begin loading beatmaps.
        GetBeatmaps();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor) {
            // If we're in the debugger.
            if (Input.GetKeyDown(KeyCode.Delete)) {
                // Load up the test beatmap so we can check everything.
                LoadBeatmap("test.gdlk");
            }
        }
        // If we're ready to play...
        if (m_PatternLoaded && m_AudioFileLoaded && m_GameManager.m_GameState == GameManager.GameState.Playing) {
            // Increment the timer...
            m_CurrentTime += Time.deltaTime * 1000;
            // Then check all delays in the list for something that matches...
            for (int i = 0; i < m_NoteMsDelays.Count; i++) {
                if (m_NoteMsDelays[i] <= m_CurrentTime) {
                    // If it does, figure out where to spawn it and do so...
                    switch (m_NoteSpawnAreas[i]) {
                        case "N":
                            m_SpawnerArrayNorth[m_NoteSpawnerIndexes[i]].SpawnBullet(m_NoteSpeeds[i]);
                            break;
                        case "E":
                            m_SpawnerArrayEast[m_NoteSpawnerIndexes[i]].SpawnBullet(m_NoteSpeeds[i]);
                            break;
                        case "S":
                            m_SpawnerArraySouth[m_NoteSpawnerIndexes[i]].SpawnBullet(m_NoteSpeeds[i]);
                            break;
                        case "W":
                            m_SpawnerArrayWest[m_NoteSpawnerIndexes[i]].SpawnBullet(m_NoteSpeeds[i]);
                            break;
                        default:
                            Debug.Log("Couldn't find a location.");
                            break;
                    }
                    // Then remove it from the lists.
                    m_NoteMsDelays.RemoveAt(i);
                    m_NoteSpawnAreas.RemoveAt(i);
                    m_NoteSpawnerIndexes.RemoveAt(i);
                    m_NoteSpeeds.RemoveAt(i);
                }
            }

            // Now we need to do the same for lasers.
            for (int i = 0; i < m_LaserMsDelays.Count; i++) {
                if (m_LaserMsDelays[i] <= m_CurrentTime) {
                    if (m_LaserSpawnAreas[i] == "V") {
                        m_VerticalLasers[m_LaserSpawnerIndexes[i]].FireLaser(m_LaserDurations[i]);
                    } else if (m_LaserSpawnAreas[i] == "H") {
                        m_HorizontalLasers[m_LaserSpawnerIndexes[i]].FireLaser(m_LaserDurations[i]);
                    } else {
                        // Area is invalid.
                        Debug.Log("Invalid area for laser " + i);
                    }
                    m_LaserDurations.RemoveAt(i);
                    m_LaserMsDelays.RemoveAt(i);
                    m_LaserSpawnAreas.RemoveAt(i);
                    m_LaserSpawnerIndexes.RemoveAt(i);
                }
            }
        }
    }

    public void LoadBeatmap(string fileName)
    {
        // Make sure it exists first.
        if (!File.Exists(m_BeatmapStoragePath + "/" + fileName)) {
            Debug.LogError("Couldn\'t find the specified file. Has it moved since then?");
            return;
        }
        // Create a new instance of the beatmap.
        m_CurrentBeatmap = new Beatmap();

        // Read the file.
        StreamReader fileReader = new StreamReader(m_BeatmapStoragePath + "/" + fileName);

        // Place it in an array so we can iterate through it.
        m_BeatmapContents = fileReader.ReadToEnd().Split('\n');

        for (var i = 0; i < m_BeatmapContents.Length; i++) {
            switch (i) {
                case 0:
                    // Line zero has to be the name.
                    m_CurrentBeatmap.beatmapName = m_BeatmapContents[0].ToString();
                    Debug.Log("Loading beatmap \"" + m_BeatmapContents[0].ToString() + "\"");
                    break;
                case 1:
                    // Line one has to be the tempo. So we'll try and
                    // change it into an int.
                    int readTempo = -1;
                    bool didParseProperly = int.TryParse(m_BeatmapContents[i], out readTempo);

                    // Make sure we parsed it properly, and that it's not
                    // a negative number. Because that would be impossible.
                    if (didParseProperly && readTempo > 0) {
                        m_CurrentBeatmap.tempo = readTempo;
                        Debug.Log("Tempo is " + readTempo);
                    } else {
                        Debug.LogError("Failed to read tempo.");
                    }
                    break;
                case 2:
                    // Line 2 has to be the name of the audio file to load.
                    m_CurrentBeatmap.beatmapAudioFile = m_BeatmapContents[i];
                    m_MainAudioPlayer.PrepareAudio(m_CurrentBeatmap.beatmapAudioFile, m_BeatmapStoragePath);
                    break;
                case 4:
                    // Line 4 has to be the regular bullet placements.
                    // Remove spaces from the string and then split it by commas.
                    m_CurrentBeatmap.beatmapNotes.AddRange(m_BeatmapContents[i].Replace(" ", "").Split(','));
                    break;
                case 5:
                    // Line 5 has to be the laser definitions.
                    m_CurrentBeatmap.beatmapLasers.AddRange(m_BeatmapContents[i].Replace(" ", "").Split(','));
                    break;
                case 6:
                    // Line 6 has to be attributions, separated by semicolons
                    m_CurrentBeatmap.beatmapAttribution.AddRange(m_BeatmapContents[i].Split(';'));
                    break;
                case 7:
                    int difficultyIndex;
                    bool didParseCorrectly = int.TryParse(m_BeatmapContents[i], out difficultyIndex);
                    if (didParseCorrectly && difficultyIndex <= 3 && difficultyIndex >= -1) {
                        m_CurrentBeatmap.difficultyIndex = difficultyIndex;
                    } else {
                        // If we couldn't parse the thing, then just assign it to -1. We'll deal with it later.
                        m_CurrentBeatmap.difficultyIndex = -1;
                    }
                    break;
                default:
                    // Assuming everything else is a description.
                    m_CurrentBeatmap.beatmapDescription += m_BeatmapContents[i];
                    break;
            }
        }

        // Split up the note thing and put them into separate lists.
        foreach (string note in m_CurrentBeatmap.beatmapNotes) {
            string[] splitNote = note.Split('-');
            // Store all the data separately for now.
            m_NoteSpawnAreas.Add(splitNote[0]);
            m_NoteSpawnerIndexes.Add(int.Parse(splitNote[1]));
            m_NoteSpeeds.Add(int.Parse(splitNote[2]));
            m_NoteMsDelays.Add(float.Parse(splitNote[3]));
        }
        
        // Split up the laser format.
        foreach (string laser in m_CurrentBeatmap.beatmapLasers) {
            string[] splitNote = laser.Split('-');
            m_LaserSpawnAreas.Add(splitNote[0]);
            m_LaserSpawnerIndexes.Add(int.Parse(splitNote[1]));
            m_LaserDurations.Add(float.Parse(splitNote[2]));
            m_LaserMsDelays.Add(float.Parse(splitNote[3]));
        }

        // Once we've done that, we can begin playing!
        m_PatternLoaded = true;

        // Reset the timer.
        m_CurrentTime = 0;
    }

    void GetBeatmaps()
    {
        // Make sure the directory exists first.
        bool directoryExists = Directory.Exists(m_BeatmapStoragePath);
        // If it doesn't, create it.
        if (directoryExists) {
            Debug.Log("Found the correct directory at \"" + m_BeatmapStoragePath + "\"");
        } else {
            Directory.CreateDirectory(m_BeatmapStoragePath);
        }

        // Then check the directory for any files with the specified file extension
        m_BeatmapArray = Directory.GetFiles(m_BeatmapStoragePath, "*.gdlk");
        Debug.Log("Found " + m_BeatmapArray.Length.ToString() + " beatmaps...");

        // Get all their names.
        foreach (string beatmapPath in m_BeatmapArray) {
            string[] fileLines = File.ReadAllLines(beatmapPath);
            m_BeatmapNameFileDict.Add(fileLines[0], Path.GetFileName(beatmapPath));

            Beatmap currentlyReadingBeatmap = new Beatmap();

            for (var i = 0; i < fileLines.Length; i++) {
                switch (i) {
                    case 0:
                        // Line zero has to be the name.
                        currentlyReadingBeatmap.beatmapName = fileLines[0].ToString();
                        Debug.Log("Reading beatmap \"" + fileLines[0].ToString() + "\"");
                        break;
                    case 1:
                        // Line one has to be the tempo. So we'll try and
                        // change it into an int.
                        int readTempo = -1;
                        bool didParseProperly = int.TryParse(fileLines[i], out readTempo);

                        // Make sure we parsed it properly, and that it's not
                        // a negative number. Because that would be impossible.
                        if (didParseProperly && readTempo > 0) {
                            currentlyReadingBeatmap.tempo = readTempo;
                            Debug.Log("Tempo is " + readTempo);
                        } else {
                            Debug.LogError("Failed to read tempo.");
                        }
                        break;
                    case 2:
                        // Line 2 has to be the name of the audio file to load.
                        currentlyReadingBeatmap.beatmapAudioFile = fileLines[i];
                        break;
                    case 4:
                        // Line 4 has to be the regular bullet placements.
                        // Remove spaces from the string and then split it by commas.
                        currentlyReadingBeatmap.beatmapNotes.AddRange(fileLines[i].Replace(" ", "").Split(','));
                        break;
                    case 5:
                        // Line 5 has to be the laser definitions.
                        currentlyReadingBeatmap.beatmapLasers.AddRange(fileLines[i].Replace(" ", "").Split(','));
                        break;
                    case 6:
                        // Line 6 has to be attributions, separated by semicolons
                        currentlyReadingBeatmap.beatmapAttribution.AddRange(fileLines[i].Split(';'));
                        break;
                    case 7:
                        int difficultyIndex;
                        bool didParseCorrectly = int.TryParse(fileLines[i], out difficultyIndex);
                        if (didParseCorrectly && difficultyIndex <= 3 && difficultyIndex >= -1) {
                            currentlyReadingBeatmap.difficultyIndex = difficultyIndex;
                        } else {
                            // If we couldn't parse the thing, then just assign it to -1. We'll deal with it later.
                            currentlyReadingBeatmap.difficultyIndex = -1;
                        }
                        break;
                    case 8:
                        // Assuming line 8 is a description.
                        currentlyReadingBeatmap.beatmapDescription += fileLines[i];
                        break;
                }
            }

            // Add it to a dict so we can work with it later.
            beatmapObjectCollection.Add(fileLines[0], currentlyReadingBeatmap);
        }

    }
}
