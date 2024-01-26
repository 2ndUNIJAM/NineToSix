using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class RequirementBase : MonoBehaviour
{
    [SerializeField] protected string contentTitle;
    [SerializeField] protected GameLogicManager gameLogicManager;
    public abstract bool DoesMeetRequirement();
    public void GetLogicManager()
    {
        GameObject gLogicManagerObj = GameObject.Find("GameLogicManager");
        gameLogicManager = gLogicManagerObj.GetComponent<GameLogicManager>();
    }

}
