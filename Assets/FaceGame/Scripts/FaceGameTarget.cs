using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARKit;

public class FaceGameTarget : MonoBehaviour
{
    [SerializeField]
    private ARKitBlendShapeLocation m_BlendShapeType;

    [SerializeField]
    private ParticleSystem m_ParticleOnHit;
    [SerializeField]
    private ParticleSystem m_ParticleOnMiss;
    [SerializeField]
    private ParticleSystem m_ParticleMiddle;


    private float m_StartTime;


    private void OnEnable()
    {
        m_StartTime = Time.time;
    }

    private void Update()
    {
        if ((Time.time - m_StartTime) > 1)
        {
            TargetMiss();
        }
    }

    private void TargetHit()
    {
        float hitTime = Time.time;
        float points = hitTime - Time.time;

        FaceGameManager.Instance.m_Score += points;
        
        m_ParticleOnHit.gameObject.SetActive(true);
        m_ParticleOnHit.Play();
        m_ParticleOnMiss.gameObject.SetActive(false);
        m_ParticleMiddle.gameObject.SetActive(false);
        
        StartCoroutine(WaitAndDelete(1.0f));
    }

    private void TargetMiss()
    {
        m_ParticleOnMiss.gameObject.SetActive(true);
        m_ParticleOnMiss.Play();
        StartCoroutine(WaitAndDelete(1.0f));
    }

    private IEnumerator WaitAndDelete(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == this.gameObject.tag) && 
            (FaceColliderManager.Instance.m_BlendShapeCache[m_BlendShapeType] > 0.5f))
        {
            TargetHit();
        }
    }
    
    
}
