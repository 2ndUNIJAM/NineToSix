using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELectureType // 다른 파일에서도 써야됨
{
    LiberalArt, // 교양
    MajorBasic, // 전공기초
    MajorRequired // 전공필수 
}

[System.Flags]
public enum ELectureGrade
{
    First = 1,
    Second = 2,
    Third = 4,
    Fourth = 8
}


public class Lecture
{
    public LectureData Data => _data;

    public int Credit => _credit;

    public int Rating => _rating;

    public IEnumerable<Vector2Int> Schedule => _schedule;
    
    public int credit; // 학점
    public int profRating; // 0~5 사이의 0.5 간격 랜덤

    // Implement: lectureTime (시간표 내 들어가는 과목 시간)
    // 1학점 1한칸, 2학점 2칸, 3학점 3칸 

    // Implement: 들을 수 있는 학년 변수

    private int _credit, _rating;
    private LectureData _data;
    private List<Vector2Int> _schedule;

    public Lecture(LectureData data)
    {
        _data = data;
        
        // Credit, Rating 랜덤 생성
        // Credit은 교양일 때만 1~3, 전기/전필시 무조건 3
        if (_data.Type == ELectureType.LiberalArt)
            _credit = Random.Range(1, 4);
        else
            _credit = 3;
        
        
    }
}
