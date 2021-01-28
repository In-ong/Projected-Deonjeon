using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Field
    Animator[] m_animator;
    #endregion

    #region Public Method
    public void OpenDoor()
    {
        for(int i = 0; i < m_animator.Length; i++)
        {
            m_animator[i].SetBool("IsOpen", true);
        }
    }

    public void CloseDoor()
    {
        for (int i = 0; i < m_animator.Length; i++)
        {
            m_animator[i].SetBool("IsOpen", false);
        }
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Awake()
    {
        m_animator = GetComponentsInChildren<Animator>();
    }
    #endregion
}
