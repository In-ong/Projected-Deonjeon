using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    #region Enum
    //장비슬롯의 종류
    public enum eEquipSlotCategory
    {
        Main,
        Second,
        Sub
    }
    #endregion

    #region Field
    int m_equipCategoryNum;
    int m_consumeSlotCount = 12;
    int m_equipSlotCount = 3;

    GameObject m_backGround;
    [SerializeField] GameObject m_consumeSlot;
    [SerializeField] GameObject m_equipSlot;
    [SerializeField] GameObject m_consumeWindow;
    [SerializeField] GameObject m_equipWindow;

    List<UI_ItemSlot> m_consumeSlotList = new List<UI_ItemSlot>();
    //UI_EquipSlot이 UI_ItemSlot를 상속받기는 했지만 각각 다른 창에서 쓰이기 때문에 하나의 리스트에 넣지 않고 따로 만들어준다
    List<UI_EquipSlot> m_equipSlotList = new List<UI_EquipSlot>();
    public List<UI_EquipSlot> EquipSlotList { get { return m_equipSlotList; } }
    #endregion

    #region Public Method

    #region Create Slot
    //소비창의 슬롯과 장비창의 슬롯을 인스턴트화하는 메소드
    public void AddConsumeSlot()
    {
        var obj = Instantiate(m_consumeSlot) as GameObject;
        obj.transform.SetParent(m_consumeWindow.transform);
        obj.transform.localScale = Vector3.one;
        var slot = obj.GetComponent<UI_ItemSlot>();
        m_consumeSlotList.Add(slot);
    }
    public void AddEquipSlot()
    {
        var obj = Instantiate(m_equipSlot) as GameObject;
        obj.transform.SetParent(m_equipWindow.transform);
        obj.transform.localScale = Vector3.one;
        var slot = obj.GetComponent<UI_EquipSlot>();
        slot.InitEquipSlot((eEquipSlotCategory)m_equipCategoryNum++);
        m_equipSlotList.Add(slot);
    }

    //소비창과 장비창의 슬롯들을 설정해둔 슬롯 수만큼 만드는 메소드
    public void InitConsumeSlot()
    {
        for(int i = 0; i < m_consumeSlotCount; i++)
        {
            AddConsumeSlot();
        }
    }
    public void InitEquipSlot()
    {
        for (int i = 0; i < m_equipSlotCount; i++)
        {
            AddEquipSlot();
        }
    }
    #endregion

    //슬롯의 이미지를 바꾸는 메소드
    public void ChangeSlot(ItemController item, eEquipSlotCategory category)
    {
        //카테고리 순서대로 리스트에 등록되어 있기에 카테고리를 키로 쓰면 됨
        m_equipSlotList[(int)category].Change(item);
    }

    //아이템을 먹었을 때, 인벤토리에 그 아이템에 대한 정보가 갱신되는 메소드
    public void GetItem(ItemController item)
    {
        //장비 슬롯이 갱신되는 코드
        if(item.Category == ItemManager.eCategory.Euipment)
        {
            //아이템 종류별로 슬롯 종류를 나눔.
            if (item.Type == ItemManager.eItemType.Sword || item.Type == ItemManager.eItemType.Bow)
            {
                for (int i = 0; i < m_equipSlotList.Count; i++)
                {
                    //켜져 있지 않은 것 중 가장 첫 번째는 메인이 열리고 다음 아이템을 획득하면 서브가 열리도록 설정.
                    if (!m_equipSlotList[i].OnItem)
                    {
                        if (m_equipSlotList[i].Category == eEquipSlotCategory.Main)
                        {
                            m_equipSlotList[i].SetItem(item);
                            break;
                        }
                        else if(m_equipSlotList[i].Category == eEquipSlotCategory.Sub)
                        {
                            m_equipSlotList[i].SetItem(item);
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_equipSlotList.Count; i++)
                {
                    //주 무기 이외에는 모두 세컨드로 가도록 설정
                    if (!m_equipSlotList[i].OnItem)
                    {
                        if (m_equipSlotList[i].Category == eEquipSlotCategory.Second)
                        {
                            m_equipSlotList[i].SetItem(item);
                            break;
                        }
                    }
                }
            }
        }
        //소비 슬롯이 갱신되는 코드
        else if (item.Category == ItemManager.eCategory.Consume)
        {
            #region 슬롯에서 아이템을 관리하지 않는 코드
            /*
            //메소드를 끝내기 위한 변수
            bool finish = false;

            //우선 인벤토리 리스트에 아이템이 하나도 없는지 확인
            if (ItemManager.Instance.InvenItemList.Count > 0)
            {
                for (int i = 0; i < ItemManager.Instance.InvenItemList.Count; i++)
                {
                    //이후 인벤토리 리스트에 아이템과 같은 것이 있는지 확인
                    if (ItemManager.Instance.InvenItemList[i].Type == item.Type && !finish)
                    {
                        for (int j = 0; j < m_consumeSlotList.Count; j++)
                        {
                            //슬롯이 켜져 있고 그 슬롯의 이미지가 아이템의 이미지이며 아이템 수가 30개 미만으로 다 안 차 있을 것, 마지막으로 끝나지 않을 것
                            if (m_consumeSlotList[j].OnItem && int.Parse(m_consumeSlotList[j].ItemImage.sprite.name.Substring(0, 1)) == (int)item.Type && m_consumeSlotList[j].Count < 30 && !m_consumeSlotList[j].Full && !finish) 
                            {
                                m_consumeSlotList[j].Count += 1;
                                //if(m_consumeSlotList[j].Count > 30)
                                //    m_consumeSlotList[j].Count = 30;
                                m_consumeSlotList[j].ItemCount.text = m_consumeSlotList[j].Count.ToString();
                                finish = true;
                                //break를 쓰지 않으면 인벤토리 리스트에 아이템이 저장되지 않음
                                break;
                            }
                            else if (m_consumeSlotList[j].OnItem && int.Parse(m_consumeSlotList[j].ItemImage.sprite.name.Substring(0, 1)) == (int)item.Type && m_consumeSlotList[j].Count >= 30 && !m_consumeSlotList[j].Full && !finish)
                            {
                                m_consumeSlotList[j].Full = true;
                                for (int k = 0; k < m_consumeSlotList.Count; k++)
                                {
                                    if (!m_consumeSlotList[k].OnItem)
                                    {
                                        m_consumeSlotList[k].SetItem(item);
                                        finish = true;
                                        //break를 쓰지 않으면 인벤토리 리스트에 아이템이 저장되지 않음
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //가장 마지막 순서까지 일치하지 없는 경우 생성하도록 함. 버그가 있을지 없을지는 확인해봐야 함.
                    else if(ItemManager.Instance.InvenItemList[i].Type != item.Type && !finish && i == ItemManager.Instance.InvenItemList.Count - 1)
                    {
                        for (int j = 0; j < m_consumeSlotList.Count; j++)
                        {
                            if (!m_consumeSlotList[j].OnItem)
                            {
                                m_consumeSlotList[j].SetItem(item);
                                finish = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < m_consumeSlotList.Count; j++)
                {
                    if (!m_consumeSlotList[j].OnItem)
                    {
                        m_consumeSlotList[j].SetItem(item);
                        break;
                    }
                }
            }*/
            #endregion

            for(int i = 0; i < m_consumeSlotList.Count; i++)
            {
                if(m_consumeSlotList[i].IsItem())
                {
                    if(m_consumeSlotList[i].ItemList[m_consumeSlotList[i].ItemList.Count - 1].Type == item.Type)
                    {
                        if (!m_consumeSlotList[i].FullItem())
                        {
                            m_consumeSlotList[i].SetItem(item);
                            UI_InGame.Instance.PlusItem(m_consumeSlotList[i]);
                            break;
                        }
                    }
                }
                else
                {
                    m_consumeSlotList[i].SetItem(item);
                    UI_InGame.Instance.PlusItem(m_consumeSlotList[i]);
                    break;
                }
            }
        }
    }
    //소비 아이테 사용 시 카운트를 줄이거나 개수를 줄이는 메소드
    public void RemoveSlotItem(ItemController item, Player player)
    {
        #region  슬롯에서 아이템을 관리하지 않는 코드
        /*
        for (int i = 0; i < m_consumeSlotList.Count; i++)
        {
            if (m_consumeSlotList[i].OnItem)
            {
                if (int.Parse(m_consumeSlotList[i].ItemImage.sprite.name.Substring(0, 1)) == (int)item.Type)
                {
                    if(m_consumeSlotList[i].Count == 30)
                    {
                        bool on = true;
                        //현재 슬롯의 아이템 개수가 꽉 찼을 때 다른 슬롯에 같은 아이템이 들어있는지 확인.
                        for(int j = i + 1; j < m_consumeSlotList.Count; j++)
                        {
                            if (m_consumeSlotList[j].OnItem)
                                if (int.Parse(m_consumeSlotList[j].ItemImage.sprite.name.Substring(0, 1)) == (int)item.Type)
                                {
                                    on = false;
                                    break;
                                }
                        }
                        //없다면 현재 슬롯의 아이템을 소모
                        if(on)
                        {
                            //m_consumeSlotList[i].Remove();
                            break;
                        }
                    }
                    else if (m_consumeSlotList[i].Count < 30)
                    {
                        //m_consumeSlotList[i].Remove();
                        break;
                    }
                }
            }
        }*/
        #endregion

        for(int i = 0; i < m_consumeSlotList.Count; i++)
        {
            if(m_consumeSlotList[i].IsItem())
            {
                if (m_consumeSlotList[i].ItemList[m_consumeSlotList[i].ItemList.Count - 1].Type == item.Type)
                    m_consumeSlotList[i].Remove(player);
            }
        }
    }

    public void SelectSlot(UI_ItemSlot slot)
    {
        for(int i = 0; i < m_consumeSlotList.Count; i++)
        {
            if (m_consumeSlotList[i].IsSelected())
                m_consumeSlotList[i].Release();
        }
        slot.Select();
    }

    public void InGameSlotOn(KeyCode key, KeyCode key2 = KeyCode.None)
    {
        for (int i = 0; i < m_consumeSlotList.Count; i++)
        {
            if (m_consumeSlotList[i].IsSelected())
                m_consumeSlotList[i].OnInGameSlot(key, key2);
        }
    }

    public ItemController SetItem(ItemManager.eItemType type)
    {
        for(int i = 0; i < m_consumeSlotList[i].Count; i++)
        {
            if(m_consumeSlotList[i].IsItem())
            {
                if (m_consumeSlotList[i].ItemList[m_consumeSlotList[i].ItemList.Count - 1].Type == type)
                {
                    var item = m_consumeSlotList[i].ItemList[m_consumeSlotList[i].ItemList.Count - 1];
                    m_consumeSlotList[i].Remove(null);
                    return item;
                }
            }
        }

        return null;
    }

    //인벤토리를 열고 닫는 메소드
    public void OpenInventory()
    {
        gameObject.SetActive(true);
        //m_backGround.SetActive(true);
    }
    public void ExitInvetory()
    {
        for (int i = 0; i < m_consumeSlotList.Count; i++)
        {
            if (m_consumeSlotList[i].IsSelected())
                m_consumeSlotList[i].Release();
        }

        gameObject.SetActive(false);
        //m_backGround.SetActive(false);
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Awake()
    {
        m_backGround = GameObject.Find("UI_BackGround");
        m_consumeWindow = GameObject.Find("Consume_Content");
        m_equipWindow = GameObject.Find("Equipment");

        InitConsumeSlot();
        InitEquipSlot();

        gameObject.SetActive(false);
        //m_backGround.SetActive(false);
    }
    #endregion
}
