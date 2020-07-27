using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public abstract class PoolMasterAbstract : MonoBehaviour
{
    public GameObject poolingObject;
    public float objectLifetime;
    private List<GameObject> poolingList = new List<GameObject>();

    #region abstract methods

    public abstract GameObject CreateObjectAbstract(GameObject _obj);

    #endregion

    #region abstract class use methods

    private void Start()
    {
        InitObject(3, poolingObject, poolingList);
    }

    public void InitObject(int _i, GameObject _obj, List<GameObject> _list)
    {
        for (int i = 0; i < _i; i++)
        {
            _list.Add(CreateObjectAbstract(_obj));
        }
    }

    public GameObject GetObject(GameObject _obj, List<GameObject> _list)
    {
        // pool 안에 비활성화중(사용가능한) 오브젝트가 있을 때
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].activeSelf == false)
            {
                _list[i].gameObject.SetActive(true);
                return _list[i];
            }
        }

        // pool 안에 사용가능한 오브젝트가 더이상 없을때
        var newObj = CreateObjectAbstract(_obj);
        newObj.SetActive(true);
        _list.Add(newObj);

        return newObj;
    }

    // event가 발생해서 Return하는 Method
    public void ReturnObject(GameObject _obj)
    {
        _obj.SetActive(false);
    }

    // 시간이 지나서 Return하는 Method
    public IEnumerator ReturnObjectLifeTime(GameObject _obj)
    {
        if (_obj.activeSelf == false)
            yield return null;
        
        yield return new WaitForSeconds(objectLifetime);

        _obj.SetActive(false);
    }

    #endregion

    #region virtual methods

    public virtual GameObject GetObjectVirtual()
    {
        return GetObject(poolingObject, poolingList);
    }

    public virtual void ReturnObjectVirtual(GameObject _obj)
    {
        ReturnObject(_obj);
    }

    #endregion

}
