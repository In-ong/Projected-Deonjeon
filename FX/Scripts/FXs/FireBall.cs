using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour, IFX
{
    #region Field
    [SerializeField] Monster m_monster;
    [SerializeField] MoveAnimCurve m_animCurve;
    #endregion

    #region Public Method
    public void Effect(Player player)
    {

    }
    #endregion

    #region Unity Method
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}
