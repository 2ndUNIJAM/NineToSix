using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqMustLecture : RequirementBase
{
    int reqLecture;
    int currentStudentGrade;

    // Start is called before the first frame update
    void Start()
    {
        base.GetLogicManager();
        Student currentStudent = new Student();
        gLogicManager.GetCurrentStudent(currentStudent);
        currentStudentGrade = currentStudent.GetStudentGrade();

        //contentTitle = string.Format("전공필수 과목 {0} {1} 수강신청 필수");
    }
    public override bool DoesMeetRequirement()
    {
        // currentStudent의 학년 통해 들을 수 있는 전공필수 과목 랜덤 선택 

        throw new System.NotImplementedException();
    }

}
