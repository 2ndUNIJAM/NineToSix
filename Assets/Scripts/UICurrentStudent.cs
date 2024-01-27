using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICurrentStudent : MonoBehaviour
{
    [SerializeField] private Image studentImage;
    [SerializeField] private TextMeshProUGUI studentName;
    [SerializeField] private TextMeshProUGUI studentDept;
    [SerializeField] private TextMeshProUGUI maxCredit;
    [SerializeField] private List<UIRequirement> requirements;

    public void SetStudent(Student student)
    {
        studentImage.sprite = student.GetStudentProfile();
        studentName.text = student.GetStudentName();
        studentDept.text = $"{student.GetStudentDepartment()} {student.GetStudentGrade()}학년";
        maxCredit.text = $"수강가능학점 : {student.GetStudentMaxCredit()}";
        requirements[0].SetText(student.GetFirstRequirement().contentTitle);
        requirements[1].SetText(student.GetSecondRequirement().contentTitle);
        requirements[2].SetText(student.GetLastRequirement().contentTitle);
        requirements[0].CheckBox(false);
        requirements[1].CheckBox(false);
        requirements[2].CheckBox(false);
    }

    public void CheckRequirements(Student student)
    {
        requirements[0].CheckBox(student.GetFirstRequirement().DoesMeetRequirement());
        requirements[1].CheckBox(student.GetSecondRequirement().DoesMeetRequirement());
        requirements[2].CheckBox(student.GetLastRequirement().DoesMeetRequirement());
    }
}
