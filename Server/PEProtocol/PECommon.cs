using PENet;

namespace PEProtocol
{
    public enum LogType
    {
        Log=0,
        Warm=1,
        Error=2,
        Info=3
    }
    
    public class PECommon
    {
        public static void Log(string msg="", LogType logType=LogType.Log)
        {
            LogLevel lv = (LogLevel) logType;
            PETool.LogMsg(msg,lv);
        }
    }
}