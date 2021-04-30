using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
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
    private static FaceColliderManager s_Instance;
    public static FaceColliderManager Instance 
    { 
        get { return s_Instance; } 
    } 
    
    [SerializeField] 
    private GameObject m_ColliderPrefab;
    
    [SerializeField] private ARKitBlendShapeLocation m_EyeBlinkLeft = ARKitBlendShapeLocation.EyeBlinkLeft;
    [SerializeField] private ARKitBlendShapeLocation m_EyeBlinkRight = ARKitBlendShapeLocation.EyeBlinkRight;
    [SerializeField] private ARKitBlendShapeLocation m_TongueOut = ARKitBlendShapeLocation.TongueOut;
    
    [HideInInspector] public Dictionary<ARKitBlendShapeLocation, float> m_BlendShapeCache;
    [HideInInspector] public Dictionary<string, float> m_BlendShapeValues;
    
    ARFace m_Face;
    ARKitFaceSubsystem m_FaceSubsystem;

    [SerializeField] private GameObject m_MouthCollider;
    [SerializeField] private GameObject m_NoseCollider;
    // These two are created in script using the values from ARKit 
    private GameObject m_EyeLeftCollider;
    private GameObject m_EyeRightCollider;
     
    void Awake()
    {
        if (s_Instance != null && s_Instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        s_Instance = this;
        
        m_Face = GetComponent<ARFace>();
        m_BlendShapeValues = new Dictionary<string, float>();
        m_BlendShapeValues.Add("EyeBlinkLeft", 0);
        m_BlendShapeValues.Add("EyeBlinkRight", 0);
        m_BlendShapeValues.Add("TongueOut", 0);
        
        m_BlendShapeCache = new Dictionary<ARKitBlendShapeLocation, float>();
        m_BlendShapeCache.Add(m_EyeBlinkLeft, 0);
        m_BlendShapeCache.Add(m_EyeBlinkRight, 0);
        m_BlendShapeCache.Add(m_TongueOut, 0);
    }
    
    void CreateGameObjectsIfNecessary()
    {
        if (m_Face.leftEye != null && m_EyeLeftCollider == null )
        {
            m_EyeLeftCollider = Instantiate(m_ColliderPrefab, m_Face.leftEye);
            m_EyeLeftCollider.tag = "EyeLeft";
            m_EyeLeftCollider.SetActive(false);
        }
        if (m_Face.rightEye != null && m_EyeRightCollider == null)
        {
            m_EyeRightCollider = Instantiate(m_ColliderPrefab, m_Face.rightEye);
            m_EyeLeftCollider.tag = "EyeRight";
            m_EyeRightCollider.SetActive(false);
        }
    }

    void UpdateBlendShapeValues()
    {
        using (var blendShapes = m_FaceSubsystem.GetBlendShapeCoefficients(m_Face.trackableId, Allocator.Temp))
        {
            foreach (var featureCoefficient in blendShapes)
            {
                if (m_BlendShapeCache.TryGetValue(featureCoefficient.blendShapeLocation, out float coefficient))
                {
                    m_BlendShapeValues[featureCoefficient.blendShapeLocation.ToString()] = coefficient;
                    Debug.Log(featureCoefficient.blendShapeLocation.ToString() + " " + coefficient);
                    m_BlendShapeCache[featureCoefficient.blendShapeLocation] = featureCoefficient.coefficient;
                }
            }
        }
    }
    
    void SetVisible(bool visible)
    {
        if (m_EyeLeftCollider != null && m_EyeRightCollider != null && m_MouthCollider != null && m_NoseCollider != null)
        {
            m_EyeLeftCollider.SetActive(visible);
            m_EyeRightCollider.SetActive(visible);
            m_MouthCollider.SetActive(visible);
            m_NoseCollider.SetActive(visible);
        }
    }
    
    void OnEnable()
    {
        var faceManager = FindObjectOfType<ARFaceManager>();
        if (faceManager != null && faceManager.subsystem != null && faceManager.descriptor.supportsEyeTracking)
        {
            //m_FaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
            SetVisible((m_Face.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready));
            m_Face.updated += OnUpdated;
        }
        else
        {
            enabled = false;
        }
    }
    
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
