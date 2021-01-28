//==========================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//==========================================================
[AddComponentMenu("Custom/PoolingSystem")]
//  에디터 > 메뉴 > Component > 해당 컴포넌트 추가 기능을 적용.
//==========================================================
public sealed class PoolingSystem : MonoBehaviour   //  sealed : 해당 클래스 상속 방지.
{
    //-----------------------------
    [System.Serializable]           //  인스펙터 창에서 클래스의 속성을 볼수 있게 함.
    public class PoolingUnit
    {
        public string name;         //  오브젝트 이름.
        public GameObject _prefObj; //  프리팹 이름
        public int _amount;         //  풀에 미리 만들어놓을 갯수.
        int _curAmount;             //  오브젝트 풀에 적용된 풀 갯수.
        public int CurAmount
        {
            get { return _curAmount;}
            set { _curAmount = value;}
        }

    }//	public class _poolingUnits
    public static PoolingSystem _instance;              //  심플한 싱글턴.
    //-----------------------------
    public PoolingUnit[]        _poolingUnits;          //	오브젝트 풀에 적용할 프리팹.
    public List<GameObject>[]   _pooledUnitsList;       //	실제 관리될 오브젝트 풀.
    //-----------------------------
    public int  _defPoolAmount = 10;                    //	디폴트 갯수.
    //-----------------------------
    public bool _canPoolExpand = true;                  //	오브젝트 풀 확장 플래그.
    //-----------------------------
    void Awake()
    {
        _instance = this;

        //  실제 사용할 오브젝트 풀의 메모리공간을 등록한 프리팹 갯수만큼 할당함..
        //  2차원 배열인 셈..
        _pooledUnitsList = new List<GameObject>[_poolingUnits.Length];

        for (int i = 0; i < _poolingUnits.Length; i++)
        {
            _pooledUnitsList[i] = new List<GameObject>();

            //	개별적으로 갯수를 설정해주지 않으면 디폴트 갯수를 적용.
            if (_poolingUnits[i]._amount > 0)
                _poolingUnits[i].CurAmount = _poolingUnits[i]._amount;
            else
                _poolingUnits[i].CurAmount = _defPoolAmount;

            int idx = 0;
            for (int j = 0; j < _poolingUnits[i].CurAmount; j++)
            {
                //	오브젝트를 생성.
                GameObject newItem = (GameObject)Instantiate(_poolingUnits[i]._prefObj);

                newItem.name += "_" + idx.ToString();

                //	비활성화.
                newItem.SetActive(false);

                //	리스트에 추가.
                _pooledUnitsList[i].Add(newItem);

                //	오브젝트 풀 매니져 오브젝트에 차일드 화.
                newItem.transform.parent = transform;

                ++idx;

            }//	for(int j=0; j<poolingAmount; j++)

        }//	for (int j = 0; j < _poolingUnits[i].CurAmount; j++)

    }//	void Start ()
    //-----------------------------    
    //  오브젝트 풀에 관리 중인 오브젝트를 가져오기..
    GameObject GetPooledItem(string pooledObjName)
    {
        for (int unitIdx = 0; unitIdx < _poolingUnits.Length; ++unitIdx)
        {
            //	오브젝트 풀에 등록된 오브젝트들중 해당 이름이 있는지 탐색.
            if (_poolingUnits[unitIdx]._prefObj.name == pooledObjName)
            {
                int listIdx = 0;
                for (listIdx = 0; listIdx < _pooledUnitsList[unitIdx].Count; ++listIdx)
                {
                    //	오브젝트 풀에 사용대기 중인 오브젝트가 있는지 체크.
                    if (_pooledUnitsList[unitIdx][listIdx] == null)
                        return null;

                    if (_pooledUnitsList[unitIdx][listIdx].activeInHierarchy == false)
                        return _pooledUnitsList[unitIdx][listIdx];

                }//	for( int listIdx = 0; listIdx < _pooledUnitsList[unitIdx].Count; ++listIdx )

                //	확장 가능한지 체크.
                if (_canPoolExpand)
                {
                    //	가능하면 오브젝트 풀에 추가하고 반환.
                    GameObject tmpObj = (GameObject)Instantiate(_poolingUnits[unitIdx]._prefObj);

                    //  확장된 오브젝트임을 명시하기 위해 이름을 구분했음..
                    tmpObj.name += "_" + listIdx.ToString() + "( " + (listIdx - _poolingUnits[unitIdx].CurAmount + 1).ToString() + " )";

                    //	비활성화.
                    tmpObj.SetActive(false);

                    //	리스트에 추가.
                    _pooledUnitsList[unitIdx].Add(tmpObj);

                    //	오브젝트 풀 매니져 오브젝트에 차일드 화.
                    tmpObj.transform.parent = transform;


                    return tmpObj;

                }//	if(_canPoolExpand)

                break;

            }//	if(_poolingUnits[unitIdx]._prefObj.name == pooledObjName)

        }//	for(int i=0; i<_poolingUnits.Length; i++)

        return null;

    }//	public GameObject GetPooledItem (string pooledObjName)
    //-----------------------------
    public GameObject InstantiateAPS(int idx, GameObject parent = null)
    {
        string pooledObjName = _poolingUnits[idx].name;

        GameObject tmp = InstantiateAPS(pooledObjName, Vector3.zero,
                                        _poolingUnits[idx]._prefObj.transform.rotation,
                                        _poolingUnits[idx]._prefObj.transform.localScale,
                                        parent);

        return tmp;

    }// public GameObject InstantiateAPS(int idx, GameObject parent = null)
    //-----------------------------
    public GameObject InstantiateAPS(int idx, Vector3 pos, Quaternion rot, Vector3 scale, GameObject parent = null)
    {
        string pooledObjName = _poolingUnits[idx].name;

        GameObject tmp = InstantiateAPS(pooledObjName, pos, rot, scale, parent);

        return tmp;

    }// public GameObject InstantiateAPS(int idx, Vector3 pos, Quaternion rot, Vector3 scale, GameObject parent = null)
    //-----------------------------
    public GameObject InstantiateAPS(string pooledObjName, GameObject parent = null)
    {
        GameObject tmpObj = GetPooledItem(pooledObjName);

        tmpObj.SetActive(true);

        return tmpObj;

    }// public GameObject InstantiateAPS(string pooledObjName, GameObject parent = null)
    //-----------------------------
    public GameObject InstantiateAPS(string pooledObjName, Vector3 pos, Quaternion rot, Vector3 scale, GameObject parent = null)
    {
        GameObject tmpObj = GetPooledItem(pooledObjName);

        if (tmpObj != null)
        {
            if (parent != null)
                tmpObj.transform.parent = parent.transform;

            tmpObj.transform.position = pos;
            tmpObj.transform.rotation = rot;
            tmpObj.transform.localScale = scale;
            tmpObj.SetActive(true);

        }//	if(newObject != null)

        return tmpObj;

    }//	public GameObject InstantiateAPS (string itemType, Vector3 itemPosition, Quaternion itemRotation, GameObject myParent = null )
    //-----------------------------
    //  현재 활성화된 오브젝트들..
    public List<GameObject> GetActivatePooledItem()
    {
        List<GameObject> tmps = new List<GameObject>();

        for (int unitIdx = 0; unitIdx < _poolingUnits.Length; ++unitIdx)
        {
            for (int listIdx = 0; listIdx < _pooledUnitsList[unitIdx].Count; ++listIdx)
            {
                if (_pooledUnitsList[unitIdx][listIdx].activeInHierarchy)
                    tmps.Add(_pooledUnitsList[unitIdx][listIdx]);

            }//	if(_poolingUnits[unitIdx]._prefObj.name == pooledObjName)

        }//	for(int i=0; i<_poolingUnits.Length; i++)

        return tmps;
    }
    //-----------------------------
    public static void DestroyAPS(GameObject obj) { obj.SetActive(false); }
    //-----------------------------



    //==========================================================
    //  이펙트, 사운드 연출 시작.
    //==========================================================
    public static void PlayEffect(ParticleSystem particleSystem)
    {
        if (particleSystem == null)
            return;

        particleSystem.gameObject.SetActive(true);

        particleSystem.Play();

    }// public static void PlayEffect(ParticleSystem particleSystem)
    //-----------------------------
    public static void PlayEffect(GameObject effObj)
    {
        effObj.SetActive(true);

        ParticleSystem tmp = effObj.GetComponent<ParticleSystem>();

        PlayEffect(tmp);

    }// public static void PlayEffect(GameObject effObj)
    //-----------------------------
    public static void PlayEffect(ParticleSystem particleSystem, int emitCount)
    {
        if (particleSystem == null)
            return;
        particleSystem.gameObject.SetActive(true);

        particleSystem.Emit(emitCount);

    }// public static void PlayEffect(ParticleSystem particleSystem, int emitCount)
    //-----------------------------
    public static void PlayEffect(GameObject effObj, int emitCount)
    {
        effObj.SetActive(true);

        ParticleSystem tmp = effObj.GetComponent<ParticleSystem>();

        PlayEffect(tmp, emitCount);

    }//	public static void PlayEffect(GameObject effObj, int emitCount )    
    //-----------------------------    
    public static void PlaySoundRepeatedly(GameObject soundObj, float volume = 1.0f)
    {
        AudioSource tmp = soundObj.GetComponent<AudioSource>();

        if (tmp.isPlaying)
            return;

        if (tmp)
        {
            tmp.Play();
            tmp.loop = true;
            tmp.volume = volume;

        }//	if(tmp)

    }// public static void PlaySoundRepeatedly(GameObject soundObj, float volume = 1.0f)
    //-----------------------------
    public static void PlaySound(GameObject soundObj, float volume = 1.0f)
    {
        AudioSource tmp = soundObj.GetComponent<AudioSource>();

        if (tmp)
        {
            tmp.PlayOneShot(tmp.clip);
            tmp.volume = volume;

        }//	if(tmp)

    }//	public static void PlaySound(GameObject soundObj)
     //-----------------------------
    public static void StopSound(GameObject soundObj)
    {
        AudioSource tmp = soundObj.GetComponent<AudioSource>();

        if (tmp)
            tmp.Stop();

    }//	public static void StopSound(GameObject soundObj )
     //-----------------------------

}//	public sealed class PoolingSystem : MonoBehaviour
//==========================================================
public static class PoolingSystemExtensions
{
    //-----------------------------	    
    public static void DestroyAPS(this GameObject obj)
    { PoolingSystem.DestroyAPS(obj); }
    //-----------------------------
    public static void PlaySoundRepeatedly(this GameObject soundObj, float volume = 1.0f)
    { PoolingSystem.PlaySoundRepeatedly(soundObj, volume); }
    //-----------------------------
    public static void PlaySound(this GameObject soundObj, float volume = 1.0f)
    { PoolingSystem.PlaySound(soundObj, volume); }
    //-----------------------------
    public static void StopSound(this GameObject soundObj)
    { PoolingSystem.StopSound(soundObj); }
    //-----------------------------
    public static void PlayEffect(this GameObject effObj, int emitCount) { PoolingSystem.PlayEffect(effObj, emitCount); }
    //-----------------------------
    public static void PlayEffect(this GameObject effObj) { PoolingSystem.PlayEffect(effObj); }
    //-----------------------------

}//	public static class PoolingSystemExtensions
 //==========================================================