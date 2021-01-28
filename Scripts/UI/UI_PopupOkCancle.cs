using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopupOkCancle : MonoBehaviour
{
    #region Field
    [SerializeField] Text m_mainMessage;
    [SerializeField] Text m_okText;
    [SerializeField] Text m_cancleText;

    PopupButtonDelegate m_okBtnDel;
    PopupButtonDelegate m_cancleBtnDel;
    #endregion

    #region Public Method
    public void SetPopup(string message, PopupButtonDelegate okBtnDel, PopupButtonDelegate cancleBtnDel, string okText = "예", string cancleText = "아니오")
    {
        m_mainMessage.text = message;
        m_okText.text = okText;
        m_cancleText.text = cancleText;
        m_okBtnDel = okBtnDel;
        m_cancleBtnDel = cancleBtnDel;
    }
    public void OnPressOk()
    {
        if (m_okBtnDel != null)
        {
            m_okBtnDel();
        }
        else
            PopupManager.Instance.ClosePopup();
    }
    public void OnPressCancle()
    {
        if (m_cancleBtnDel != null)
            m_cancleBtnDel();
        PopupManager.Instance.ClosePopup();
    }
    #endregion
}
