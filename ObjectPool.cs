/*

    targetObject -> 오브젝트풀링에 사용될 오브젝트
    ObjectScript -> targetObject에 있는 스크립트
    
    ObjectPool.GetObject()로 풀에서 꺼냄
    ObjectPool.ReturnObject(this)로 풀에 반납

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private ObjectPool() { }
    public static ObjectPool instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        // 풀에 저장해놓을 오브젝트 갯수 생성
        Initialize(20);
    }

    public GameObject targetObject;
    private Queue<ObjectScript> targetObjectQueue = new Queue<ObjectScript>();

    private void Initialize(int _initCount)
    {
        for (int i = 0; i < _initCount; i++)
        {
            targetObjectQueue.Enqueue(CreateNew());
        }
    }

    // 오브젝트 생성
    private ObjectScript CreateNewObject()
    {
        var newObj = Instantiate(targetObject).GetComponent<ObjectScript>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static ObjectScript GetObject()
    {
        // 풀에서 꺼내감
        if (instance.targetObjectQueue.Count > 0)
        {
            var obj = instance.targetObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        // 풀에 여분이 없을때 새로운 오브젝트 생성
        else
        {
            var newObj = instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    // 풀에 반납
    public static void ReturnObject(ObjectScript _obj)
    {
        _obj.gameObject.SetActive(false);
        _obj.transform.SetParent(instance.transform);
        instance.targetObjectQueue.Enqueue(_obj);
    }
}
