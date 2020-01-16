using System.Runtime.InteropServices;

public class LocalStorage
{
    [DllImport("__Internal")]
    public static extern string GetLocalStorageItem(string key);

    [DllImport("__Internal")]
    public static extern void SetLocalStorageItem(string key, string value);
}