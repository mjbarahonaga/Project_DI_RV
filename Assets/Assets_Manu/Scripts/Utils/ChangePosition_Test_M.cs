using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class ChangePosition_Test_M : MonoBehaviour
{
    public float SizeSphere = 0.1f;
    public Vector3 OriginPosition;
    public Vector3 TargetPosition = Vector3.zero;


    public bool SetOriginPosition = false;
    public bool ChangePositionPlayer = false;
    public bool ReturnToOrigintPos = false;
    public bool ReturnToTargetPos = false;
    public XROrigin _tempPlayerRef;
    
    private void OnValidate()
    {
        if (SetOriginPosition)
        {
            SetOriginPosition = false;
            _tempPlayerRef = FindObjectOfType<XROrigin>();
            OriginPosition = _tempPlayerRef.gameObject.transform.position;
        }
        if(TargetPosition == Vector3.zero)
        {
            TargetPosition = transform.position;
        }

        if (ChangePositionPlayer)
        {
            ChangePositionPlayer = false;
            if (_tempPlayerRef.gameObject.transform.position == OriginPosition)
            {
                _tempPlayerRef.gameObject.transform.position = TargetPosition;
            }
            else
            {
                _tempPlayerRef.gameObject.transform.position = OriginPosition;
            }
        }

        if (ReturnToOrigintPos)
        {
            ReturnToOrigintPos = false;
            _tempPlayerRef.gameObject.transform.position = OriginPosition;
        }
        if (ReturnToTargetPos)
        {
            ReturnToTargetPos = false;
            _tempPlayerRef.gameObject.transform.position = TargetPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(OriginPosition, SizeSphere);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(TargetPosition, SizeSphere);
    }
}
#endif