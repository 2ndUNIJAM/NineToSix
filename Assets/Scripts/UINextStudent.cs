using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UINextStudent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI studentName;
    [SerializeField] private TextMeshProUGUI studentDept;

    public void SetStudent(Student student)
    {
        studentName.text = student.GetStudentName();
        studentDept.text = $"{student.GetStudentDepartment()} {student.GetStudentGrade()}학년";
    }
}
