using System.Runtime.InteropServices;

public class JsInteractor
{
    [DllImport("__Internal")]
    public static extern void OpenURL(string url);

    [DllImport("__Internal")]
    public static extern void Hello();
}