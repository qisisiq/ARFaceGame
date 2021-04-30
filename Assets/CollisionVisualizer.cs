using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionVisualizer : MonoBehaviour
{
    [SerializeField] private Renderer m_Renderer;
    private Material m_Material;

    private Color m_DefaultColor;
    // Start is called before the first frame update
    void Start()
    {
        m_Material = m_Renderer.material;
        m_DefaultColor = m_Material.color;
    }

    private void OnCollisionEnter(Collision other)
    {
        m_Material.color = Color.red;
        Debug.Log(this.gameObject.tag + " collided with something");
    }

    private void OnCollisionStay(Collision other)
    {
        m_Material.color = Color.red;
    }

    private void OnCollisionExit(Collision other)
    {
        m_Material.color = m_DefaultColor;
    }
}
