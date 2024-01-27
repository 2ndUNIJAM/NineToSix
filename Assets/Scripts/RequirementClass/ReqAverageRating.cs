using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;

public class ReqAverageRating : RequirementBase
{
    float reqAverageRating;
    public ReqAverageRating()
    {
        base.GetLogicManager();

        reqAverageRating = Random.Range(4, 9) / 2f; // 2,2.5, 3, 3.5, 4 중 하나
        contentTitle = $"교수 평균 평점 {reqAverageRating}점 이상";
    }

    public override bool DoesMeetRequirement()
    {
        int finalLecCount = 0;
        float finalAverageRating = 0;

        foreach (Lecture lecture in gLogicManager.GetSelectedLectures())
        {
            ++finalLecCount;
            finalAverageRating += lecture.Rating;
        }

        finalAverageRating /= finalLecCount;
        if (finalAverageRating >= reqAverageRating) return true;
        else return false;
    }
}
