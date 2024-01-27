using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqBlankDay : RequirementBase
// 공강날이 있는지 확인하는 요청사항
// i) 특정 요일 지정 후 공강 사수
// ii) 일주일 중 최소 1일 공강 사수 
{
    UISchedule finalSchedule;
    bool needSpecificDay; // true인 경우 i) 실행
    int needDay;

    public ReqBlankDay()
    {
        base.GetLogicManager();

        needSpecificDay = Random.Range(0, 2) == 1 ? true : false;
        finalSchedule = gLogicManager.Schedule;

        if (needSpecificDay)
        {
            needDay = Random.Range(0, 5); // 월화수목금 01234 중 랜덤
            string day = string.Empty;
            switch (needDay)
            {
                case 0:
                    day = "월요일";
                    break;
                case 1:
                    day = "화요일";
                    break;
                case 2:
                    day = "수요일";
                    break;
                case 3:
                    day = "목요일";
                    break;
                case 4:
                    day = "금요일";
                    break;
                default:
                    break;
            }
            contentTitle = $"{day} 공강 사수!";
        }
        else
        {
            contentTitle = "최소 1일 공강 사수!";
        }
    }

    public override bool DoesMeetRequirement()
    {
        if(!needSpecificDay) // 아무요일 1일 공강 사수
        { 
            for(int i=0; i<5; ++i)
            {
                if (finalSchedule.IsColumnClear(i)) 
                    return true;
            }
            return false;
        }
        else // 특정요일 공강 사수
        {
            if (finalSchedule.IsColumnClear(needDay))
                return true;
            else 
                return false;
        }
    }

}
