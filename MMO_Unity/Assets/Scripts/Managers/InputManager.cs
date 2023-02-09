using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseEventAction = null;

    bool _pressed = false;

    public void OnUpdate()
    {
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
}
