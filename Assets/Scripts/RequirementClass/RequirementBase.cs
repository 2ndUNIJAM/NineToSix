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
    public virtual void OnEnter() { return; } // logic���� Visualization �Ҷ� ���� / ����: id ��ȯ 
    public virtual void OnExit() { return; } // logic���� Student ������ ���� cleanup > Ȯ�� �� ����

    public void GetLogicManager()
    {
        GameObject gLogicManagerObj = GameObject.Find("Game Logic Manager");
        gLogicManager = gLogicManagerObj.GetComponent<GameLogicManager>();
    }



}
