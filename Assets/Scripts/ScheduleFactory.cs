using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class ScheduleFactory
{
    static bool TFRandomDecide()
    {
        return (Random.Range(0, 2) == 1 ? true : false);
    }

    // int�� ���� ���� 1�����̸� 1��, 2�����̸� 2��, 3�����̸� 3�� �� 
    static List<Vector2Int> GenerateLectureBlock(int credit)
    {
        List<Vector2Int> returnBlock = new List<Vector2Int>();

        // x col(����) 0~4 / y row(�ð�) 0~7
        // ������x 1����y
        int xPos = 0, yPos = 0;
        int xMin = 0, xMax = 5;
        int yMin = 0, yMax = 8;

        switch(credit)
        {
            case 1:
                returnBlock.Add(new Vector2Int(Random.Range(xMin, xMax), Random.Range(yMin, yMax)));
                break;

            case 2:
                if(TFRandomDecide()) // ���������� ��� �߰��ϴ� ���
                {
                    xPos = Random.Range(xMin, xMax-1); // 0 1 2 3 
                    yPos = Random.Range(yMin, yMax);
                    returnBlock.Add(new Vector2Int(xPos, yPos));
                    returnBlock.Add(new Vector2Int(xPos + 1, yPos));
                }
                else // �Ʒ������� ��� �߰��ϴ� ���
                {
                    xPos = Random.Range(xMin, xMax);
                    yPos = Random.Range(yMin, yMax - 1);
                    returnBlock.Add(new Vector2Int(xPos, yPos));
                    returnBlock.Add(new Vector2Int(xPos, yPos+1));
                }
                break;
            case 3:
                if(TFRandomDecide()) // true: Line��
                {
                    if(TFRandomDecide()) // true: ������ ����
                    {
                        xPos = Random.Range(xMin, xMax - 2);
                        yPos = Random.Range(yMin, yMax);
                        returnBlock.Add(new Vector2Int(xPos, yPos));
                        returnBlock.Add(new Vector2Int(xPos+1, yPos));
                        returnBlock.Add(new Vector2Int(xPos+2, yPos));
                    }
                    else // false: �Ʒ��� ����
                    {
                        xPos = Random.Range(xMin, xMax);
                        yPos = Random.Range(yMin, yMax - 2);
                        returnBlock.Add(new Vector2Int(xPos, yPos));
                        returnBlock.Add(new Vector2Int(xPos, yPos+1));
                        returnBlock.Add(new Vector2Int(xPos, yPos+2));
                    }
                }
                else // ������ 
                {
                    // Set x
                    bool generateLeft = TFRandomDecide();
                    if(generateLeft) // true: ���� ����
                    {
                        xPos = Random.Range(xMin+1, xMax);
                    }
                    else // false: ������ ����
                    {
                        xPos = Random.Range(xMin, xMax-1);
                    }

                    // Set y
                    bool generateUp = TFRandomDecide();
                    if(generateUp) // true: ���� ����
                    {
                        yPos = Random.Range(yMin + 1, yMax);
                    }
                    else // �Ʒ��� ����
                    {
                        yPos = Random.Range(yMin, yMax-1);
                    }

                    returnBlock.Add(new Vector2Int(xMin, yPos));
                    
                    // �� �� ��� �� �ϳ� ����
                    if(generateLeft)
                    {
                        returnBlock.Add(new Vector2Int(xPos-1, yPos));
                    }
                    else
                    {
                        returnBlock.Add(new Vector2Int(xPos+1, yPos));
                    }

                    // �� �Ʒ� ��� �� �ϳ� ����
                    if(generateUp)
                    {
                        returnBlock.Add(new Vector2Int(xPos, yPos-1));
                    }
                    else
                    {
                        returnBlock.Add(new Vector2Int(xPos, yPos+1));
                    }
                }
                break;
            default:
                Debug.Log("Wrong credit input in ScheduleFactory");
                break;
        }

        return returnBlock;
    }
    
    // output list<vector2int>

}
