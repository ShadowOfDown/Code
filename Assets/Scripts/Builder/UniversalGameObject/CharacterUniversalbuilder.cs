// 人物框
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterUniversalbuilder : UniversalGameObjectBuilder
{
  #region Fields
  public static readonly string roleResourcesFolderPath = "Arts/Textures/RoleImage/";
  public static readonly Vector2 characterReferencePixel = new(763.6257f, 1080f);
  private readonly ImageGameObjectBuilder _characterImageBuilder;
  #endregion


  #region Properties
  public override string Type { get; } = "CharacterImage";
  public override string GameObjectName { get; }
  public override Transform Transform { get { return _characterImageBuilder.Transform; } }
  public override RectTransform RectTransform { get { return _characterImageBuilder.RectTransform; } }
  #endregion

  #region Methods
  public CharacterUniversalbuilder(
    string characterName,
    Vector2 characterPos,
    Transform parentTransform)
  {
    _characterImageBuilder = new ImageGameObjectBuilder(characterName, parentTransform);
    _characterImageBuilder.Build(
      new Dictionary<string, IComponentBuilder>()
      {
        {"RectTransform", new RectTransformComponentBuilder(
          new Dictionary<string, Vector2>
          {
            {"referenceObjectPixels", characterReferencePixel},
            {"archorMin", characterPos},
            {"archorMax", characterPos},
            {"pivotPos", new Vector2(0.5f, 0.5f)},
          }
        )},
        {"Image", new ImageComponentBuilder(
          new Dictionary<string, string>
          {
            {"gameObjectName", characterName},
            {"resourcesFolderPath", roleResourcesFolderPath},
          }
        )}
      }
    );
  }

  public override void SetActive(bool state)
  {

  }
  #endregion
}