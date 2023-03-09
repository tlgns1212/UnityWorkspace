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

        Managers.Map.LoadMap(1);

        GameObject player = Managers.Resource.Instantiate("Creature/Player");
        player.name = "Player";
        Managers.Object.Add(player);

        for(int i = 0; i < 5; i++)
        {
            GameObject monster = Managers.Resource.Instantiate("Creature/Monster");
            monster.name = $"Monster_{i+1}";

            // 랜덤 위치 스폰 (일단 겹쳐도 OK)
            Vector3Int pos = new Vector3Int()
            {
                x = Random.Range(-20, 20),
                y = Random.Range(-10, 10)
            };

            MonsterController mc = monster.GetComponent<MonsterController>();
            mc.CellPos = pos;

            Managers.Object.Add(monster);
        }



        //Managers.Resource.Instantiate("UI/UI_Button");
        //Managers.UI.ShowPopupUI<UI_Button>();

        //Dictionary<int,Stat> dict =  Managers.Data.StatDict;

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
