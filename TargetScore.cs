using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TargetScore : MonoBehaviour
{
    public Transform centerPos;
    public Transform edgePos;

    private BoxCollider col;
    private Vector3 inputVec, localVec;
    private Vector2 finalPos;
    private float ratio;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    private void CalculateScore()
    {
        Vector2 tempVec;
        float colSize = col.size.x;
        colSize /= 2;
        localVec = transform.InverseTransformPoint(inputVec);
        tempVec = localVec / colSize;

        float tempVecMagitude = tempVec.magnitude;
        float score = 1 - tempVecMagitude;
        score = (int)((score + 0.1f) * 10.0f);
        Debug.Log(score);
    }

    public void InputProjecetilePos(Vector3 _vec)
    {
        inputVec = _vec;
        CalculateScore();
    }

    public int ReturnScore()
    {
        int score = 0;

        return score;
    }

    public void DebugTargetScore()
    {
        CalculateScore();
    }
}
