using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class RequirementBase
{
    [SerializeField] protected GameLogicManager gLogicManager;
    public string contentTitle;
    public bool essential;
    public abstract bool DoesMeetRequirement();
    public virtual void OnEnter() { return; } // logic에서 Visualization 할때 실행 / 역할: id 반환 
    public virtual void OnExit() { return; } // logic에서 Student 나가기 직전 cleanup > 확정 시 실행

    public void GetLogicManager()
    {
        GameObject gLogicManagerObj = GameObject.Find("Game Logic Manager");
        gLogicManager = gLogicManagerObj.GetComponent<GameLogicManager>();
    }



}
