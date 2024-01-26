using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqMustLecture : RequirementBase
{
    string[] reqLecture;

    // Start is called before the first frame update
    void Start()
    {
        base.GetLogicManager();

        reqLecture = Random.Range(0, 2) == 1 ? new string[2] : new string[1];
        //contentTitle = string.Format("전공필수 과목 {0} {1} 수강신청 필수");
    }
    public override bool DoesMeetRequirement()
    {
        throw new System.NotImplementedException();
    }

}
