using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : class
{
    public delegate T Func();

    #region Field
    int m_count;
    Func CreateFunc;
    Queue<T> m_objectPool; //순서대로 GameObject를 꺼냈다 넣었다 하기 위해 Queue로 관리

    public int Count { get { return m_objectPool.Count; } }

    //ObjectPool의 총갯수와 세부 정보
    public GameObjectPool(int count, Func createFunc)
    {
        m_count = count; //ObjectPool 안의 Object 갯수
        CreateFunc = createFunc; //람다식로 그때 그때 세부정보를 설정할 수 있도록 함
        m_objectPool = new Queue<T>(count);
        Allocate();
    }
    #endregion

    #region Public Method
    public void Allocate()
    {
        for (int i = 0; i < m_count; i++)
        {
            m_objectPool.Enqueue(CreateFunc()); //정해진 세부사항대로 Queue에 넣음
        }
    }

    public T Peek()
    {
        return m_objectPool.Peek();
    }

    public T Get()
    {
        if (m_objectPool.Count > 0)
            return m_objectPool.Dequeue(); //ObjectPool의 갯수가 0이 되기 전까지 빼냄
        else
        {
            m_objectPool.Enqueue(CreateFunc()); //ObjectPool의 갯수가 넘었는데 빼낼 것을 요구하면 새로 만들어서 빼냄
            return m_objectPool.Dequeue();
        }
    }

    public void Set(T item)
    {
        m_objectPool.Enqueue(item);
    }
    #endregion
}
