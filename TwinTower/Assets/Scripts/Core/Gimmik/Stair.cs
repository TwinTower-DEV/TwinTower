using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

/// <summary>
/// 계단, 플레이어가 동시에 진입 시 다음 단계 진입.
/// OnPlayer로 플레이어가 계단 위에 있을 경우 다른 계단 확인 후 다음 단계 진입.
/// Manager 이용.
/// 보툥 계단 2개가 함께 이 스크립트를 저장함. 서로 Stair로 연결되어 있음.
/// </summary>
public class Stair : MonoBehaviour
{
    private bool OnPlayer;
    public Stair stair2;
    [SerializeField] private string stair;             // 다음 씬 저장 오브젝트
    [SerializeField] private CutSceneCheck cutSceneCheck;
    private void Awake()
    {
        OnPlayer = false;
    }
    
    // 동시에 진입했는지 알아야 하며 동시 진입시 다음 단계 진입 - NextLevelManager이용
    // 다음 단계 씬 string도 함께 전달.
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            MoveControl playerControl = other.gameObject.GetComponent<MoveControl>();
            OnPlayer = true;
            if (stair2.getOnPlayer() && OnPlayer) {       // 계단 동시 진입 - 다음 단계 진입                     // 다른 계단에 플레이어가 위치해 있는지 확인
                //NextLevelManager.Instance.NextLevel(stair.NextSceneString);
                if (cutSceneCheck == null)
                {
                    GameManager.Instance.isClearCheck = true;
                    StartCoroutine(NextStage());

                }
                else
                    cutSceneCheck.CutSceneStart();
                Debug.Log("다음 단계 진입" + gameObject.name);
            }
        }
    }

    private IEnumerator NextStage()
    {
        yield return StartCoroutine(UI_ScreenFader.FadeScenOut());
        UIManager.Instance.ShowNormalUI<UI_Clear>();
        yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) OnPlayer = false;
    }

    // 다른 계단에게 넘겨주기 위함.
    public bool getOnPlayer() {
        return OnPlayer;
    }
}