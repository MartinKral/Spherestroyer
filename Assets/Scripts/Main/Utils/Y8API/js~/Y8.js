var Y8lib = {
    $y8_global: {
        Callback: function (callbackType, callback) {            
            var bufferSize = lengthBytesUTF8(callbackType) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(callbackType, buffer, bufferSize);
            dynCall_vi(callback, buffer);
        },
        Login: function(callback) {
            ID.login((response) => {
                if (response) {     
                    console.log(response);           
                    if (response.status == "ok"){
                        y8_global.Callback("login", callback);
                    }
                }
            });
        },
    },

    Init: function(appId_p, callback){
        var appId = UTF8ToString(appId_p);

        var d = document;
        var s = 'script';
        var id = 'id-jssdk';

        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) {return;}
        js = d.createElement(s); js.id = id;
        js.src =  document.location.protocol == 'https:' ? "https://cdn.y8.com/api/sdk.js" : "http://cdn.y8.com/api/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);  
        
        window.idAsyncInit = function(){
            ID.Event.subscribe('id.init', function() { // SDK initialized
                y8_global.Callback("init", callback);

                ID.getLoginStatus(function(data) { // Try Autologin
                    console.log(data);
                    if (data.status == 'not_linked') y8_global.Login(callback);
                    if (data.status == 'ok'){                        
                        y8_global.Callback("login", callback);

                    }    
                });
            });

            ID.init({
                appId : appId
              });
        }
    },


    ShowHighscore: function(tableId_p, callback) {
        var tableId = UTF8ToString(tableId_p);
        ID.GameAPI.Leaderboards.list({table: tableId}); 
    },


    SaveHighscore: function(tableId_p, score, callback) {
        var tableId = UTF8ToString(tableId_p);
        ID.GameAPI.Leaderboards.save(
            {
                table: tableId,
                points: score
            },
            (response)=> {
                console.log(response);
                if (response.success) {
                    y8_global.Callback("score-success", callback);
                } else {
                    y8_global.Callback("score-fail", callback);
                }
            });
    }

}

autoAddDeps(Y8lib, '$y8_global');
mergeInto(LibraryManager.library, Y8lib);

