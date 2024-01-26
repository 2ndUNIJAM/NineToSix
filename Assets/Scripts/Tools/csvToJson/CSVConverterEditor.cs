#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;

[CustomEditor(typeof(CSVConverter))]
public class CSVConverterEditor : Editor
{
    private struct lecture
    {
        public int id;
        public int type;
        public int rec_grade;
        public string name;
    }

    private CSVConverter csvConverter;
    private void OnEnable()
    {
        csvConverter = target as CSVConverter;
    }

    public override void OnInspectorGUI()
    {
        csvConverter.CSVData = (TextAsset)EditorGUILayout.ObjectField("csv 파일", csvConverter.CSVData, typeof(TextAsset), true);
        if (GUILayout.Button("Convert CSV to Json"))
            ConvertCSVToJson();
    }

    private void ConvertCSVToJson()
    {
        var data = CSVReader.ReadToString(csvConverter.CSVData);

        var lectureList = new List<lecture>();
        var everyDataValid = true;

        foreach (var dict in data)
        {
            var lecture = LectureBy(dict, out bool canParse);
            if (!canParse || !IsValid(lecture))
            {
                everyDataValid = false;
                continue;
            }
            lectureList.Add(lecture);
        }

        if (everyDataValid)
        {
            var saveData = JsonConvert.SerializeObject(lectureList);
            File.WriteAllText(Application.dataPath + $"/Resources/lectures.json", saveData);
            Debug.Log("변환 완료 : 모든 데이터가 유효합니다");
            Debug.Log($"lectures.json is created in {Application.dataPath + "/Resources"}");
        }
        else
            Debug.Log("변환 실패 : 일부 데이터가 유효하지 않습니다");
    }

    private static lecture LectureBy(Dictionary<string, string> dict, out bool canParse)
    {
        var keys = new List<string>(dict.Keys);
        var result = new lecture();
        canParse = true;
        foreach (var key in keys)
        {
            switch (key)
            {
                case "id":
                    if (int.TryParse(dict[key], out int id))
                        result.id = id;
                    else
                    {
                        Debug.Log($"과목 {dict["name"]}의 id가 정수가 아닙니다");
                        canParse = false;
                    }
                    break;

                case "name":
                    result.name = dict[key];
                    break;

                case "type":
                    if (int.TryParse(dict[key], out int type))
                        result.type = type;
                    else
                    {
                        Debug.Log($"과목 {dict["name"]}의 type이 정수가 아닙니다");
                        canParse = false;
                    }
                    break;

                case "rec_grade":
                    if (int.TryParse(dict[key], out int rec_grade))
                    {
                        int parsed_grade = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (rec_grade % 10 == 1)
                                parsed_grade += (int)Math.Pow(2, i);
                            if (rec_grade % 10 > 1)
                            {
                                Debug.Log($"과목 {dict["name"]}의 {i + 1}번째 비트가 유효하지 않습니다");
                                canParse = false;
                            }
                            rec_grade /= 10;
                        }
                        result.rec_grade = parsed_grade;
                    }
                    else
                    {
                        Debug.Log($"과목 {dict["name"]}의 type이 정수가 아닙니다");
                        canParse = false;
                    }
                    break;

                default:
                    break;
            }
        }
        return result;
    }

    private static bool IsValid(lecture lecture)
    {
        bool isValid = true;
        int[] gradeForMajorBasic = { 3, 6, 12 };
        int[] gradeForMajorRequired = { 1, 2, 4, 8 };

        if (lecture.type < 0 || lecture.type > 2)
        {
            isValid = false;
            Debug.Log($"과목 {lecture.name}의 타입은 기준 범위를 벗어납니다.");
        }

        if (lecture.id < 0)
        {
            isValid = false;
            Debug.Log($"과목 {lecture.name}의 id가 0보다 작습니다.");
        }

        if (lecture.name.Length > 10)
        {
            isValid = false;
            Debug.Log($"과목 {lecture.name}의 이름은 10자보다 깁니다.");
        }

        if (lecture.rec_grade < 1 || lecture.rec_grade > 15)
        {
            isValid = false;
            Debug.Log($"과목 {lecture.name}의(는) 권장 학년은 기준 범위를 벗어납니다.");
        }

        if (lecture.type == 0 && lecture.rec_grade != 15)
        {
            isValid = false;
            Debug.Log($"과목 {lecture.name}은(는) 교양과목임에도 전 학년이 수강하지 못합니다.");
        }

        if (lecture.type == 1 && !gradeForMajorBasic.Contains(lecture.rec_grade))
        {
            isValid = false;
            Debug.Log($"과목 {lecture.name}은(는) 전공기초지만, 연속한 두 학년만 수강할 수 있다는 법칙에 어긋납니다.");
        }

        if (lecture.type == 2 && !gradeForMajorRequired.Contains(lecture.rec_grade))
        {
            isValid = false;
            Debug.Log($"과목 {lecture.name}은(는) 전공필수지만, 특정 한 학년만 수강할 수 있다는 법칙에 어긋납니다.");
        }

        return isValid;
    }
}

#endif