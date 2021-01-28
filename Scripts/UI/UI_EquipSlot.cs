using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipSlot : UI_ItemSlot
{
    #region Field
    bool m_onItem;
    public bool OnItem { get { return m_onItem; } }

    UI_Inventory.eEquipSlotCategory m_category;
    public UI_Inventory.eEquipSlotCategory Category { get { return m_category; } }
    #endregion

    #region Public Method
    //주 무기를 교체했을 때, 슬롯의 아이템 이미지를 변경하는 메소드
    public void Change(ItemController item = null)
    {
        //서브 무기나 주 무기가 없는 상황에서 교체할 경우 이미지를 없애도록 함.
        if (item == null)
        {
            m_itemImage.sprite = null;
            m_itemImage.gameObject.SetActive(false);

            m_onItem = false;
        }
        else
        {
            for (int i = 0; i < m_itemIcon.Length; i++)
            {
                if (int.Parse(m_itemIcon[i].name.Substring(0, 1)) == (int)item.Type)
                    m_itemImage.sprite = m_itemIcon[i];
            }
        }
    }
    //장비 슬롯이 생성될 때, 각 슬롯의 종류와 이름을 정하는 메소드
    public void InitEquipSlot(UI_Inventory.eEquipSlotCategory category)
    {
        m_category = category;

        if (category == UI_Inventory.eEquipSlotCategory.Main)
            m_itemCount.text = string.Format("메인 장비");
        else if (category == UI_Inventory.eEquipSlotCategory.Second)
            m_itemCount.text = string.Format("고정 장비");
        else if (category == UI_Inventory.eEquipSlotCategory.Sub)
            m_itemCount.text = string.Format("서브 장비");
    }
    //장비 슬롯에는 갯수 등록이 필요 없음으로 삭제
    public override void SetItem(ItemController item)
    {
        m_itemImage.gameObject.SetActive(true);
        m_itemCount.transform.parent.gameObject.SetActive(true);

        for (int i = 0; i < m_itemIcon.Length; i++)
        {
            if (int.Parse(m_itemIcon[i].name.Substring(0, 1)) == (int)item.Type)
                m_itemImage.sprite = m_itemIcon[i];
        }

        m_onItem = true;
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    protected override void OnAwake()
    {
        m_itemImage = GameObject.Find("Image_Equip").GetComponent<Image>();
        m_itemIcon = Resources.LoadAll<Sprite>("SlotImage/Equip");
        m_itemCount = GetComponentInChildren<Text>();

        m_itemImage.gameObject.SetActive(false);
    }
    #endregion
}
