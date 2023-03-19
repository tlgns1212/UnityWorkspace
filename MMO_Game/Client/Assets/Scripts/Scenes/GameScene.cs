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
        Screen.SetResolution(640, 480, false);

        //GameObject player = Managers.Resource.Instantiate("Creature/Player");
        //player.name = "Player";
        //Managers.Object.Add(player);

        //for(int i = 0; i < 5; i++)
        //{
        //    GameObject monster = Managers.Resource.Instantiate("Creature/Monster");
        //    monster.name = $"Monster_{i+1}";

        //    // 랜덤 위치 스폰 (일단 겹쳐도 OK)
        //    Vector3Int pos = new Vector3Int()
        //    {
        //        x = Random.Range(-20, 20),
        //        y = Random.Range(-10, 10)
        //    };
        //    if (pos.x < Managers.Map.MinX || pos.x > Managers.Map.MaxX)
        //        continue;
        //    if (pos.y < Managers.Map.MinY || pos.y > Managers.Map.MaxY)
        //        continue;

        //    MonsterController mc = monster.GetComponent<MonsterController>();
        //    mc.CellPos = pos;

        //    Managers.Object.Add(monster);
        //}


    }


    public override void Clear()
    {

    }

}
