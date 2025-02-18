using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DramaPerformSystem : MonoBehaviour
{
    #region field

    /* 定义一些数据 */
    // 文件夹
    private const string folderPath = @"Textures\DramaPerformSystem\";
    Canvas dramaPerformCanvas = null;
    private TMP_FontAsset font = null;
    // 屏幕状态切换:
    // [isCharacterImageDisplaying isNameCardDisplaying, isDialogBoxDisplaying]
    // 按照 ppt 上面可以被编码为: 0000 0110 0111 0101
    // 使用 ArrayList 存储每个屏幕的渲染状态
    private bool isDramaPerformSystem = false; // 默认是演出系统
    private readonly ArrayList dramaScreenArrayList = new()
    {
        new ArrayList { false, false, false }, // 屏幕 0
        new ArrayList { true, true, false }, // 屏幕 1
        new ArrayList { true, true, true }, // 屏幕 2
        new ArrayList { true, false, true }, // 屏幕 3
    };

    private int dramaPerformIndex = 2; // 当前在那一幕的下表
    private readonly int[] dramaPerformArrayList = { 0, 1, 2, 3 };  // 样式, 这个应该是一个表格记录了所有的样式
    private const int dramaPerfromScreenNum = 4;
    private const float longPressThreshold = 0.5f; // 长按阈值（单位：秒）

    /* 一些默认参数 */

    // 默认手机像素 
    private readonly Vector2 referenceScreenPixel = new(1920, 1080);

    // 剧情演出系统:
    private CanvasLoader dramaPerformCanvasLoader = null;

    // 大背景: y 方向上肯定会填满, 左右两边可能会空出来(1080x1920)
    private ImageLoader dramaPerformBackgroundLoader = null;
    private const string dramaPerformBackgroundName = @"DramaPerformBackground";
    private readonly Vector2 referenceDramaPerformBackground = new(1920, 1080);

    // 第一个对话框(也可以是第三个, 关键是是否有名片)(360x1920)
    private DialogBoxLoader dialogBoxLoader_01 = null;
    private const string dialogBoxImageName_01 = @"DialogBackground_01";
    private readonly Vector2 dialogBoxPos_01 = new(0.5f, 0.3f);
    private readonly Vector2 referenceDialogBoxPixel_01 = new(1920, 360);
    // 第一个对话框中的文本
    private readonly float dialogTextScale_01 = 0.9f;
    private readonly int dialogTextSize_01 = 16;
    private readonly string dialogTextName_01 = @"DialogText_01";
    private readonly string dialogTextContent_01 = "undefined";
    private readonly Color dialogTextColor_01 = Color.black;
    private readonly TextAlignmentOptions dialogTextAlignment_01 = TextAlignmentOptions.Left;
    // 名片
    private readonly string nameCardName = @"NameCard";
    private ImageLoader nameCardImageLoader = null;
    private readonly Vector2 nameCardPos = new(0.15f, 1f);
    private readonly Vector2 referenceNameCard = new(480, 120);

    // 第二个对话框(90x720): 宽度是父类的 0.375;
    private readonly string normalDialogImageName_02 = @"DialogBackground_02";
    private const string pressedDialogImageName_02 = @"HangingDialogText_02";
    private ButtonLoader dialogBackgroundImageLoader_02 = null;
    private readonly Vector2 dialogBoxPos_02 = new(0.5f, 0.7f);
    private readonly Vector2 referenceDialogBoxPixel_02 = new(720, 90);
    // 第二个对话框的内容
    private readonly string dialogTextName_02 = @"DialogText_02";
    private readonly float dialogTextScale_02 = 0.9f;
    private readonly int dialogTextSize_02 = 16;
    private readonly string dialogTextContent_02 = "undefined";
    private Color dialogTextColor_02 = Color.white;
    private readonly TextAlignmentOptions dialogTextAlignment_02 = TextAlignmentOptions.Left;

    // 人物立绘(1080x360): 
    public readonly string characterImageName = @"CharacterImage";
    private ImageLoader characterImageLoader = null;
    private readonly Vector2 characterImagePos = new(0.5f, 0.5f);
    private readonly Vector2 referenceCharacterImagePixel = new(360, 1080);

    // 回顾剧情按钮(90x270)
    private ButtonLoader reviewButtonLoader = null;
    private const string reviewButtonName = @"ReviewButton";
    private const string hangingReviewImageName = @"HangingReviewButton";
    private readonly Vector2 reviewButtonPos = new(0.1f, 0.85f);
    private readonly Vector2 referenceReviewButton = new(270, 90);
    // 回顾按钮文本
    private string reviewButtonTextName = "ReviewButtonText";
    private readonly float reviewButtonTextScale = 0.9f;
    private readonly int reviewButtonFontSize = 16;
    private readonly string reviewButtonTextContent = "undefined";
    private Color reviewButtonColor = Color.black;
    private const TextAlignmentOptions reviewButtonTextAlignment = TextAlignmentOptions.Center;

    // 回顾界面
    private string reviewBackgroundImageName = @"ReviewBackground";
    private ImageLoader reviewBackgroundImageLoader = null;
    private Vector2 referenceReviewBackground = new(1920, 1080);

    // 返回剧情按钮(90x270)
    private const string returnButtonName = @"ReturnButton";
    private string hangingReturnImageName = @"HangingReturnButton";
    private ButtonLoader returnButtonLoader = null;
    private readonly Vector2 returnButtonPos = new(0.9f, 0.85f);
    private readonly Vector2 referenceReturnButton = new(270, 90);
    //返回按钮文本
    private ScrollbarLoader scrollbarLoader = null;
    private const string returnButtonTextName = "ReturnButtonText";
    private readonly int returnButtonFontSize = 36;
    private readonly string returnButtonTextContent = "undefined";
    private Color returnTextColor = Color.black;
    private readonly float returnButtonTextScale = 0.9f;
    private readonly TextAlignmentOptions returnButtonTextAlignment = TextAlignmentOptions.Center;

    // 剧情回顾系统文字部分:
    // mask:(810x1440) 
    private const string reviewTextMaskName = @"ReviewTextMask";
    private readonly Vector2 reviewTextMaskPos = new(0.45f, 0.5f);
    private readonly Vector2 referenceReviewTextMask = new(1440, 810);
    // 文本
    private const string reviewTextName = @"TeviewText";
    private readonly int reviewTextFontSize = 36;
    private string reviewTextContent = "undefined";
    private Color reviewTextColor = Color.black;
    private const float reviewTextScale = 0.9f;
    private readonly TextAlignmentOptions reviewButtonTextAlignmentOptions = TextAlignmentOptions.Center;
    // 结束标志(72x1440)
    private const string endBarName = @"EndBar";
    private Vector2 referenceEndbar = new(1440, 72);
    private Color handleColor = Color.white;
    private readonly Vector2 handlePos = new(0.85f, 0.45f);
    private readonly Vector2 referenceSilidingArea = new(20, 640);
    private readonly TextAlignmentOptions reviewTextAlignment = TextAlignmentOptions.Center;
    #endregion

    #region methods
    /* 函数方法 */
    public void Start()
    {
        // 初始化各种各样的东西
        font = Resources.Load<TMP_FontAsset>(@"Fonts\my_font");
        if (font == null)
        {
            Debug.Log("fail to load the font!");
        }

        dramaPerformCanvasLoader = new("DramaPerformCanvas", referenceScreenPixel);
        dramaPerformCanvas = dramaPerformCanvasLoader.Canvas;

        // 加载背景, 这里必须或填满
        dramaPerformBackgroundLoader = new(folderPath, dramaPerformBackgroundName, dramaPerformCanvas.transform, referenceDramaPerformBackground);

        // 加载人物立绘
        characterImageLoader = new(
            folderPath, characterImageName, dramaPerformBackgroundLoader.GameObject.transform, referenceCharacterImagePixel,
            referenceCharacterImagePixel.y / referenceDramaPerformBackground.y, true, characterImagePos);

        // 加载一号文本框
        dialogBoxLoader_01 = new(
            folderPath, dialogBoxImageName_01, dialogTextName_01, dramaPerformBackgroundLoader.GameObject.transform, referenceDialogBoxPixel_01,
            dialogTextContent_01, dialogTextSize_01, dialogTextColor_01, dialogTextAlignment_01, font, dialogTextScale_01, true, 
            referenceDialogBoxPixel_01.y / referenceDramaPerformBackground.y, true, dialogBoxPos_01);

        // 加载可能的名片
        nameCardImageLoader = new(
            folderPath, nameCardName, dialogBoxLoader_01.GameObject.transform, referenceNameCard,
            referenceNameCard.y / referenceDramaPerformBackground.y, true, nameCardPos);

        // 加载二号文本框
        dialogBackgroundImageLoader_02 = new(
            folderPath, normalDialogImageName_02, pressedDialogImageName_02, dialogTextName_02, 
            dramaPerformBackgroundLoader.GameObject.transform, referenceDialogBoxPixel_02,
            dialogTextContent_02, dialogTextSize_02, dialogTextColor_02, dialogTextAlignment_02, font, dialogTextScale_02, false,
            referenceDialogBoxPixel_02.y / referenceDramaPerformBackground.y, true, dialogBoxPos_02, null, longPressThreshold, OnDialogBoxButtonClick, OnDialogBoxButtonLongPressed);

        // 回顾界面
        reviewBackgroundImageLoader = new(
            folderPath, reviewBackgroundImageName, dramaPerformBackgroundLoader.GameObject.transform, referenceReviewBackground,
            referenceReviewBackground.y / referenceDramaPerformBackground.y, true);

        // 加载返回按钮
        returnButtonLoader = new(
            folderPath, returnButtonName, hangingReturnImageName, returnButtonName, 
            reviewBackgroundImageLoader.GameObject.transform, referenceReturnButton,
            returnButtonTextContent, returnButtonFontSize, returnTextColor, returnButtonTextAlignment, font, returnButtonTextScale, true, 
            referenceReturnButton.y / referenceDramaPerformBackground.y, true, returnButtonPos, null, longPressThreshold, OnReturnButtonClick, OnReturnButtonLongPressed);
        
        // 第三个按钮: 回顾按钮
        reviewButtonLoader = new(
            folderPath, reviewButtonName, hangingReviewImageName, reviewButtonName, 
            dramaPerformBackgroundLoader.GameObject.transform, referenceReviewButton, 
            reviewButtonTextContent, reviewButtonFontSize, reviewTextColor, reviewButtonTextAlignmentOptions, font, reviewButtonTextScale, true, 
            referenceReviewButton.y / referenceDramaPerformBackground.y, true, reviewButtonPos, null, longPressThreshold, OnReviewButtonClick, OnReviewButtonLongPressed);

        // 剧情回顾系统, 带有滚轮
        scrollbarLoader = new(folderPath, reviewTextMaskName, reviewTextName, endBarName, reviewBackgroundImageLoader.GameObject.transform,
            referenceReviewTextMask, referenceEndbar, referenceSilidingArea, handleColor, reviewTextContent, reviewTextFontSize, reviewTextColor, TextAlignmentOptions.Midline, font,
            referenceReviewTextMask.x / referenceReviewBackground.x, reviewTextScale, false, reviewTextMaskPos, handlePos, new Vector2(0.5f, 1));
    }

    public void Update()
    {
        UpdateScreen(dramaPerformArrayList[dramaPerformIndex]);
    }

    private void UpdateScreen(int screenIndex)
    {
        // 获取指定屏幕的渲染状态
        ArrayList dramaPerformScreen = (ArrayList)dramaScreenArrayList[screenIndex];

        // 根据渲染状态设置 UI 元素的显示
        characterImageLoader.SetActive((bool)dramaPerformScreen[0] & isDramaPerformSystem);
        nameCardImageLoader.SetActive((bool)dramaPerformScreen[1] & isDramaPerformSystem);
        dialogBackgroundImageLoader_02.SetActive((bool)dramaPerformScreen[2] & isDramaPerformSystem);
        dialogBoxLoader_01.SetActive(isDramaPerformSystem);
        reviewButtonLoader.SetActive(isDramaPerformSystem);
        returnButtonLoader.SetActive(!isDramaPerformSystem);
        reviewBackgroundImageLoader.SetActive(!isDramaPerformSystem);
    }

    private void OnReturnButtonClick()
    {
        if (isDramaPerformSystem == false)
        {
            isDramaPerformSystem = true;
            Debug.Log("Return button clicked");
        }
    }

    private void OnReturnButtonLongPressed()
    {
        Debug.Log($"return button long pressed detected");
    }

    private void OnReviewButtonClick()
    {
        if (isDramaPerformSystem == true)
        {
            isDramaPerformSystem = false;
            Debug.Log("Review button clicked"); ;
        }
    }

    private void OnReviewButtonLongPressed()
    {
        Debug.Log($"review bug long pressed detected");
    }

    private void OnDialogBoxButtonClick()
    {
        // 对话框点击事件
        Debug.Log("Dialog box clicked");
    }

    private void OnDialogBoxButtonLongPressed()
    {
        // 对话框点击事件
        Debug.Log("Dialog box long pressed");
    }
    #endregion
}