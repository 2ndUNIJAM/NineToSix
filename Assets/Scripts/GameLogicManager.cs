using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    [SerializeField] List<Student> studentList;
    
    public void GetCurrentStudent(Student student)
    {
        student = studentList[0];
    }

}
