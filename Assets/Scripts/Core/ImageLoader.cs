using UnityEngine;
using UnityEngine.UI; // 引入命名空间以访问 Image 组件

public class ImageLoader : GameObjectLoader
{
    #region field
    protected Image _image; // 用于存储 Image 组件的引用
    protected Sprite _sprite; // 用于存储加载的 Sprite
    protected Texture2D _texture;
    #endregion
    
    #region property
    public Texture2D Texture
    {
        get { return _texture; }
        set { _texture = value; }
    }

    public Sprite Sprite
    {
        get { return _sprite; }
        set { _sprite = value; }
    }

    public Image Image
    {
        get { return _image; }
        set { _image = value; }
    }
    #endregion

    // 构造函数
    #region method
    public ImageLoader(
        string resourcesPath, string objectName, Transform parentTransform, Vector2 referencePixel, 
        float scaleRatio = 1, bool isHeightBench = true, Vector2? objectPos = null, Vector2? pivotPos = null) : 
        base(objectName, parentTransform, referencePixel, scaleRatio, isHeightBench, objectPos, pivotPos)
    {
        _image = SetImage(_gameObject, resourcesPath + _objectName);
        _sprite = Image.sprite;
        _texture = _sprite.texture;

        Debug.Log($"successfully load {resourcesPath + _objectName} image");
    }

    public static Sprite SetSprite(string texturePath)
    {   
        Debug.Log($"load {texturePath}");
        Texture2D texture = Resources.Load<Texture2D>(texturePath);
        if (texture == null)
        {
            Debug.LogError($"Texture not found: {texturePath}");
            return null;
        }

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        if (sprite == null)
        {
            Debug.LogError($"Sprite not created: {texturePath}");
            return null;
        }

        return sprite;            
    }


    public static Image SetImage(GameObject gameObject, string texturePath)
    {
        Sprite sprite= SetSprite(texturePath);
        
        Image image = gameObject.AddComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = true;

        return image;
    }
    #endregion
}