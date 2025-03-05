using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogBoxLoader : ImageLoader
{
    #region field
    protected TextLoader _text;
    #endregion

    #region property
    public TextLoader Text { get => _text; set => _text = value; }
    #endregion

    #region method
    public DialogBoxLoader(
        string resourcesPath, string imageName, string textName, Transform parentTransform, Vector2 referenceImagePixel, 
        string textContent, int fontSize, Color textColor, TextAlignmentOptions textAlignmentOptions, TMP_FontAsset font, float textScale,
        bool isTextNeeded = true, float scaleRatio = 1, bool isHeightBench = true, Vector2? objectPos = null, Vector2? pivotPos = null) : 
        base(resourcesPath, imageName, parentTransform, referenceImagePixel, scaleRatio, isHeightBench, objectPos, pivotPos)
    {   
        _text = isTextNeeded ? new(textName, _gameObject.transform, referenceImagePixel, textContent, fontSize, textColor, textAlignmentOptions, font, false, 
            textScale, true, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)) : null;
    }
    #endregion
}
