using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDebugger : MonoBehaviour
{
    private List<Renderer> m_DebugRenderers;
    private bool m_firstTime = true;
    
    void GetObjects()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("Debug");
        m_DebugRenderers = new List<Renderer>();
        foreach (var go in gameObjects)
        {
            if (go.GetComponent<Renderer>() != null)
            {
                m_DebugRenderers.Add(go.GetComponent<Renderer>());

            }
        }
    }

    public void OnToggleDebug(bool value)
    {
        GetObjects();
        m_firstTime = false;
        foreach (var renderer in m_DebugRenderers)
        {
            renderer.enabled = value;
        }
        
    }
}
