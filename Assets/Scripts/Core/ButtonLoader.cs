using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonLoader : DialogBoxLoader
{
    #region field
    private Sprite _pressedSprite;
    private Button _button;
    private UnityAction _onClick;
    private UnityAction _onLongPress;
    private float _longPressThreshold;
    #endregion

    #region contructor
    public ButtonLoader(string resourcesPath, string normalImageName, string PressedSpriteName, string textName, Transform parentTransform, Vector2 referenceImagePixel,
        string textContent, int fontSize, Color textColor, TextAlignmentOptions textAlignmentOptions, TMP_FontAsset font, float textScale, bool isTextNeeded = true, 
        float scaleRatio = 1, bool isHeightBench = true, Vector2? objectPos = null, Vector2? pivotPos = null, 
        float longPressThreshold = 0.5f, UnityAction onClick = null, UnityAction onLongPress = null) :
        base(resourcesPath, normalImageName, textName, parentTransform, referenceImagePixel, 
            textContent, fontSize, textColor, textAlignmentOptions, font, textScale, isTextNeeded, scaleRatio, isHeightBench, objectPos, pivotPos)
    {

        _longPressThreshold = longPressThreshold;
        
        _pressedSprite = SetSprite(resourcesPath + PressedSpriteName);
        _onClick = onClick;
        _onLongPress = onLongPress;

        _button = SetButton(_gameObject, _onClick, _onLongPress);
    }
    #endregion
    
    #region property

    public Sprite PressedSprite
    {
        get { return _pressedSprite; }
        set { _pressedSprite = value; }
    }

    public Button ButtonComponent
    {
        get { return _button; }
        private set { _button = value; }
    }

    public float LongPressThreshold
    {
        get { return _longPressThreshold; }
        set { _longPressThreshold = value; }
    }

    public UnityAction OnClick
    {
        get { return _onClick; }
        set { _onClick = value; }
    }

    public UnityAction OnLongPress
    {
        get { return _onLongPress;}
        set { _onLongPress = value;}
    }
    #endregion

    #region public methods
    public Button SetButton(GameObject buttonObject, UnityAction onClick = null, UnityAction onLongPress = null)
    {
        Button button = buttonObject.GetComponent<Button>();
        
        // 添加交互逻辑
        EventTrigger trigger = buttonObject.AddComponent<EventTrigger>();
        float pressTime = 0f; // 按下时间

        // PointerDown事件：开始按下
        EventTrigger.Entry entryDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entryDown.callback.AddListener((data) =>
        {
            _image.sprite = _pressedSprite; // 切换为长按状态的图片
            pressTime = Time.time; // 记录按下时间
        });
        trigger.triggers.Add(entryDown);

        // PointerUp事件：结束按下
        EventTrigger.Entry entryUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entryUp.callback.AddListener((data) =>
        {
            _image.sprite = _sprite; // 恢复为正常状态的图片

            // 判断是否是长按
            if (Time.time - pressTime > _longPressThreshold)
            {
                onLongPress?.Invoke(); // 调用长按事件
            }
            else
            {
                onClick?.Invoke(); // 调用点击事件
            }
        });
        trigger.triggers.Add(entryUp);

        return button;
    }
    #endregion

    #region private methods
    #endregion
    
}
