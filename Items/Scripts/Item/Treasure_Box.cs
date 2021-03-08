using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure_Box : ItemController
{
    #region Field
    int m_num;
    public int Number { get { return m_num; } set { m_num = value; } }

    [SerializeField] Animator m_animator;
    [SerializeField] GameObject m_head;

    public Animator BoxAnimator { get { return m_animator; } }
    #endregion

    #region Public Method
    public override void SetItem(Vector3 pos)
    {
        gameObject.SetActive(true);

        m_head.transform.rotation = Quaternion.identity;

        //보물상자가 여러 개 있을 경우 각각 들어 있는 보물을 관리하기 위해 번호를 매겨준다
        //번호는 보물 리스트의 총 갯수의 -1을 한 값
        if (ItemManager.Instance.TreasureList.Count > 0)
            m_num = ItemManager.Instance.TreasureList.Count - 1;

        transform.position = pos;
        transform.rotation = Quaternion.identity;
    }

    public void GetTreasure(Player player)
    {
        //m_animator.SetTrigger("Open");

        var treasure = ItemManager.Instance.TreasureList[m_num];
        if (ItemManager.Instance.TreasureList.Remove(treasure))    
        {
            //카테고리에 따라 보물을 얻었을 때의 효과가 달라진다
            //장비 카테고리는 player의 장착 dummy에 붙여주고 소비 카테고리는 각각의 효과가 발동된다.
            if (treasure.Category == ItemManager.eCategory.Euipment)
            {
                if (!treasure.OnInvenItem)
                    treasure.OnInvenItem = true;

                treasure.gameObject.SetActive(true);

                if(treasure.Type == ItemManager.eItemType.Sword)
                {
                    if(!player.Bow.activeSelf)
                    {
                        player.Sword.SetActive(true);
                        treasure.transform.SetParent(player.Sword.transform);
                        treasure.transform.localPosition = Vector3.zero;
                        treasure.transform.localRotation = Quaternion.identity;
                        treasure.transform.localScale = Vector3.one;
                    }
                    else
                    {
                        player.Acceptant_Sword.SetActive(true);
                        treasure.transform.SetParent(player.Acceptant_Sword.transform);
                        treasure.transform.localPosition = Vector3.zero;
                        treasure.transform.localRotation = Quaternion.identity;
                        treasure.transform.localScale = Vector3.one;
                    }

                    ItemManager.Instance.InvenItemList.Add(treasure);
                    ItemManager.Instance.Inventory.GetItem(treasure);
                }
                else if(treasure.Type == ItemManager.eItemType.Bow)
                {
                    if (!player.Sword.activeSelf)
                    {
                        player.Bow.SetActive(true);
                        treasure.transform.SetParent(player.Bow.transform);
                        treasure.transform.localPosition = Vector3.zero;
                        treasure.transform.localRotation = Quaternion.identity;
                        treasure.transform.localScale = Vector3.one;
                    }
                    else
                    {
                        player.Acceptant_Bow.SetActive(true);
                        treasure.transform.SetParent(player.Acceptant_Bow.transform);
                        treasure.transform.localPosition = Vector3.zero;
                        treasure.transform.localRotation = Quaternion.identity;
                        treasure.transform.localScale = Vector3.one;
                    }

                    ItemManager.Instance.InvenItemList.Add(treasure);
                    ItemManager.Instance.Inventory.GetItem(treasure);
                }
                else if(treasure.Type == ItemManager.eItemType.Shield)
                {
                    player.Shield.SetActive(true);
                    treasure.transform.SetParent(player.Shield.transform);
                    treasure.transform.localPosition = Vector3.zero;
                    treasure.transform.localRotation = Quaternion.identity;
                    treasure.transform.localScale = Vector3.one;

                    ItemManager.Instance.InvenItemList.Add(treasure);
                    ItemManager.Instance.Inventory.GetItem(treasure);
                }
            }
            else if (treasure.Category == ItemManager.eCategory.Consume)
            {
                if (treasure.Type == ItemManager.eItemType.Heart_UP)
                {
                    var consume = (Heart_Up)treasure;
                    consume.Effect(player);
                }
            }
        }
    }
    #endregion

    #region Unity Method
    protected override void OnAwake()
    {
        m_animator = GetComponent<Animator>();

        m_head.transform.rotation = Quaternion.identity;

        base.OnAwake();
    }

    private void Update()
    {
        if(m_animator.GetCurrentAnimatorStateInfo(0).IsName("Treasure Box Open"))
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= m_animator.GetCurrentAnimatorStateInfo(0).length)
            {
                gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
