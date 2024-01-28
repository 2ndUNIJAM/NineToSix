using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryScene : MonoBehaviour
{
    IEnumerator Start()
    {
        // TODO: 로고 보여주기......?
        
        GameManager.Instance.ChangeToScene("TitleScene");
        yield break;
    }
}
