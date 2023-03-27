using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    //Coroutine co;               // 코루틴
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        //Managers.Resource.Instantiate("UI/UI_Button");
        //Managers.UI.ShowPopupUI<UI_Button>();
        //Managers.UI.ShowSceneUI<UI_Inven>();              // 인벤토리

        Dictionary<int, Data.Stat> dict =  Managers.Data.StatDict;

        gameObject.GetOrAddComponent<CursorController>();


        GameObject player =  Managers.Game.Spawn(Define.WorldObject.Player, "Creature/UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
        player.GetOrAddComponent<PlayerController>().SetCamera(Camera.main);

        GameObject go = new GameObject { name = "SpawningPool" };

        // 몬스터 소환
        //SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        //pool.SetKeepMonsterCount(5);



        //for (int i = 0; i < 5; i++)
        //{
        //    Managers.Resource.Instantiate("UnityChan");
        //}

        //co = StartCoroutine("ExplodeAfterSeconds", 4.0f);   // 코루틴, Coroutine
        //StartCoroutine("CoStopExplode", 2.0f);

    }

    //IEnumerator CoStopExplode(float seconds)                      // Coroutine 코루틴
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
