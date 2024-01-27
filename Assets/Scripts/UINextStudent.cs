using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UINextStudent : MonoBehaviour
{
    [SerializeField] private Image studentImage;
    [SerializeField] private TextMeshProUGUI studentName;
    [SerializeField] private TextMeshProUGUI studentDept;

    public void SetStudent(Student student)
    {
        studentImage.sprite = student.GetStudentProfile();
        studentName.text = student.GetStudentName();
        studentDept.text = $"{student.GetStudentDepartment()} {student.GetStudentGrade()}학년";
    }
}
