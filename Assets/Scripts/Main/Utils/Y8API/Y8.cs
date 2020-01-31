using System;

public class Y8
{
    private static readonly string appId = "5e22f7f4e694aabc7e4d0ee2";

    public static API Api { get; } = new API(appId);
}