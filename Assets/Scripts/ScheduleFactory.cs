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

    // int값 따라서 생성 1학점이면 1개, 2학점이면 2개, 3학점이면 3개 셀 
    static List<Vector2Int> GenerateLectureBlock(int credit)
    {
        List<Vector2Int> scheduleBlock = new List<Vector2Int>();

        // x col(요일) 0~4 / y row(시간) 0~7
        // 월요일x 1교시y
        int xPos = 0, yPos = 0;
        int xMin = 0, xMax = 5;
        int yMin = 0, yMax = 8;

        switch(credit)
        {
            case 1:
                scheduleBlock.Add(new Vector2Int(Random.Range(xMin, xMax), Random.Range(yMin, yMax)));
                break;

            case 2:
                if(TFRandomDecide()) // 오른쪽으로 블록 추가하는 경우
                {
                    xPos = Random.Range(xMin, xMax-1); // 0 1 2 3 
                    yPos = Random.Range(yMin, yMax);
                    scheduleBlock.Add(new Vector2Int(xPos, yPos));
                    scheduleBlock.Add(new Vector2Int(xPos + 1, yPos));
                }
                else // 아래쪽으로 블록 추가하는 경우
                {
                    xPos = Random.Range(xMin, xMax);
                    yPos = Random.Range(yMin, yMax - 1);
                    scheduleBlock.Add(new Vector2Int(xPos, yPos));
                    scheduleBlock.Add(new Vector2Int(xPos, yPos+1));
                }
                break;
            case 3:
                if(TFRandomDecide()) // true: Line형
                {
                    if(TFRandomDecide()) // true: 오른쪽 전개
                    {
                        xPos = Random.Range(xMin, xMax - 2);
                        yPos = Random.Range(yMin, yMax);
                        scheduleBlock.Add(new Vector2Int(xPos, yPos));
                        scheduleBlock.Add(new Vector2Int(xPos+1, yPos));
                        scheduleBlock.Add(new Vector2Int(xPos+2, yPos));
                    }
                    else // false: 아래쪽 전개
                    {
                        xPos = Random.Range(xMin, xMax);
                        yPos = Random.Range(yMin, yMax - 2);
                        scheduleBlock.Add(new Vector2Int(xPos, yPos));
                        scheduleBlock.Add(new Vector2Int(xPos, yPos+1));
                        scheduleBlock.Add(new Vector2Int(xPos, yPos+2));
                    }
                }
                else // ㄴ자형 
                {
                    // Set x
                    bool generateLeft = TFRandomDecide();
                    if(generateLeft) // true: 왼쪽 생성
                    {
                        xPos = Random.Range(xMin+1, xMax);
                    }
                    else // false: 오른쪽 생성
                    {
                        xPos = Random.Range(xMin, xMax-1);
                    }

                    // Set y
                    bool generateUp = TFRandomDecide();
                    if(generateUp) // true: 위쪽 생성
                    {
                        yPos = Random.Range(yMin + 1, yMax);
                    }
                    else // 아래쪽 생성
                    {
                        yPos = Random.Range(yMin, yMax-1);
                    }

                    scheduleBlock.Add(new Vector2Int(xMin, yPos));
                    
                    // 좌 우 블록 중 하나 생성
                    if(generateLeft)
                    {
                        scheduleBlock.Add(new Vector2Int(xPos-1, yPos));
                    }
                    else
                    {
                        scheduleBlock.Add(new Vector2Int(xPos+1, yPos));
                    }

                    // 위 아래 블록 중 하나 생성
                    if(generateUp)
                    {
                        scheduleBlock.Add(new Vector2Int(xPos, yPos-1));
                    }
                    else
                    {
                        scheduleBlock.Add(new Vector2Int(xPos, yPos+1));
                    }
                }
                break;
            default:
                Debug.Log("Wrong credit input in ScheduleFactory");
                break;
        }

        return scheduleBlock;
    }
    
    // output list<vector2int>

}
