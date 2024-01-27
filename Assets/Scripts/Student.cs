using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student
{
    const string department = "시작이반학과";
    string studentName; // 한글 최대 6글자
    int studentID;
    int grade; // 학년
    int maxCredit; // 18 19 20 21 중 랜덤
    Sprite profile; // 프로필사진
    List<Lecture> currentLectures; //
    RequirementBase[] requirements; // 요구사항 3개 고정, 필수 2개 선택 1개

    public Student(string name, int id)
    {
        studentName = name;
        studentID = id;
        grade = Random.Range(1, 5);
        maxCredit = Random.Range(18, 22);
        currentLectures = new List<Lecture>();
        profile = Resources.Load<Sprite>("profile_" + (id < 5 ? id.ToString() : "default"));
        GenerateRequirements();
    }

    private void GenerateRequirements()
    {
        requirements = new RequirementBase[3];
        var candidate = new RequirementBase[] { new ReqMustLecture(this.grade), new ReqLectureNumber(), new ReqAverageRating(), new ReqNeedLunch(), new ReqBlankDay() };
        var excludeEssential = Random.Range(0, 3);
        var includeOptional = excludeEssential == 2 ? Random.Range(2, 5) : Random.Range(3, 5);
        var essentialIndex = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            if (i != excludeEssential)
                essentialIndex.Add(i);
        }
        requirements[0] = candidate[essentialIndex[0]];
        requirements[0].essential = true;
        requirements[1] = candidate[essentialIndex[1]];
        requirements[1].essential = true;
        requirements[2] = candidate[includeOptional];
        requirements[2].essential = false;
    }

    public void GetCurrentLectures(List<Lecture> lectureList)
    {
        lectureList = currentLectures;
    }

    public string GetStudentName() { return studentName; }
    public string GetStudentDepartment() { return department; }
    public int GetStudentID() { return studentID; }
    public int GetStudentGrade() { return grade; }
    public int GetStudentMaxCredit() { return maxCredit; }
    public Sprite GetStudentProfile() { return profile; }

    public RequirementBase GetFirstRequirement() { return requirements[0]; }
    public RequirementBase GetSecondRequirement() { return requirements[1]; }
    public RequirementBase GetLastRequirement() { return requirements[2]; }

}
