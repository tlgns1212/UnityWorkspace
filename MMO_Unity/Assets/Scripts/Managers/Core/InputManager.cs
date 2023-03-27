using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action<Define.ArrowKeyEvent> KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressedMouse = false;
    bool _pressedKey = false;
    float _pressedMouseTime = 0;
    float _pressedKeyTime = 0;

    public void OnUpdate()
    {
        // 이벤트가 실행됐는지 확인
        if (EventSystem.current.IsPointerOverGameObject())
            return;


        if (KeyAction != null)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D))
            {
                if (!_pressedKey)
                {
                    KeyAction.Invoke(Define.ArrowKeyEvent.PressDown);
                    _pressedKeyTime = Time.time;
                }
                KeyAction.Invoke(Define.ArrowKeyEvent.Press);
                _pressedKey = true;
            }
            if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.D))
            {
                if (_pressedKey)
                {
                    if (Time.time < _pressedKeyTime + 0.2f)
                        KeyAction.Invoke(Define.ArrowKeyEvent.Click);
                    KeyAction.Invoke(Define.ArrowKeyEvent.PressUp);
                }
                _pressedKey = false;
                _pressedKeyTime = 0;
            }
        }
        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (!_pressedMouse)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedMouseTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressedMouse = true;
            }
            if(Input.GetMouseButtonUp(0))
            {
                if (_pressedMouse)
                {
                    if (Time.time < _pressedMouseTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                _pressedMouse = false;
                _pressedMouseTime = 0;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
