using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    //Coroutine co;               // 内风凭
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        //Managers.Resource.Instantiate("UI/UI_Button");
        //Managers.UI.ShowPopupUI<UI_Button>();
        Managers.UI.ShowSceneUI<UI_Inven>();

        //for (int i = 0; i < 5; i++)
        //{
        //    Managers.Resource.Instantiate("UnityChan");
        //}

        //co = StartCoroutine("ExplodeAfterSeconds", 4.0f);   // 内风凭, Coroutine
        //StartCoroutine("CoStopExplode", 2.0f);

    }

    //IEnumerator CoStopExplode(float seconds)                      // Coroutine 内风凭
    //{
    //    Debug.Log("Stop Enter");
    //    yield return new WaitForSeconds(seconds);
    //    Debug.Log("Stop Execute!!");
    //    if(co != null)
    //    {
    //        StopCoroutine(co);
    //        co = null;
    //    }
    //}
    //IEnumerator ExplodeAfterSeconds(float seconds)
    //{
    //    Debug.Log("Explode Enter");
    //    yield return new WaitForSeconds(seconds);
    //    Debug.Log("Explode Execute!!");
    //    co = null;
    //}


    public override void Clear()
    {

    }

}
