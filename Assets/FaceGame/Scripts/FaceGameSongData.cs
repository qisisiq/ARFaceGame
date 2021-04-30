using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceGameSongData : MonoBehaviour
{
    [SerializeField] public AudioClip m_Song;
    [SerializeField] public int m_BPM;
    [SerializeField] public float m_StartTime;
    [SerializeField] public float m_EndTime;
    [SerializeField] public List<Vector2> m_Positions;
    [SerializeField] public List<string> m_IconTypes;
    [SerializeField] public List<float> m_IconTimes;
}
