using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TxtColor
{
    Reg,
    Green,
    Blue,
    Yellow
}
public class Constants
{
    private const string ColorRed = "<color=#FF0000FF>";
    private const string ColorGreen = "<color=#00FF00FF>";
    private const string ColorBlue = "<color=#00B4FFFF>";
    private const string ColorYellow = "<color=#FFFF00FF>";
    private const string ColorEnd = "</color>";

    public static string Color(string str, TxtColor color)
    {
        string result = "";
        switch (color)
        {
            case TxtColor.Reg:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
        }

        return result;
    }
    
    
    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;
    public const int ScreenOPDis = 90;

    public const int PlayerMoveSpeed = 8;
    public const int MonsterMoveSpeed = 4;
    public const int AccelerSpeed = 5;
    public const int BlendIdle = 0;
    public const int BlendWalk = 1;
    
    public const string SceneLogin = "SceneLogin";
    public const int MainCityMapID = 10000;
    //public const string SceneMainCity = "SceneMainCity";

    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;
    
    
    public const string BGLogin = "bgLogin";
    public const string UILoginBtn = "uiLoginBtn";
    public const string UIClickBtn = "uiClickBtn";
    public const string BGMainCity = "bgMainCity";
    public const string UIExtenBtn = "uiExtenBtn";
    public static string UIOpenPage = "uiOpenPage";
}
