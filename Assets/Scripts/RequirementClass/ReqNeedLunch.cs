using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqNeedLunch : RequirementBase
// ��ȭ����� ���ɽð� 4���ð� ��� ����־�� �ϴ� ��û���� 
{

    public ReqNeedLunch()
    {
        base.GetLogicManager();

        contentTitle = string.Empty;

    }

    public override bool DoesMeetRequirement()
    {
        throw new System.NotImplementedException();
    }
}
