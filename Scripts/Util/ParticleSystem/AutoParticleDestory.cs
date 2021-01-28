using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoParticleDestory : MonoBehaviour
{
    public enum eDestoryType
    {
        Inactive,
        Destory
    }
    public float m_lifeTime;
    float m_curTime;
    eDestoryType m_type;

    ParticleSystem[] m_particles;
    void RemoveParticles()
    {
        switch(m_type)
        {
            case eDestoryType.Inactive:
                gameObject.SetActive(false);
                break;
            case eDestoryType.Destory:
                Destroy(gameObject);
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_particles = GetComponentsInChildren<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if (m_lifeTime == -1f) return;

        if (m_lifeTime > 0f)
        {
            m_curTime += Time.deltaTime;
            if (m_curTime >= m_lifeTime)
            {
                RemoveParticles();
                m_curTime = 0f;
            }
        }
        else
        {
            bool isPlaying = false;
            for (int i = 0; i < m_particles.Length; i++)
            {
                if (m_particles[i].isPlaying)
                {
                    isPlaying = true;
                    break;
                }
            }
            if (!isPlaying)
            {
                RemoveParticles();
            }
        }
    }
}
