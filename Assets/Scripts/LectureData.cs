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

    [JsonProperty("id")] private int _id;
    [JsonProperty("name")] private string _name;
    [JsonProperty("type")] private ELectureType _type;
    [JsonProperty("rec_grade")] private ELectureGrade _targetGrade;
}
