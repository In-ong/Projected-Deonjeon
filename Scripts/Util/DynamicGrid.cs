using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour
{
    #region Field
    float m_originWidth, m_originHeight;
    RectTransform m_parent;
    GridLayoutGroup m_grid;
    #endregion

    #region Public Method
    public void SetDynamicGrid(int cnt, int minColsInARow, int maxRow)
    {
        int rows = Mathf.Clamp(Mathf.CeilToInt((float)cnt / minColsInARow), 1, maxRow + 1);
        int cols = Mathf.CeilToInt((float)cnt / rows);

        float spaceW = (m_grid.padding.left + m_grid.padding.right) + (m_grid.spacing.x * (cols - 1));
        float spaceH = (m_grid.padding.top + m_grid.padding.bottom) + (m_grid.spacing.y * (rows - 1));

        float maxWidth = m_originWidth - spaceW;
        float maxHeight = m_originHeight - spaceH;

        float width = Mathf.Min(m_parent.rect.width - spaceW, maxWidth);
        float height = Mathf.Min(m_parent.rect.height - spaceH, maxHeight);

        m_grid.cellSize = new Vector2(width / cols, height / rows);
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Awake()
    {
        m_parent = GetComponent<RectTransform>();
        m_grid = GetComponent<GridLayoutGroup>();

        m_originWidth = m_parent.rect.width;
        m_originHeight = m_parent.rect.height;
    }
    #endregion
}
