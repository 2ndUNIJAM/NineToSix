using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELectureType // 다른 파일에서도 써야됨
{
    MajorRequired, // 전공필수 
    MajorBasic, // 전공기초
    LiberalArt // 교양
}

public class Lecture : MonoBehaviour
{
    [SerializeField] string lectureName;
    public ELectureType lectureType;
    public int credit; // 학점
    public int profRating; // 0~5 사이의 0.5 간격 랜덤

    // Implement: lectureTime (시간표 내 들어가는 과목 시간)
    // 1학점 1한칸, 2학점 2칸, 3학점 3칸 

    // Implement: 들을 수 있는 학년 변수

}
