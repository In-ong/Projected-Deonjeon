using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Delegate
public delegate void PopupButtonDelegate();
#endregion

public class PopupManager : DontDestroy<PopupManager>
{
    #region Field
    GameObject m_popupOkCancle;

    List<GameObject> m_popupList = new List<GameObject>();
    #endregion

    #region Public Method
    public void OpenPopupOkCancle(string message, PopupButtonDelegate okBtnDel, PopupButtonDelegate cancleBtnDel, string okText = "예", string cancleText = "아니오")
    {
        var obj = Instantiate<GameObject>(m_popupOkCancle);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        transform.SetAsLastSibling();
        var popup = obj.GetComponent<UI_PopupOkCancle>();
        popup.SetPopup(message, okBtnDel, cancleBtnDel, okText, cancleText);
        m_popupList.Add(obj);
    }
    public void ClosePopup()
    {
        if(m_popupList.Count > 0)
        {
            Destroy(m_popupList[m_popupList.Count - 1]);
            m_popupList.RemoveAt(m_popupList.Count - 1);
        }
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Start()
    {
        m_popupOkCancle = Resources.Load("Prefab/UI/UI_PopupOkCancle") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}
