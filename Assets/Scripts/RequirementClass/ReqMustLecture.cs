using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqMustLecture : RequirementBase
// 전공필수 중 본인 학년에 해당하는 A 과목이 수강신청 되었는지 확인하는 요구사항
{
    int reqLecture;
    int currentStudentGrade;

    // Start is called before the first frame update
    public ReqMustLecture()
    {
        base.GetLogicManager();
    }
    public override bool DoesMeetRequirement()
    {
        // currentStudent의 학년 통해 들을 수 있는 전공필수 과목 랜덤 선택 

        throw new System.NotImplementedException();
    }

}
