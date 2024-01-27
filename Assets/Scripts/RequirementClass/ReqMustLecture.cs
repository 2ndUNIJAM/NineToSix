using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqMustLecture : RequirementBase
// 전공필수 중 본인 학년에 해당하는 A 과목이 수강신청 되었는지 확인하는 요구사항
{
    ELectureType reqLectureType;
    ELectureGrade currentStudentGrade;

    // Start is called before the first frame update
    public ReqMustLecture()
    {
        base.GetLogicManager();
        contentTitle = "뒹굴역학 필수 이수";
    }
    public override bool DoesMeetRequirement()
    {
        // currentStudent의 학년 통해 들을 수 있는 전공필수 과목 랜덤 선택 

        throw new System.NotImplementedException();
    }

}
