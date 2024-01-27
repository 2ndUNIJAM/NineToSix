using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class ReqLectureNumber : RequirementBase
// 특정 분야 n과목 이상 필수 요구사항 클래스
{
    ELectureType reqLectureType;
    [SerializeField] int reqNumLectures; // 필요 과목 수

    // Start is called before the first frame update
    public ReqLectureNumber()
    {
        base.GetLogicManager();

        reqLectureType = (ELectureType)Random.Range(0, 3); // 0 1 2 랜덤 생성
        string type = string.Empty;
        switch ((int)reqLectureType)
        {
            case 0:
                type = "교양";
                break;
            case 1:
                type = "전공기초";
                break;
            case 2:
                type = "전공필수";
                break;
            default:
                break;
        }
        reqNumLectures = Random.Range(1, 3);
        contentTitle = $"{type} {reqNumLectures} 과목 필수 신청";
    }

    override public bool DoesMeetRequirement()
    {
        int numOKLectures = 0;
        foreach (Lecture lecture in gLogicManager.GetSelectedLectures())
        {
            if (lecture.Data.Type == reqLectureType)
            {
                ++numOKLectures;
            }
        }
        if (numOKLectures >= reqNumLectures) return true;
        else return false;
    }

}
