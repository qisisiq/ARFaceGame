using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif

/// <summary>
/// Adds a game object that acts as a collider to the eyes, nose, and lips at the AR face representation
/// </summary>

[RequireComponent(typeof(ARFace))]
public class FaceColliderManager : MonoBehaviour
{
    private GameObject m_eyeLeftCollider;
    private GameObject m_eyeRightCollider;
    private GameObject m_mouthCollider;
    private GameObject m_noseCollider;

    [SerializeField] 
    private GameObject m_colliderPrefab;

    ARFace m_Face;
    XRFaceSubsystem m_FaceSubsystem;

     
    void Awake()
    {
        m_Face = GetComponent<ARFace>();
    }
    
    void CreateGameObjectsIfNecessary()
    {
        if (m_Face.leftEye != null && m_eyeLeftCollider == null )
        {
            m_eyeLeftCollider = Instantiate(m_colliderPrefab, m_Face.leftEye);
            m_eyeLeftCollider.tag = "EyeLeft";
            m_eyeLeftCollider.SetActive(false);
        }
        if (m_Face.rightEye != null && m_eyeRightCollider == null)
        {
            m_eyeRightCollider = Instantiate(m_colliderPrefab, m_Face.rightEye);
            m_eyeLeftCollider.tag = "EyeRight";
            m_eyeRightCollider.SetActive(false);
        }

        // Calculating the offset to approximate where to put the nose and mouth
        var eyeDist = Vector3.Distance(m_Face.leftEye.position, m_Face.rightEye.position);
        var noseOffset = new Vector3 (eyeDist * 0.5f, -eyeDist * 0.66f, -eyeDist * 0.33f);
        noseOffset = noseOffset * 0.3f;
        var mouthOffset = new Vector3 (eyeDist * 0.5f, -eyeDist * 1.33f, 0);
        mouthOffset = mouthOffset * 0.3f;

        
        if (m_mouthCollider == null)
        {
            m_mouthCollider = Instantiate(m_colliderPrefab, m_Face.rightEye);
            m_mouthCollider.transform.position = mouthOffset;
            m_mouthCollider.tag = "Mouth";
            m_mouthCollider.SetActive(false);
        }
        
        if (m_noseCollider == null)
        {
            m_noseCollider = Instantiate(m_colliderPrefab, m_Face.rightEye);
            m_noseCollider.transform.position = noseOffset;
            m_noseCollider.tag = "Nose";
            m_noseCollider.SetActive(false);
        }
    }
    
    void SetVisible(bool visible)
    {
        if (m_eyeLeftCollider != null && m_eyeRightCollider != null && m_mouthCollider != null && m_noseCollider != null)
        {
            m_eyeLeftCollider.SetActive(visible);
            m_eyeRightCollider.SetActive(visible);
            m_mouthCollider.SetActive(visible);
            m_noseCollider.SetActive(visible);
        }
    }
    
    void OnEnable()
    {
        var faceManager = FindObjectOfType<ARFaceManager>();
        if (faceManager != null && faceManager.subsystem != null && faceManager.descriptor.supportsEyeTracking)
        {
            m_FaceSubsystem = (XRFaceSubsystem)faceManager.subsystem;
            SetVisible((m_Face.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready));
            m_Face.updated += OnUpdated;
        }
        else
        {
            enabled = false;
        }
    }

    /*
    void Start()
    {
        if(m_colliderPrefab != null){
            m_eyeLeftCollider = Instantiate(m_colliderPrefab, m_Face.leftEye);
            m_eyeLeftCollider.tag = "EyeLeft";
            m_eyeRightCollider = Instantiate(m_colliderPrefab, m_Face.rightEye);
            m_eyeRightCollider.tag = "EyeRight";
            m_mouthCollider = Instantiate(m_colliderPrefab);
            m_mouthCollider.tag = "Mouth";
            m_noseCollider = Instantiate(m_colliderPrefab);
            m_noseCollider.tag = "Nose";
        }
    }*/
    
    
    void OnDisable()
    {
        m_Face.updated -= OnUpdated;
        SetVisible(false);
    }
    
    void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        CreateGameObjectsIfNecessary();
        SetVisible((m_Face.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready));
    }
}
