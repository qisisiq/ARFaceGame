using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FaceGameManager : MonoBehaviour
{
    [SerializeField] private FaceGameSongData m_Song1;
    [SerializeField] private FaceGameSongData m_Song2;
    
    
    [HideInInspector] public float m_Score;
    private float m_CurrentGameStartTime;
    private FaceGameSongData m_SongData;
    private AudioSource m_AudioSource;

    private static FaceGameManager s_Instance;
    public static FaceGameManager Instance 
    { 
        get { return s_Instance; } 
    }


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
        m_CurrentGameStartTime = Time.time;
        m_SongData = song;

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
        }
    }
}
