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
        if ((Time.time - m_StartTime) > 1.5)
        {
            TargetMiss();
        }
    }

    private void TargetHit()
    {

        FaceGameManager.Instance.IncreaseScore();
        
        m_ParticleOnHit.gameObject.SetActive(true);
        m_ParticleOnHit.Play();
        Destroy(m_ParticleOnMiss);
        Destroy(m_ParticleMiddle);
        //m_ParticleOnMiss.Stop();
        //m_ParticleMiddle.Stop();
        //m_ParticleOnMiss.gameObject.SetActive(false);
        //m_ParticleMiddle.gameObject.SetActive(false);
        
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
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (this.tag == other.tag) {
            TargetHit();
        }

        /*
                 if (this.tag == "Nose")
        {
            if (other.tag == "Nose")
            {
                TargetHit();
            }
        }
        float blendValue = FaceColliderManager.Instance.m_BlendShapeValues[m_BlendShapeType.ToString()];
        Debug.Log(m_BlendShapeType.ToString() + " : "+ blendValue);

        if ((other.tag == this.gameObject.tag) && blendValue > 0.5f)
        {
            TargetHit();
        }*/
    }
    
    
}
