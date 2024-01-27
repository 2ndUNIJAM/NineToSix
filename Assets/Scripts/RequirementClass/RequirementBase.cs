using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class RequirementBase
{
    [SerializeField] protected GameLogicManager gLogicManager;
    public string contentTitle;
    public bool essential;
    public abstract bool DoesMeetRequirement();
    public void GetLogicManager()
    {
        GameObject gLogicManagerObj = GameObject.Find("Game Logic Manager");
        gLogicManager = gLogicManagerObj.GetComponent<GameLogicManager>();
    }
}
