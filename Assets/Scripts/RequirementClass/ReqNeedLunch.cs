using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqNeedLunch : RequirementBase
// 월화수목금 점심시간 4교시가 모두 비어있어야 하는 요청사항 
{
    int lunchTime;

    public ReqNeedLunch()
    {
        base.GetLogicManager();

        contentTitle = "매일 4교시 점심시간 사수!";
        lunchTime = 3; // 4교시
    }

    public override bool DoesMeetRequirement()
    {
        return gLogicManager.Schedule.IsRowClear(lunchTime);
    }
}
