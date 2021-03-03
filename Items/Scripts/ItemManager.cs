using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingleTonMonoBehaviour<ItemManager>
{
    #region Enum
    public enum eCategory
    {
        None = -1,
        Euipment,
        Consume
    }

    public enum eItemType
    {
        None = -1,
        Treasure_Box,
        Sword,
        Shield,
        Bow,
        Arrow,
        Hill_Potion,
        Power_Potion,
        Heart_UP
    }
    #endregion

    #region Field
    [SerializeField] UI_Inventory m_inventory;
    public UI_Inventory Inventory { get { return m_inventory; } }
    [SerializeField] GameObject[] m_itemPrefab;

    //필드에 나온 아이템을 관리하기 위한 리스트
    List<ItemController> m_fieldItemList = new List<ItemController>();
    //보물상자에 들어 있는 아이템을 관리하기 위한 리스트
    List<ItemController> m_treasureList = new List<ItemController>();
    public List<ItemController> TreasureList { get { return m_treasureList; } set { m_treasureList = value; } }
    //인벤토리에 있는 아이템을 관리하기 위한 리스트
    List<ItemController> m_invenItemList = new List<ItemController>();
    public List<ItemController> InvenItemList { get { return m_invenItemList; } set { m_invenItemList = value; } }

    //인스턴스한 장비 아이템을 모아놓은 리스트
    List<ItemController> m_equipmentItemList = new List<ItemController>();
    //인스턴스한 소비 아이템의 ObjectPool을 모아놓은 Dictionary. Key로 Type을 사용.
    Dictionary<eItemType, GameObjectPool<ItemController>> m_consumeItemDic = new Dictionary<eItemType, GameObjectPool<ItemController>>();
    #endregion

    #region Public Method

    #region Create Func
    //소비 아이템을 생성하는 메소드
    public void CreateConsumeItem(Vector3 pos)
    {
        //타입은 화살부터 파워포션까지 랜덤 함수를 이용하여 랜덤으로 나타나게 함.
        var type = Random.Range((int)eItemType.Arrow, (int)eItemType.Heart_UP);

        var consume = m_consumeItemDic[(eItemType)type].Get();
        consume.SetItem(pos);
        m_fieldItemList.Add(consume);
    }

    public void CreateEquipItem(eItemType itemType, Transform pos)
    {
        for(int i = 0; i < m_equipmentItemList.Count; i++)
        {
            if(m_equipmentItemList[i].Type == itemType)
            {
                m_equipmentItemList[i].gameObject.SetActive(true);

                m_equipmentItemList[i].transform.SetParent(pos);
                m_equipmentItemList[i].transform.localPosition = Vector3.zero;
                m_equipmentItemList[i].transform.localRotation = Quaternion.identity;
                m_equipmentItemList[i].transform.localScale = Vector3.one;

                m_invenItemList.Add(m_equipmentItemList[i]);
                m_inventory.GetItem(m_equipmentItemList[i]);
            }
        }
    }

    //보물상자에 아이템을 넣어서 생성하는 메소드
    public void CreateTreasureBox(eCategory category, Vector3 pos)
    {
        var box = (Treasure_Box)m_consumeItemDic[eItemType.Treasure_Box].Get();

        //카테고리로 나누어서 소스를 진행
        if(category == eCategory.Consume)
        {
            var item = m_consumeItemDic[eItemType.Heart_UP].Get();
            box.SetItem(pos);
            //보물상자에 넣는 아이템은 TreasureList로
            m_treasureList.Add(item);
            //보물상자 자체는 FieldItemList로
            m_fieldItemList.Add(box);
        }
        else if(category == eCategory.Euipment)
        {            
            int num = 0; //이미 존재하고 있는 장비 아이템의 수를 확인하는 변수

            //인벤토리와 보물 상자에 장비 아이템이 전부 들어가 있는지 확인하는 코드
            for (int i = 0; i < m_invenItemList.Count; i++)
            {
                if(m_invenItemList[i].Type == eItemType.Sword)
                {
                    num++;
                }
                else if(m_invenItemList[i].Type == eItemType.Shield)
                {
                    num++;
                }
                else if(m_invenItemList[i].Type == eItemType.Bow)
                {
                    num++;
                }

                if (num >= 3)
                {
                    Debug.Log("더 이상 얻을 수 있는 장비가 없습니다.");
                    return;
                }
            }
            for (int i = 0; i < m_treasureList.Count; i++)
            {
                if (m_treasureList[i].Type == eItemType.Sword)
                {
                    num++;
                }
                else if (m_treasureList[i].Type == eItemType.Shield)
                {
                    num++;
                }
                else if (m_treasureList[i].Type == eItemType.Bow)
                {
                    num++;
                }

                if (num >= 3)
                {
                    Debug.Log("더 이상 얻을 수 있는 장비가 없습니다.");
                    return;
                }
            }

            bool isItem = false; //아이템이 있는 경우를 확인하기 위한 bool
            bool next = true; //보물상자 안에 없는 경우 그 다음 인벤토리에 있는지 확인하는 절차를 실행하기 위한 bool

            var type = (eItemType)Random.Range((int)eItemType.Sword, (int)eItemType.Arrow);

            do
            {
                //다음 반복될 때 isItem이 true임으로 type을 다시 정해줄 수 있다
                //새로 정한 type을 비교대상으로 한다
                if(isItem)
                    type = (eItemType)Random.Range((int)eItemType.Sword, (int)eItemType.Arrow);

                //isItem은 미리 false로 만들어 놓는다.
                isItem = false;

                //보물상자에 똑같은 아이템이 있는지 확인하기 위한 for문
                for (int i = 0; i < m_treasureList.Count; i++)
                {
                    //보물상자에 똑같은 아이템이 있으면 isItem을 true로 하여 반복
                    //next는 false로 하여 인벤토리 확인은 생략한다
                    if (m_treasureList[i].Type == type)
                    {
                        isItem = true;
                        next = false;
                    }
                }

                //보물상자 확인 후 일치하는 아이템이 없으면 인벤토리 확인 실행
                if (next)
                {
                    //인벤토리에 똑같은 아이템이 있는지 확인하기 위한 for문
                    for (int i = 0; i < m_invenItemList.Count; i++)
                    {
                        if (m_invenItemList[i].Type == type)
                        {
                            //인벤토리에 똑같은 아이템 있으면 isItem을 true로 하여 반복
                            //다음 반복을 위해 next는 true로 만들어 놓는다
                            isItem = true;
                            next = true;
                        }
                    }
                }
            } while (isItem);

            for(int i = 0; i < m_equipmentItemList.Count; i++)
            {
                if(m_equipmentItemList[i].Type == type)
                {
                    var item = m_equipmentItemList[i];
                    box.SetItem(pos);
                    m_treasureList.Add(item);
                    m_fieldItemList.Add(box);
                }
            }
        }
    }
    #endregion

    #region Remove Func
    //소비 아이템을 제거하는 메소드
    public void RemoveConsumeItem(ItemController item, Player player = null)
    {
        //인벤토리에서 꺼낸 아이템인지 아닌지 판단.
        if(!item.OnInvenItem)
        {
            //제거 시 효과가 있는 아이템이라면 효과 적용
            //if(item.Type == eItemType.Hill_Potion)
            //{
            //    var effect = (Hill_Potion)item;
            //    effect.Effect(player);
            //}
            //else if (item.Type == eItemType.Power_Potion)
            //{
            //    var effect = (Power_Potion)item;
            //    effect.Effect(player);
            //}

            item.gameObject.SetActive(false);
            if (m_fieldItemList.Remove(item))
            {
                if (!item.OnInvenItem)
                    item.OnInvenItem = true;

                m_inventory.GetItem(item);
                //필드에 있는 아이템을 먹으면 인벤토리 리스트에 넣어준다.
                //m_invenItemList.Add(item);

                //if (item.Type == eItemType.Arrow)
                //{
                //    m_inventory.GetItem(item);
                //    //필드에 있는 아이템을 먹으면 인벤토리 리스트에 넣어준다.
                //    m_invenItemList.Add(item);
                //}
                m_consumeItemDic[item.Type].Set(item);
            }
        }
        else
        {
            if (item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(false);

                m_consumeItemDic[item.Type].Set(item);
            }

            m_inventory.RemoveSlotItem(item, player);

            if (item.OnInvenItem)
                item.OnInvenItem = false;
        }
    }

    public void RemoveTreasureBox(Treasure_Box box, Player player)
    {
        box.GetTreasure(player);

        box.gameObject.SetActive(false);
        if (m_fieldItemList.Remove(box))
            m_consumeItemDic[box.Type].Set(box);
    }
    #endregion

    #region Player Item Control
    public void ChangeWeapon(Player player)
    {
        //Player의 Shield를 제외한 Dummy들 중 어떤 것이 활성화되어 있는지 확인
        if(player.Sword.activeSelf)
        {
            for(int i = 0; i < m_invenItemList.Count; i++)
            {
                if(m_invenItemList[i].Type == eItemType.Sword)
                {
                    player.Acceptant_Sword.SetActive(true);
                    player.Sword.SetActive(false);
                    m_invenItemList[i].transform.SetParent(player.Acceptant_Sword.transform);
                    m_invenItemList[i].transform.localPosition = Vector3.zero;
                    m_invenItemList[i].transform.localRotation = Quaternion.identity;
                    m_invenItemList[i].transform.localScale = Vector3.one;

                    if (!m_inventory.EquipSlotList[(int)UI_Inventory.eEquipSlotCategory.Sub].OnItem)
                    {
                        m_inventory.GetItem(m_invenItemList[i]);
                        m_inventory.ChangeSlot(null, UI_Inventory.eEquipSlotCategory.Main);
                    }
                    else
                        m_inventory.ChangeSlot(m_invenItemList[i], UI_Inventory.eEquipSlotCategory.Sub);
                }
                else if (m_invenItemList[i].Type == eItemType.Bow)
                {
                    player.Bow.SetActive(true);
                    player.Acceptant_Bow.SetActive(false);
                    m_invenItemList[i].transform.SetParent(player.Bow.transform);
                    m_invenItemList[i].transform.localPosition = Vector3.zero;
                    m_invenItemList[i].transform.localRotation = Quaternion.identity;
                    m_invenItemList[i].transform.localScale = Vector3.one;

                    m_inventory.ChangeSlot(m_invenItemList[i], UI_Inventory.eEquipSlotCategory.Main);
                }
            }
        }
        //수납 Dummy의 if문은 무기를 한 가지 밖에 가지고 있지 않은 상황에 쓰기 위함
        //수납 Dummy의 활성화는 물론 소유 무기가 없는지도 확인
        else if (player.Acceptant_Sword.activeSelf && !player.Bow.activeSelf)
        {
            for (int i = 0; i < m_invenItemList.Count; i++)
            {
                if (m_invenItemList[i].Type == eItemType.Sword)
                {
                    player.Sword.SetActive(true);
                    player.Acceptant_Sword.SetActive(false);
                    m_invenItemList[i].transform.SetParent(player.Sword.transform);
                    m_invenItemList[i].transform.localPosition = Vector3.zero;
                    m_invenItemList[i].transform.localRotation = Quaternion.identity;
                    m_invenItemList[i].transform.localScale = Vector3.one;

                    m_inventory.GetItem(m_invenItemList[i]);
                    m_inventory.ChangeSlot(null, UI_Inventory.eEquipSlotCategory.Sub);
                }                
            }
        }
        else if (player.Bow.activeSelf)
        {
            for (int i = 0; i < m_invenItemList.Count; i++)
            {
                if (m_invenItemList[i].Type == eItemType.Sword)
                {
                    player.Sword.SetActive(true);
                    player.Acceptant_Sword.SetActive(false);
                    m_invenItemList[i].transform.SetParent(player.Sword.transform);
                    m_invenItemList[i].transform.localPosition = Vector3.zero;
                    m_invenItemList[i].transform.localRotation = Quaternion.identity;
                    m_invenItemList[i].transform.localScale = Vector3.one;

                    m_inventory.ChangeSlot(m_invenItemList[i], UI_Inventory.eEquipSlotCategory.Main);
                }
                else if (m_invenItemList[i].Type == eItemType.Bow)
                {
                    player.Acceptant_Bow.SetActive(true);
                    player.Bow.SetActive(false);
                    m_invenItemList[i].transform.SetParent(player.Acceptant_Bow.transform);
                    m_invenItemList[i].transform.localPosition = Vector3.zero;
                    m_invenItemList[i].transform.localRotation = Quaternion.identity;
                    m_invenItemList[i].transform.localScale = Vector3.one;

                    if (!m_inventory.EquipSlotList[(int)UI_Inventory.eEquipSlotCategory.Sub].OnItem)
                    {
                        m_inventory.GetItem(m_invenItemList[i]);
                        m_inventory.ChangeSlot(null, UI_Inventory.eEquipSlotCategory.Main);
                    }
                    else
                        m_inventory.ChangeSlot(m_invenItemList[i], UI_Inventory.eEquipSlotCategory.Sub);
                }
            }
        }
        else if (player.Acceptant_Bow.activeSelf && !player.Sword.activeSelf)
        {
            for (int i = 0; i < m_invenItemList.Count; i++)
            {
                if (m_invenItemList[i].Type == eItemType.Bow)
                {
                    player.Bow.SetActive(true);
                    player.Acceptant_Bow.SetActive(false);
                    m_invenItemList[i].transform.SetParent(player.Bow.transform);
                    m_invenItemList[i].transform.localPosition = Vector3.zero;
                    m_invenItemList[i].transform.localRotation = Quaternion.identity;
                    m_invenItemList[i].transform.localScale = Vector3.one;

                    m_inventory.GetItem(m_invenItemList[i]);
                    m_inventory.ChangeSlot(null, UI_Inventory.eEquipSlotCategory.Sub);
                }
            }
        }
    }

    public void ShotArrow(Player player)
    {
        Arrow arrow = null;

        #region 인벤토리 리스트 사용 시 코드
        //for(int i = 0; i < m_invenItemList.Count; i++)
        //{
        //    if (m_invenItemList[i].Type == eItemType.Arrow)
        //    {
        //        arrow = (Arrow)m_invenItemList[i];
        //        break;
        //    }

        //    if (i == m_invenItemList.Count - 1)
        //        arrow = null;
        //}
        #endregion

        arrow = (Arrow)m_inventory.SetItem(eItemType.Arrow);

        if (arrow != null)
        {
            player.ChangeState(PlayerAttack.Instance);
            arrow.SetAttack(player);
        }
        else
        {
            player.ChangeState(PlayerIdle.Instance);
        }
    }
    #endregion

    public bool OnInventory()
    {
        if (!m_inventory.gameObject.activeSelf)
            return false;
        else
            return true;
    }

    #endregion

    #region Unity Method
    protected override void OnAwake()
    {
        foreach (var item in m_itemPrefab)
        {
            eItemType type = (eItemType)(int.Parse(item.name.Substring(item.name.Length - 1)));
            if(type == eItemType.Treasure_Box || type > eItemType.Bow)
            {
                eCategory category = eCategory.Consume;
                GameObjectPool<ItemController> pool = new GameObjectPool<ItemController>(10, () =>
                {
                    var obj = Instantiate(item) as GameObject;
                    obj.transform.SetParent(transform);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localRotation = Quaternion.identity;
                    obj.transform.localScale = Vector3.one;
                    var consume = obj.GetComponent<ItemController>();
                    consume.InitItem(category, type);

                    return consume;
                });
                m_consumeItemDic.Add(type, pool);
            }
            else
            {
                eCategory category = eCategory.Euipment;
                var obj = Instantiate(item) as GameObject;
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                var equip = obj.GetComponent<ItemController>();
                equip.InitItem(category, type);

                m_equipmentItemList.Add(equip); 
            }
        }
    }
    #endregion
}
