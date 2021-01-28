using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    #region Field
    ItemManager.eCategory m_category;
    public ItemManager.eCategory Category { get { return m_category; } set { m_category = value; } }

    ItemManager.eItemType m_type;
    public ItemManager.eItemType Type { get { return m_type; } set { m_type = value; } }

    bool m_onInvenItem;
    public bool OnInvenItem { get { return m_onInvenItem; } set { m_onInvenItem = value; } }
    #endregion

    #region Public Method
    public void InitItem(ItemManager.eCategory category, ItemManager.eItemType type)
    {
        m_category = category;
        m_type = type;
    }

    public virtual void SetItem(Vector3 pos)
    {
        gameObject.SetActive(true);        

        transform.position = pos;
        transform.rotation = Quaternion.identity;
    }

    public virtual void Effect(Player player)
    {

    }
    #endregion

    #region Unity Method
    private void Awake()
    {
        OnAwake();
    }
    protected virtual void OnAwake()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }
    protected virtual void OnStart()
    {

    }
    #endregion
}
