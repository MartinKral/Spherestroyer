mergeInto(LibraryManager.library, {
    OpenURL: function (url) { 
        url = UTF8ToString(url);  
        var gameCanvas = document.getElementById("UT_CANVAS");  

        if (gameCanvas != null)  {
            var endInteractFunction = function()
            {    
                window.open(url, "_blank");
                gameCanvas.onmouseup = null;  
                gameCanvas.ontouchend = null;              
            }

            gameCanvas.ontouchend = endInteractFunction;
            gameCanvas.onmouseup = endInteractFunction;
        } else {
            console.error("UT_CANVAS not found, was it renamed?");
        }
      }
});
