using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FaceGameManager : MonoBehaviour
{
    [SerializeField] private FaceGameSongData m_Song1;
    [SerializeField] private FaceGameSongData m_Song2;

    [HideInInspector] public int m_Score;
    private FaceGameSongData m_SongData;
    private AudioSource m_AudioSource;

    private List<TargetSpawnInfo> m_SpawnInfo;
    private int m_TargetCounter = 0;
    private float m_BeatLength;

    private static FaceGameManager s_Instance;
    public static FaceGameManager Instance 
    { 
        get { return s_Instance; } 
    }

    [SerializeField] private GameObject m_NoseTarget;
    [SerializeField] private GameObject m_TongueTarget;
    [SerializeField] private GameObject m_LeftWinkTarget;
    [SerializeField] private GameObject m_RightWinkTarget;

    [SerializeField] private GameObject m_EndGameScreen;
    [SerializeField] private Text m_ScoreText;



    void Awake()
    {
        if (s_Instance != null && s_Instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        s_Instance = this;

        m_AudioSource = this.gameObject.GetComponent<AudioSource>();
    }

    public void StartGame(FaceGameSongData song)
    {
        m_Score = 0;
        m_TargetCounter = 0;
        m_SongData = song;
        
        var reorderedList = m_SongData.m_SpawnInformation.OrderBy(x => x.beat);
        m_SpawnInfo = reorderedList.ToList();
        
        m_BeatLength = 60f/m_SongData.m_BPM;

        m_AudioSource.clip = m_SongData.m_Song;
        m_AudioSource.time = m_SongData.m_StartTime;
        m_AudioSource.Play();
        
        Debug.Log("Starting game: " + song.name);
    }

    public void IncreaseScore()
    {
        m_Score++;
    }

    void EndGame()
    {
        var score = (m_Score / m_SpawnInfo.Count) * 100;
        m_ScoreText.text = score.ToString();
        DisplayEndGameMenu();
    }

    void DisplayEndGameMenu()
    {
        m_EndGameScreen.gameObject.SetActive(true);
    }

    void Update()
    {
        if (m_AudioSource.clip != null)
        {
            if (m_AudioSource.time > m_SongData.m_EndTime)
            {
                m_AudioSource.Stop();
                EndGame();
            }

            if (m_TargetCounter != -1)
            {
                var spawnInfo = m_SpawnInfo[m_TargetCounter];
            
                float currentBeatTime = m_SongData.m_StartTime + m_SongData.m_StartOffset + (spawnInfo.beat * m_BeatLength) - 1.50f;
                if (m_AudioSource.time > currentBeatTime)
                {
                    Instantiate(spawnInfo.prefab, spawnInfo.transform);
                    m_TargetCounter++;
                    if (m_TargetCounter > m_SpawnInfo.Count - 1)
                    {
                        m_TargetCounter = -1;
                    }
                }
            }
        }
    }
}
