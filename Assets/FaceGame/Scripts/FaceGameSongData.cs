using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceGameSongData : MonoBehaviour
{
    [SerializeField] public AudioClip m_Song;
    [SerializeField] public int m_BPM;
    [SerializeField] public float m_StartTime;
    [SerializeField] public float m_EndTime;
    [SerializeField] public float m_StartOffset;
    [SerializeField] public List<TargetSpawnInfo> m_SpawnInformation;
}

[System.Serializable]
public struct TargetSpawnInfo
{
    public Transform transform;
    public GameObject prefab;
    public float beat;
}
