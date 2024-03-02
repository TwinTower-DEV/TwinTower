using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 발판을 밟을 시 기믹 작동을 편하게 해주기 위한 인터페이스이다.
/// </summary>
public class ActivateObject : MonoBehaviour
{
    public virtual void Launch()
    {
    }
    
    public virtual void UnLaunch(){}
}
