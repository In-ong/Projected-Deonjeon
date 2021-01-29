using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    #region Field
    [SerializeField] bool m_inGameSlotOn;
    public bool InGameSlotOn { set { m_inGameSlotOn = value; } }
    //아이템 갯수가 꽉 찼을 경우를 알려주기 위한 변수
    bool m_full;
    public bool Full { get { return m_full; } set { m_full = value; } }

    int m_count;
    public int Count { get { return m_count; } set { m_count = value; } }

    UI_Inventory m_inventory;
    Text m_keySelect;
    public Text KeySelect { get { return m_keySelect; } }
    GameObject m_selectSlotSpr;
    protected Image m_itemImage;
    public Image ItemImage { get { return m_itemImage; } }
    [SerializeField] protected Sprite[] m_itemIcon;
    protected Text m_itemCount;
    public Text ItemCount { get { return m_itemCount; } set { m_itemCount = value; } }

    //획득한 아이템을 담아놓는 리스트
    List<ItemController> m_itemList = new List<ItemController>();
    public List<ItemController> ItemList { get { return m_itemList; } }
    #endregion

    #region Public Method
    //슬롯에 아이템의 이미지와 갯수를 등록하는 메소드
    public virtual void SetItem(ItemController item)
    {
        if (!m_itemImage.gameObject.activeSelf)
        {
            m_itemImage.gameObject.SetActive(true);
            m_itemCount.transform.parent.gameObject.SetActive(true);

            for (int i = 0; i < m_itemIcon.Length; i++)
            {
                if (int.Parse(m_itemIcon[i].name.Substring(0, 1)) == (int)item.Type)
                    m_itemImage.sprite = m_itemIcon[i];
            }
            //m_count += 1;
            //m_itemCount.text = string.Format(m_count.ToString());

            m_itemList.Add(item);
            m_itemCount.text = m_itemList.Count.ToString();
        }
        else
        {
            m_itemList.Add(item);
            m_itemCount.text = m_itemList.Count.ToString();
        }
    }

    #region PreRemove Methods
    //슬롯의 개수나 아이템을 제거하는 메소드
    //public void Remove()
    //{
    //    if(m_count > 1 && m_count <= 30)
    //    {
    //        m_count -= 1;
    //        m_itemCount.text = string.Format(m_count.ToString());
    //    }
    //    else if(m_count == 1)
    //    {
    //        m_count = 0;
    //        m_itemImage.sprite = null;
    //        m_onItem = false;

    //        m_itemImage.gameObject.SetActive(false);
    //        m_itemCount.transform.parent.gameObject.SetActive(false);
    //    }
    //}
    #endregion
    //슬롯의 개수나 아이템을 제거하는 메소드
    public void Remove(Player player)
    {
        if (m_itemList.Count > 1 && m_itemList.Count <= 30)
        {
            if (player != null)
            {
                if (m_itemList[m_itemList.Count - 1].Type == ItemManager.eItemType.Hill_Potion)
                {
                    var effect = (Hill_Potion)m_itemList[m_itemList.Count - 1];
                    effect.Effect(player);
                }
                else if (m_itemList[m_itemList.Count - 1].Type == ItemManager.eItemType.Power_Potion)
                {
                    var effect = (Power_Potion)m_itemList[m_itemList.Count - 1];
                    effect.Effect(player);
                }
            }

            m_itemList.RemoveAt(m_itemList.Count - 1);
            m_itemCount.text = m_itemList.Count.ToString();
        }
        else if (m_itemList.Count == 1)
        {
            if (player != null)
            {
                if (m_itemList[m_itemList.Count - 1].Type == ItemManager.eItemType.Hill_Potion)
                {
                    var effect = (Hill_Potion)m_itemList[m_itemList.Count - 1];
                    effect.Effect(player);
                }
                else if (m_itemList[m_itemList.Count - 1].Type == ItemManager.eItemType.Power_Potion)
                {
                    var effect = (Power_Potion)m_itemList[m_itemList.Count - 1];
                    effect.Effect(player);
                }
            }

            m_itemList.RemoveAt(m_itemList.Count - 1);

            m_itemImage.sprite = null;

            m_itemImage.gameObject.SetActive(false);
            m_itemCount.transform.parent.gameObject.SetActive(false);
        }
    }
    //인게임의 아이템 슬롯과 연동하는 메소드
    public void OnInGameSlot(KeyCode key, KeyCode key2 = KeyCode.None)
    {
        if (m_selectSlotSpr.activeSelf)
        {
            if (!m_keySelect.transform.parent.gameObject.activeSelf)
            {
                m_keySelect.transform.parent.gameObject.SetActive(true);
                m_keySelect.text = key.ToString();
            }
            else
                m_keySelect.text = key.ToString();

            UI_InGame.Instance.SelectSlotItem(this, key, m_inGameSlotOn, key2);
        }
    }

    public void OnSelectSlot()
    {
        m_inventory.SelectSlot(this);
    }

    public void Select()
    {
        if(m_itemImage.gameObject.activeSelf)
            m_selectSlotSpr.SetActive(true);
    }
    public void Release()
    {
        m_selectSlotSpr.SetActive(false);
    }

    public bool IsSelected()
    {
        if (m_selectSlotSpr.activeSelf)
            return true;
        else
            return false;
    }
    public bool IsItem()
    {
        if (m_itemList.Count == 0)
            return false;
        else
            return true;
    }
    public bool FullItem()
    {
        if (m_itemList.Count == 30)
            return true;

        return false;
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Awake()
    {
        OnAwake();
    }
    protected virtual void OnAwake()
    {
        m_selectSlotSpr = GameObject.Find("SelectSlot");
        m_keySelect = Util.FindChildObject(this.gameObject, "Image_KeySelect").GetComponentInChildren<Text>();
        m_itemImage = Util.FindChildObject(this.gameObject, "Image_Item").GetComponent<Image>();
        m_inventory = GameObject.Find("Panel_Inventory").GetComponent<UI_Inventory>();
        m_itemCount = GetComponentInChildren<Text>();

        m_selectSlotSpr.SetActive(false);
        m_keySelect.transform.parent.gameObject.SetActive(false);
        m_itemImage.gameObject.SetActive(false);
        m_itemCount.transform.parent.gameObject.SetActive(false);
    }
    #endregion
}
