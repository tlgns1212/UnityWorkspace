using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseEventAction = null;

    bool _pressed = false;

    public void OnUpdate()
    {
        // 이벤트가 실행됐는지 확인
        if (EventSystem.current.IsPointerOverGameObject())
            return;


        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if(MouseEventAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                MouseEventAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if(_pressed)
                {
                    MouseEventAction.Invoke(Define.MouseEvent.Click);
                    _pressed = false;
                }
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseEventAction = null;
    }
}
