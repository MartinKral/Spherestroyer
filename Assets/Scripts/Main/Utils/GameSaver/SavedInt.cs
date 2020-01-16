public class SavedInt
{
    private readonly string id;
    private int value;

    public int Value
    {
        get => value;
        set
        {
            this.value = value;
            Save();
        }
    }

    public SavedInt(string id)
    {
        this.id = id;

        string stringValue = LocalStorage.GetLocalStorageItem(id);
        if (stringValue == "") return;
        if (stringValue == null) return;

        Value = stringValue.ToInt();
    }

    public static implicit operator int(SavedInt savedInt) => savedInt.value;

    private void Save()
    {
        LocalStorage.SetLocalStorageItem(id, Value.ToStringExtension());
    }
}