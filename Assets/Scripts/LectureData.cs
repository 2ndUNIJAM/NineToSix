using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public struct LectureData
{
    public int ID => _id;

    public string Name => _name;

    public ELectureType Type => _type;

    public ELectureGrade TargetGrade => _targetGrade;

    [JsonProperty("lecture_id")] private int _id;
    [JsonProperty("lecture_name")] private string _name;
    [JsonProperty("lecture_type")] private ELectureType _type;
    [JsonProperty("rec_grade")] private ELectureGrade _targetGrade;
}
