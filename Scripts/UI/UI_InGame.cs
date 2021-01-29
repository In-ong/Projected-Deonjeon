using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : SingleTonMonoBehaviour<UI_InGame>
{
    #region SerializeField
    [SerializeField] Text m_levelText;
    public Text Level { get { return m_levelText; } set { m_levelText = value; } }
    [SerializeField] Image[] m_itemImage;
    [SerializeField] Text[] m_key;
    [SerializeField] Text[] m_itemCount;
    #endregion

    #region Field
    int m_count = 1;

    Player m_player;
    GameObject m_statusPrefab;
    GameObject m_statusGrid;

    List<Image> m_statusList = new List<Image>();
    //키를 등록했을 때, 등록한 아이템 슬롯을 보관.
    Dictionary<KeyCode, UI_ItemSlot> m_keyCodeDic = new Dictionary<KeyCode, UI_ItemSlot>();
    //키마다 정해진 인게임 슬롯 이미지를 보관.
    Dictionary<KeyCode, Image> m_itemImageDic = new Dictionary<KeyCode, Image>();
    Dictionary<KeyCode, Text> m_itemCountDic = new Dictionary<KeyCode, Text>();
    #endregion

    #region Public Method
    //전체 체력 증가 시 하트를 추가하는 메소드
    public void AddStatus()
    {
        GameObject obj = null;
        Image image = null;

        //우선 시작 시인지 아닌지를 구분.
        if (m_player.Hp - m_player.CurrentHp == 0)
        {
            //체력 2당 하트 하나이기 때문에 체력에서 2를 나눠준 값까지 i를 증가시킴.
            for (int i = 0; i < m_player.Hp / 2; i++)
            {
                obj = Instantiate<GameObject>(m_statusPrefab);
                obj.transform.SetParent(m_statusGrid.transform);
                obj.transform.localScale = Vector3.one;
                image = obj.GetComponent<Image>();
                if (m_player.Hp - m_player.CurrentHp == 0)
                    image.fillAmount = m_player.Hp * 0.5f;
                else if (m_player.Hp - m_player.CurrentHp != 0)
                    image.fillAmount = (m_player.Hp - m_player.CurrentHp) * 0.5f;
                m_statusList.Add(image);
            }
        }
        else if (m_player.Hp - m_player.CurrentHp != 0)
        {
            //변화한 체력이 짝수인지 홀수인지 구분.
            if ((m_player.Hp - m_player.CurrentHp) % 2 == 0 && m_player.Hp - m_player.CurrentHp != 0)
            {
                for (int i = 0; i < (m_player.Hp - m_player.CurrentHp) / 2; i++)
                {
                    obj = Instantiate<GameObject>(m_statusPrefab);
                    obj.transform.SetParent(m_statusGrid.transform);
                    obj.transform.localScale = m_statusList[0].transform.localScale;
                    image = obj.GetComponent<Image>();
                    if (m_player.Hp - m_player.CurrentHp == 0)
                        image.fillAmount = m_player.Hp * 0.5f;
                    else if (m_player.Hp - m_player.CurrentHp != 0)
                        image.fillAmount = (m_player.Hp - m_player.CurrentHp) * 0.5f;
                    m_statusList.Add(image);
                }
            }
            else if((m_player.Hp - m_player.CurrentHp) % 2 == 1)
            {
                int num = m_player.Hp - m_player.CurrentHp;

                //반복문을 먼저 사용해야 증가가 제대로 적용됨.
                for (int i = 0; i < ((m_player.Hp - m_player.CurrentHp) / 2) + 1; i++)
                {
                    //홀수일 경우 마지막 하트가 채워져있는지부터 확인.
                    if (m_statusList[m_statusList.Count - 1].fillAmount == 1)
                    {
                        //채워져 있을 경우 새로운 하트를 생성.
                        obj = Instantiate<GameObject>(m_statusPrefab);
                        obj.transform.SetParent(m_statusGrid.transform);
                        obj.transform.localScale = m_statusList[0].transform.localScale;
                        image = obj.GetComponent<Image>();
                        if (m_player.Hp - m_player.CurrentHp == 0)
                            image.fillAmount = m_player.Hp * 0.5f;
                        else if (num != 0)
                        {
                            image.fillAmount = num * 0.5f;
                            num -= 2;
                        }
                        m_statusList.Add(image);
                    }
                    else
                    {
                        //채워져 있지 않는다면 FillAmount를 변화시킨다.
                        m_statusList[m_statusList.Count - 1].fillAmount += (m_player.Hp - m_player.CurrentHp) * 0.5f;
                    }
                }
            }
        }

        #region Do 반복문
        //이상하게도 반복문이 false임에도 계속 반복되는 오류가 발생.
        
        //int i = 0;

        //do
        //{
        //    i += 2;
        //    var obj = Instantiate<GameObject>(m_statusPrefab);
        //    obj.transform.SetParent(m_statusGrid.transform);
        //    var image = obj.GetComponent<Image>();
        //    if (m_player.Hp - m_player.CurrentHp == 0)
        //        image.fillAmount = m_player.Hp * 0.5f;
        //    else if (m_player.Hp - m_player.CurrentHp != 0)
        //        image.fillAmount = (m_player.Hp - m_player.CurrentHp) * 0.5f;
        //    m_statusList.Add(image);
        //} while ((m_player.Hp - m_player.CurrentHp) / i != 1 && m_player.Hp - m_player.CurrentHp != 0);
        #endregion
    }

    //데미지를 받을 시 하트의 FillAmount를 조절하는 메소드.
    public void Damage(int atk)
    {
        //반복문을 컨트롤하는 변수.
        bool notFinish = true;
        //Image의 fillAmount를 감소시키는 변수
        float amount = atk * 0.5f;
        //이전의 fillAmount를 담아놓는 변수
        float prefillAmount = 0f;

        //불필요한 코드를 진행하지 않도록 일단 데미지가 들어오는지 확인
        if (amount > 0)
        {
            do
            {
                //리스트의 마지막에 있는 이미지의 fillAmount가 0이 아닐 경우 하트를 조절하고 아니면 그 이전에 있는 이미지로 넘어감.
                if (m_statusList[m_statusList.Count - m_count].fillAmount != 0)
                {
                    //fillAmount가 1이 아닌지 확인
                    if (m_statusList[m_statusList.Count - m_count].fillAmount != 1)
                    {
                        //빼는 변수가 1 이상일 경우와 그 미만일 경우로 구분
                        if (amount >= 1)
                        {
                            prefillAmount = m_statusList[m_statusList.Count - m_count].fillAmount;

                            m_statusList[m_statusList.Count - m_count].fillAmount -= amount;

                            //빼는 변수와 깎이기 전의 fillAmount를 빼고서 남은 수가 0 초과일 경우 다음 하트로 진행.
                            //0일 경우 반복을 끝낸다.
                            if (amount - prefillAmount > 0)
                            {
                                amount = amount - prefillAmount;

                                m_count++;

                                notFinish = true;
                            }
                            else
                            {
                                if (m_count != 1)
                                    m_count = 1;

                                notFinish = false;
                            }
                        }
                        else
                        {
                            //빼는 변수가 1 미만인 경우는 한 번만 빼주고 반복을 끝냄.
                            m_statusList[m_statusList.Count - m_count].fillAmount -= amount;

                            if (m_count != 1)
                                m_count = 1;

                            notFinish = false;
                        }
                    }
                    else
                    {
                        //fillAmount가 1인 경우는 빼는 변수가 1 초과인지로 확인하고 그 외에는 위와 똑같이 진행.
                        if (amount > 1)
                        {
                            prefillAmount = m_statusList[m_statusList.Count - m_count].fillAmount;
                            m_statusList[m_statusList.Count - m_count].fillAmount -= amount;
                            if (amount - prefillAmount > 0)
                            {
                                amount = amount - prefillAmount;

                                m_count++;

                                notFinish = true;
                            }
                            else
                            {
                                if (m_count != 1)
                                    m_count = 1;

                                notFinish = false;
                            }
                        }
                        else
                        {
                            //변화한 체력에 0.5를 곱해주어 fillAmount를 조절. fillAmount는 1이기 때문에 이렇게 적용하는 것.
                            m_statusList[m_statusList.Count - m_count].fillAmount -= amount;
                            //m_count는 리스트 키를 조절하기 위한 변수로 하트 조절이 끝나면 1로 초기화.
                            if (m_count != 1)
                                m_count = 1;
                            notFinish = false;
                        }
                    }
                }
                else
                {
                    m_count++;
                    notFinish = true;
                }
            } while (notFinish);
        }
    }

    public void Hilling(int hillPoint)
    {
        //반복문을 컨트롤하는 변수.
        bool notFinish = true;
        //Image의 fillAmount를 증가시키는 변수
        float amount = hillPoint * 0.5f;
        //이전의 fillAmount를 담아놓는 변수
        float prefillAmount = 0f;

        do
        {
            //현재 하트의 fillAmount가 0이고 그 전의 하트가 1이 아닐 경우 이전 하트로 넘어감.
            if (m_statusList[m_statusList.Count - m_count].fillAmount == 0 && m_statusList[m_statusList.Count - (m_count + 1)].fillAmount != 1)
            {
                m_count++;
                notFinish = true;
            }
            else if(m_statusList[m_statusList.Count - m_count].fillAmount != 1)
            {
                prefillAmount = m_statusList[m_statusList.Count - m_count].fillAmount;
                m_statusList[m_statusList.Count - m_count].fillAmount += amount;
                //현재 fillAmount와 이전의 fillAmount를 빼서 증가된 값을 구한 후, 증가시키는 변수에서 빼준 것을 비교
                if (amount - (m_statusList[m_statusList.Count - m_count].fillAmount - prefillAmount) > 0)
                {
                    //0 초과이면 증가시킬 변수를 빼준 값으로 변경.
                    amount = amount - (m_statusList[m_statusList.Count - m_count].fillAmount - prefillAmount);

                    //리스트의 이전이 아닌 다음에 있는 하트를 꺼내야 함으로 count를 감소시켜준다.
                    m_count--;
                    notFinish = true;
                }
                else
                {
                    if (m_count != 1)
                        m_count = 1;
                    notFinish = false;
                }
            }
        } while (notFinish);
    }

    //슬롯을 선택하고 키에 장비했을 때, 진행되는 메소드
    public void SelectSlotItem(UI_ItemSlot slot, KeyCode key, bool on, KeyCode key2 = KeyCode.None)
    {
        //사용 중인 키가 있는지 확인
        if (m_keyCodeDic.Count > 0)
        {
            //사용 중인 키가 있다면 선택된 슬롯이 이미 등록되어 있는지 확인
            if (!on)
            {
                //키의 슬롯이 켜져 있지 않다면 켜주고서 키를 Dictionary에 넣어준다
                if (!m_itemImageDic[key].gameObject.activeSelf)
                {
                    m_itemImageDic[key].gameObject.SetActive(true);
                    m_itemCountDic[key].transform.parent.gameObject.SetActive(true);

                    slot.InGameSlotOn = true;

                    m_itemImageDic[key].sprite = slot.ItemImage.sprite;
                    m_itemCountDic[key].text = slot.ItemCount.text;

                    m_keyCodeDic.Add(key, slot);
                }
                //키의 슬롯이 켜져 있면 그냥 이미지만 바꿔줌
                else
                {
                    slot.InGameSlotOn = true;

                    m_itemImageDic[key].sprite = slot.ItemImage.sprite;
                    m_itemCountDic[key].text = slot.ItemCount.text;

                    m_keyCodeDic[key].InGameSlotOn = false;
                    m_keyCodeDic[key].KeySelect.transform.parent.gameObject.SetActive(false);
                    m_keyCodeDic.Remove(key);

                    m_keyCodeDic.Add(key, slot);
                }
            }
            else
            {
                if (!m_itemImageDic[key].gameObject.activeSelf)
                {
                    //이전 키의 슬롯을 끄고 그 키를 Dictionary에서 제외시킨다.
                    m_itemImageDic[key].gameObject.SetActive(true);
                    m_itemCountDic[key].transform.parent.gameObject.SetActive(true);

                    m_itemImageDic[key2].gameObject.SetActive(false);
                    m_itemCountDic[key2].transform.parent.gameObject.SetActive(false);

                    m_itemImageDic[key].sprite = slot.ItemImage.sprite;
                    m_itemCountDic[key].text = slot.ItemCount.text;

                    m_keyCodeDic.Add(key, slot);
                    m_keyCodeDic.Remove(key2);
                }
                else
                {
                    //켜져 있는 슬롯인데 거기에 등록된 아이템 슬롯과 등록하려는 슬롯이 같은지 아닌지 판단
                    if (m_keyCodeDic[key] != slot)
                    {
                        //등록된 슬롯인데 리스트의 슬롯과 같지 않다는 건 다른 슬롯에 등록되어 있다는 의미이므로 이전 슬롯을 꺼준다.
                        m_itemImageDic[key2].gameObject.SetActive(false);
                        m_itemCountDic[key2].transform.parent.gameObject.SetActive(false);

                        m_itemImageDic[key].sprite = slot.ItemImage.sprite;
                        m_itemCountDic[key].text = slot.ItemCount.text;

                        //켜져 있는 슬롯이라는 의미는 이미 등록된 아이템 슬롯이 있다는 의미이므로 등록되어 있는 슬롯을 제거해준다.
                        m_keyCodeDic[key].InGameSlotOn = false;
                        m_keyCodeDic[key].KeySelect.transform.parent.gameObject.SetActive(false);
                        m_keyCodeDic.Remove(key);

                        m_keyCodeDic.Add(key, slot);
                        m_keyCodeDic.Remove(key2);
                    }
                    else
                    {
                        m_itemImageDic[key].gameObject.SetActive(false);
                        m_itemCountDic[key].transform.parent.gameObject.SetActive(false);

                        m_keyCodeDic[key].InGameSlotOn = false;
                        m_keyCodeDic[key].KeySelect.transform.parent.gameObject.SetActive(false);
                        m_keyCodeDic.Remove(key);
                    }
                }
            }
        }
        else
        {
            m_itemImageDic[key].gameObject.SetActive(true);
            m_itemCountDic[key].transform.parent.gameObject.SetActive(true);


            m_itemImageDic[key].sprite = slot.ItemImage.sprite;
            m_itemCountDic[key].text = slot.ItemCount.text;

            slot.InGameSlotOn = true;

            m_keyCodeDic.Add(key, slot);
        }
    }

    public void UsingItem(KeyCode key, Player player)
    {
        if (m_keyCodeDic.ContainsKey(key))
        {
            if (m_keyCodeDic[key].ItemList[m_keyCodeDic[key].ItemList.Count - 1].Type != ItemManager.eItemType.Arrow)
            {
                ItemManager.Instance.RemoveConsumeItem(m_keyCodeDic[key].ItemList[m_keyCodeDic[key].ItemList.Count - 1], player);

                if (m_keyCodeDic[key].ItemList.Count != 0)
                {
                    m_itemCountDic[key].text = m_keyCodeDic[key].ItemCount.text;
                }
                else
                {
                    m_itemImageDic[key].gameObject.SetActive(false);
                    m_itemCountDic[key].transform.parent.gameObject.SetActive(false);

                    m_keyCodeDic[key].InGameSlotOn = false;
                    m_keyCodeDic[key].KeySelect.transform.parent.gameObject.SetActive(false);
                    m_keyCodeDic.Remove(key);
                }
            }
        }
    }

    public void PlusItem(UI_ItemSlot slot)
    {
        if (m_keyCodeDic.Count != 0)
        {
            if (slot == m_keyCodeDic[KeyCode.A])
            {
                m_itemCountDic[KeyCode.A].text = slot.ItemCount.text;
            }
            else if (slot == m_keyCodeDic[KeyCode.S])
            {
                m_itemCountDic[KeyCode.S].text = slot.ItemCount.text;
            }
        }
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_statusPrefab = Resources.Load("Prefab/UI/Image_Status") as GameObject;
        m_statusGrid = GameObject.Find("Grid_Status");
        m_player = GameObject.Find("Player").GetComponent<Player>();

        m_levelText.text = m_player.Level.ToString();
        AddStatus();

        for (int i = 0; i < m_itemImage.Length; i++)
        {
            if(i == 0)
            {
                m_itemImageDic.Add(KeyCode.S, m_itemImage[i]);
                m_itemCountDic.Add(KeyCode.S, m_itemCount[i]);
                m_key[i].text = KeyCode.S.ToString();
            }
            else
            {
                m_itemImageDic.Add(KeyCode.A, m_itemImage[i]);
                m_itemCountDic.Add(KeyCode.A, m_itemCount[i]);
                m_key[i].text = KeyCode.A.ToString();
            }

            m_itemImage[i].gameObject.SetActive(false);
            m_itemCount[i].transform.parent.gameObject.SetActive(false);
        }
    }
    #endregion
}
