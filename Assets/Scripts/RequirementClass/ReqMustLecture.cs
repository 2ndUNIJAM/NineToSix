using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ReqMustLecture : RequirementBase
// 전공필수 중 본인 학년에 해당하는 A 과목이 수강신청 되었는지 확인하는 요구사항
{
    string requiredLectureName;

    // Start is called before the first frame update
    public ReqMustLecture(int grade)
    {
        base.GetLogicManager();
        var lecturesInGrade = from lecture in gLogicManager.GetLectureData()
                              where lecture.Type == ELectureType.MajorRequired && (int)lecture.TargetGrade == (int)Math.Pow(2, grade - 1)
                              select lecture;
        if (lecturesInGrade.Count() > 0)
            requiredLectureName = lecturesInGrade.ElementAt(UnityEngine.Random.Range(0, lecturesInGrade.Count())).Name;
        else
            requiredLectureName = "오류과목";
        contentTitle = $"{requiredLectureName} 필수 이수";
    }
    public override bool DoesMeetRequirement()
    {
        foreach (Lecture lecture in gLogicManager.GetSelectedLectures())
        {
            if (lecture.Data.Name.Equals(requiredLectureName))
                return true;
        }
        return false;
    }

}
