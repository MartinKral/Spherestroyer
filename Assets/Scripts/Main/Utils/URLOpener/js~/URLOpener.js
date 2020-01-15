mergeInto(LibraryManager.library, {
    OpenURL: function (url) { 
        url = UTF8ToString(url);  
        var gameCanvas = document.getElementById("UT_CANVAS");  

        if (gameCanvas != null)  {
            gameCanvas.onmouseup = function()
            {    
                window.open(url, "_blank");
                gameCanvas.onmouseup = null;                
            }
        } else {
            console.error("UT_CANVAS not found, was it renamed?");
        }
      }
});
