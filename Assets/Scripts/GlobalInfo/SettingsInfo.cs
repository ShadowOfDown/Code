using System.Collections.Generic;

public static class SettingsInfo
{
  public static float longPressThreshold = 0.5f;
  public readonly static int maxPlayerCount = 8;
  public static List<string> playerNameList = new()
  {
    @"name_01",
    @"name_02",
    @"name_03",
    @"name_04",
    @"name_05",
    @"name_06",
    @"name_07",
    @"name_08",
  };
  public static List<string> roleNameList = new()
  {
    @"role_01",
    @"role_02",
    @"role_03",
    @"role_04",
    @"role_05",
    @"role_06",
    @"role_07",
    @"role_08",
  };
  public static List<string> roleIntroductionList = new()
  {
    @"introduction_01",
    @"introduction_02",
    @"introduction_03",
    @"introduction_04",
    @"introduction_05",
    @"introduction_06",
    @"introduction_07",
    @"introduction_08",
  };
}