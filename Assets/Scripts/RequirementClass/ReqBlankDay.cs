using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqBlankDay : RequirementBase
// �������� �ִ��� Ȯ���ϴ� ��û����
// i) Ư�� ���� ���� �� ���� ���
// ii) ������ �� �ּ� 1�� ���� ��� 
{
    UISchedule finalSchedule;
    bool needSpecificDay; // true�� ��� i) ����
    int needDay;

    public ReqBlankDay()
    {
        base.GetLogicManager();

        needSpecificDay = Random.Range(0, 2) == 1 ? true : false;
        finalSchedule = gLogicManager.Schedule;

        if (needSpecificDay)
        {
            needDay = Random.Range(0, 5); // ��ȭ����� 01234 �� ����
            string day = string.Empty;
            switch (needDay)
            {
                case 0:
                    day = "������";
                    break;
                case 1:
                    day = "ȭ����";
                    break;
                case 2:
                    day = "������";
                    break;
                case 3:
                    day = "�����";
                    break;
                case 4:
                    day = "�ݿ���";
                    break;
                default:
                    break;
            }
            contentTitle = $"{day} ���� ���!";
        }
        else
        {
            contentTitle = "�ּ� 1�� ���� ���!";
        }
    }

    public override bool DoesMeetRequirement()
    {
        if(needSpecificDay) 
        { 
        
        }

        throw new System.NotImplementedException();
    }

}
