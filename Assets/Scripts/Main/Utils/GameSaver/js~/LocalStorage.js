mergeInto(LibraryManager.library, {
    GetLocalStorageItem: function (key) { 
        var itemValue = localStorage.getItem(UTF8ToString(key));   
        if (itemValue == null) itemValue = "";             
        var bufferSize = lengthBytesUTF8(itemValue) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(itemValue, buffer, bufferSize);
        return buffer;
      },

    SetLocalStorageItem: function (key, value) {
        localStorage.setItem(UTF8ToString(key), UTF8ToString(value));
    }
});
