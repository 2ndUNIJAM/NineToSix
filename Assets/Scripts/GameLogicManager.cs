using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    public UISchedule Schedule => schedule;
    
    [SerializeField] private UISchedule schedule;
    
    List<Student> studentList;
    private List<Lecture> _selectedLectures;
    
    public void GetCurrentStudent(Student student)
    {
        student = studentList[0];
    }

    public IEnumerable<Lecture> GetSelectedLectures()
    {
        yield return null;
    }
    
    

    public void ReserveLecture(Lecture lecture)
    {
        
    }

    public void OnLectureKeyActionStart(Lecture lecture)
    {
        
    }

    public void OnLectureKeyActionFinish(bool succeed)
    {
        
    }
}
