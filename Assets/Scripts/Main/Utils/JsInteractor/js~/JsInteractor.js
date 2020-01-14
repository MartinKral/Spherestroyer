mergeInto(LibraryManager.library, {
    OpenURL: function (url) {
        
        url = UTF8ToString(url);  
        var gameCanvas = document.getElementById("UT_CANVAS");   
        if (gameCanvas != null)  {
            gameCanvas.onmouseup = function()
            {    
                console.log("On mouse up from Tiny")            
                window.open(url, "_blank");
                gameCanvas.onmouseup = null;                
            }
        } else {
            console.error("UT_CANVAS not found, was it renamed?");
        }
      },

      Hello: function(){
          console.log("Hello world!");
      }
});
