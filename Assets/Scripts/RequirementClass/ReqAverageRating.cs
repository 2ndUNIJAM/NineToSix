using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;

public class ReqAverageRating : RequirementBase
{
    float reqAverageRating;
    void Start()
    {
        base.GetLogicManager();

        reqAverageRating = Random.Range(4, 9) / 2f;

        contentTitle = string.Format("교수 평균 평점 {0}점 이상", reqAverageRating);
    }

    public override bool DoesMeetRequirement()
    {
        // ★★Implement: GameManager 업데이트 후 currentStudent 받아오는거 변경 필수 안 그럼 에러 발생 
        Student currentStudent = new Student();
        gameLogicManager.GetCurrentStudent(currentStudent);

        List<Lecture> finalLectures = new List<Lecture>();
        currentStudent.GetCurrentLectures(finalLectures);

        int finalLecCount = 0;
        float finalAverageRating = 0;
        foreach(Lecture lecture in finalLectures)
        {
            ++finalLecCount;
            finalAverageRating += lecture.profRating;
        }

        finalAverageRating /= finalLecCount;
        if (finalAverageRating >= reqAverageRating) return true;
        else return false;

    }

}
