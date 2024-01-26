using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICurrentStudent : MonoBehaviour
{
    [SerializeField] private Image studentImage;
    [SerializeField] private TextMeshProUGUI studentName;
    [SerializeField] private TextMeshProUGUI studentDept;
    [SerializeField] private TextMeshProUGUI maxCredit;
    [SerializeField] private UIRequirement requirementTemplate;
    [SerializeField] private RectTransform requirementParent;

    private List<UIRequirement> _requirements;

    private void Awake()
    {
        _requirements = new List<UIRequirement>();
    }

    public void SetStudent(Student student)
    {
        
    }
}
