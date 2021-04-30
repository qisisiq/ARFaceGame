using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FaceGameManager : MonoBehaviour
{
    [SerializeField] private FaceGameSongData m_Song1;
    [SerializeField] private FaceGameSongData m_Song2;

    [SerializeField] private List<Transform> m_SpawnPoints;
    
    [HideInInspector] public float m_Score;
    private float m_CurrentGameStartTime;
    private FaceGameSongData m_SongData;
    private AudioSource m_AudioSource;

    private List<TargetSpawnInfo> m_SpawnInfo;
    private int m_TargetCounter = 0;
    private float m_BPS;

    private static FaceGameManager s_Instance;
    public static FaceGameManager Instance 
    { 
        get { return s_Instance; } 
    }

    [SerializeField] private GameObject m_NoseTarget;
    [SerializeField] private GameObject m_TongueTarget;
    [SerializeField] private GameObject m_LeftWinkTarget;
    [SerializeField] private GameObject m_RightWinkTarget;


    void Awake()
    {
        if (s_Instance != null && s_Instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        s_Instance = this;
        DontDestroyOnLoad(this.gameObject);

        m_AudioSource = this.gameObject.GetComponent<AudioSource>();
    }

    void RestartGame()
    {
        
    }

    public void StartGame(FaceGameSongData song)
    {
        m_Score = 0;
        m_TargetCounter = 0;
        m_CurrentGameStartTime = Time.time;
        m_SongData = song;
        
        var reorderedList = m_SongData.m_SpawnInformation.OrderBy(x => x.beat);
        m_SpawnInfo = reorderedList.ToList();
        
        m_BPS = m_SongData.m_BPM / 60f;

        m_AudioSource.clip = m_SongData.m_Song;
        m_AudioSource.time = m_SongData.m_StartTime;
        m_AudioSource.Play();
        
        Debug.Log("Starting game: " + song.name);
    }

    void EndGame()
    {
        DisplayEndGameMenu();
    }

    void DisplayEndGameMenu()
    {
        
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

            var spawnInfo = m_SpawnInfo[m_TargetCounter];
            
            float currentBeatTime = m_SongData.m_StartTime + (spawnInfo.beat * m_BPS);
            if (m_AudioSource.time > currentBeatTime)
            {
                Instantiate(spawnInfo.prefab, spawnInfo.transform);
                m_TargetCounter++;
            }
        }
    }
}
