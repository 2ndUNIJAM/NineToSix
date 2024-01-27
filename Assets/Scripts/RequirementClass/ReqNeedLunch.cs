using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqNeedLunch : RequirementBase
// 월화수목금 점심시간 4교시가 모두 비어있어야 하는 요청사항 
{

    public ReqNeedLunch()
    {
        base.GetLogicManager();

        contentTitle = string.Empty;

    }

    public override bool DoesMeetRequirement()
    {
        throw new System.NotImplementedException();
    }
}
