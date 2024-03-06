using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 발판을 밟을 시 기믹 작동을 편하게 해주기 위한 상위 클래스
/// 우선은 전부 빈 함수로 두었음.
/// </summary>
public class ActivateObject : MonoBehaviour
{
    public virtual void Launch()
    {
    }
    
    public virtual void UnLaunch(){}
}
