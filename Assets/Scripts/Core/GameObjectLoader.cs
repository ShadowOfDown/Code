using UnityEngine;

public class GameObjectLoader
{
    #region field
    protected readonly Transform _parentTransform;
    protected readonly string _objectName;
    protected readonly Vector2 _referencePixel;
    protected RectTransform _rectTransform;
    protected GameObject _gameObject;
    public Transform GetParentTransform() { return _parentTransform; } 
    public string GetObjectName() { return _objectName;}
    public Vector2 GetReferencePixel() { return _referencePixel; }
    #endregion
    
    #region property
    public RectTransform RectTransform
    {
        get { return _rectTransform; }
        set { _rectTransform = value; }
    }

    public GameObject GameObject
    {
        get { return _gameObject; }
        set { _gameObject = value; }
    }

    public Vector2 ReferencePixel => _referencePixel;
    #endregion

    #region method
    public GameObjectLoader(
        string objectName, Transform parentTransform, Vector2 referencePixel, 
        float scaleRatio = 1, bool isHeightBench = true, Vector2? objectPos = null, Vector2? pivotPos = null)
    {
        _parentTransform = parentTransform;
        _objectName = objectName;
        _referencePixel = referencePixel;

        _gameObject = new GameObject(_objectName);
        _gameObject.transform.SetParent(parentTransform, false);

        _rectTransform = SetRectTransform(_gameObject, parentTransform, referencePixel, scaleRatio, isHeightBench, objectPos, pivotPos);

        Debug.Log($"GameObject {objectName} is loaded");
    }

    public void setActive(bool isActive) { _gameObject.SetActive(isActive); }

    public static RectTransform SetRectTransform(GameObject gameObject, Transform parentTransform, Vector2 referencePixel, float scaleRatio = 1, bool isHeightBench = true, Vector2? objectPos = null, Vector2? pivotPos = null)
    {
        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = objectPos ?? new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = objectPos ?? new Vector2(0.5f, 0.5f);
        rectTransform.pivot = pivotPos ?? new Vector2(0.5f, 0.5f);
        rectTransform.localScale = Vector2.one;

        float convertRatio = isHeightBench ? 
            parentTransform.GetComponent<RectTransform>().rect.height * scaleRatio / referencePixel.y :
            parentTransform.GetComponent<RectTransform>().rect.width * scaleRatio / referencePixel.x;
        rectTransform.sizeDelta = new Vector2(referencePixel.x * convertRatio, referencePixel.y * convertRatio);

        return rectTransform;
    }

    public void SetActive(bool isActive) { _gameObject.SetActive(isActive); }
    #endregion 
}
