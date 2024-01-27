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

    public int TimeLimit => _timeLimit;

    public IEnumerable<Vector2Int> Schedule => _schedule;

    // Implement: lectureTime (시간표 내 들어가는 과목 시간)
    // 1학점 1한칸, 2학점 2칸, 3학점 3칸 

    // Implement: 들을 수 있는 학년 변수

    private int _credit, _rating, _timeLimit;
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

        _rating = Random.Range(2, 10) % 5 + 1;
        _timeLimit = _data.Type switch
        {
            ELectureType.LiberalArt => 7,
            ELectureType.MajorBasic => 8,
            ELectureType.MajorRequired => 9,
            _ => 5
        };

        // TODO: 스케쥴 생성
        _schedule = ScheduleFactory.GenerateLectureBlock(_credit);
    }
}
