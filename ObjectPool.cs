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
            taretObjectQueue.Enqueue(CreateNew());
        }
    }

    // 오브젝트 생성
    private ObjectScript CreateNew()
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
            obj.GetComponent<ObjectScript>().InitBullet();
            return obj;
        }
        // 풀에 여분이 없을때 새로운 오브젝트 생성
        else
        {
            var newObj = instance.CreateNewBullet();
            newObj.gameObject.SetActive(true);
            newObj.GetComponent<ObjectScript>().InitBullet();
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
