//Author : _SourceCode
//CreateTime : 2025-03-03-19:21:05
//Version : 1.0
//UnityVersion : 2022.3.53f1c1
   
namespace MyFrame.Event
{
    public delegate void CallBack();
    public delegate void CallBack<T>(T Arg1);
    public delegate void CallBack<T, U>(T Arg1, U Arg2);
    public delegate void CallBack<T, U, V>(T Arg1, U Arg2, V Arg3);
}
