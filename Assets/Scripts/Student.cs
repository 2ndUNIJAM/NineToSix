using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour
{
    string studentName; // 한글 최대 6글자
    int grade; // 학년
    int maxCredit; // 18 19 20 21 중 랜덤
    List<Lecture> currentLectures; //
    RequirementBase[] requirements; // 요구사항 3개 고정, 필수 2개 선택 1개

    void Start()
    {
        requirements = new RequirementBase[3];
    }

    public void GetCurrentLectures(List<Lecture> lectureList)
    {
        lectureList = currentLectures;
    }
/*    public int GetCurrentLecturesNum()
    {
        return currentLectures.Count;
    }

    public int GetCurrentLecturesCredits() // 현재 담은 과목 학점 총합 반환
    {
        int currentCredits = 0;
        foreach(Lecture lecture in currentLectures)
        {
            currentCredits += lecture.credit;
        }

        return currentCredits;
    }*/
}
