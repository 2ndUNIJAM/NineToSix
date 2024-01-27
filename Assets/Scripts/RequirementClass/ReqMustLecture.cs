using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ReqMustLecture : RequirementBase
// 전공필수 중 본인 학년에 해당하는 A 과목이 수강신청 되었는지 확인하는 요구사항
{
    string requiredLectureName;
    int requiredLectureId;

    // Start is called before the first frame update
    public ReqMustLecture(int grade)
    {
        base.GetLogicManager();
        var lecturesInGrade = from lecture in gLogicManager.GetLectureData()
                              where lecture.Type == ELectureType.MajorRequired && (int)lecture.TargetGrade == (int)Math.Pow(2, grade - 1)
                              select lecture;
        if (lecturesInGrade.Count() > 0)
        {
            var reqLectureData = lecturesInGrade.ElementAt(UnityEngine.Random.Range(0, lecturesInGrade.Count()));
            requiredLectureName = reqLectureData.Name;
            requiredLectureId = reqLectureData.ID;
        }
        else
            requiredLectureName = "오류과목";
        contentTitle = $"{requiredLectureName} 필수 이수";
    }
    public override bool DoesMeetRequirement()
    {
        foreach (Lecture lecture in gLogicManager.GetSelectedLectures())
        {
            if (lecture.Data.Name.Equals(requiredLectureName))
            {
                gLogicManager.LectureSpawner.ReturnGuaranteedLectureToPool();
                return true;
            }
        }
        return false;
    }

    public override void OnEnter()
    {
        gLogicManager.LectureSpawner.SetGuaranteedLecture(requiredLectureId);
    }

    public override void OnExit()
    {
        // 요구사항의 전필 과목을 못채운 경우로 확정할 때 Spawner에게 가서 guaranteed가 나갈 시점까지 존재하면 없앨 것
        // guaranteedLecture가 있는지 확인하는 조건은 logicManager에 생성해놓음
        if (!DoesMeetRequirement())
        {
            gLogicManager.LectureSpawner.ReturnGuaranteedLectureToPool();
        }
    }

}
