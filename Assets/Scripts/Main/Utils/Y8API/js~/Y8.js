/*
y8_global is a global js object.

- Calling the GameAPI directly from library does not work, even with delay (iframe is downloaded, but without additional data)
- Calling the GameAPI without delay also does not work (perhaps it needs to wait for another frame?)
https://forum.unity.com/threads/tiny-3rd-party-api-requests-iframes-bug.819057/

*/


var Y8lib = {
    $ExternalAPI: {},
    $y8_global: {
        isLoggedIn: false,
        isInitialized: false,
        loginCallback: function (response) {
            if (response) {
                console.log(response);
                if (response.status == "ok") this.isLoggedIn = true;
            }
        },
        delay: async function() {
            return new Promise(resolve => setTimeout(resolve, 300));
        },
        ShowHighscore: async function(tableId){
            await this.delay();
            ID.GameAPI.Leaderboards.list({table: tableId}); 
        },

        SaveHighscore: async function (tableId, score) {
            await this.delay();
            ID.GameAPI.Leaderboards.save(
                {
                    table: tableId,
                    points: score
                },
                (response)=> {console.log(response)});

        }

    },

    Init: function(appId_p){
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
                y8_global.isInitialized = true;

                ID.getLoginStatus(function(data) { // Try Autologin
                    console.log(data);
                    if (data.status == 'not_linked') ID.login(y8_global.LoginCallback);
                    if (data.status == 'ok'){                        
                        y8_global.isLoggedIn = true;

                    }    
                });
            });

            ID.init({
                appId : appId
              });
        }
    },

    Login: function() {
        ID.login(y8_global.LoginCallback);
    },

    IsLoggedIn: function() {
        return y8_global.isLoggedIn;
    },

    ShowHighscore: function(tableId_p) {
        var tableId = UTF8ToString(tableId_p);
        y8_global.ShowHighscore(tableId);
    },

    ProvideCallback: function(obj)
    {
        console.log("ProvideCallback");
        console.log(obj);
        ExternalAPI.callback = obj;
        dynCall_v(obj);
        //dynCall('v', obj, 0);
    },

    SaveHighscore: async function(tableId_p, score) {
        var tableId = UTF8ToString(tableId_p);
        y8_global.SaveHighscore(tableId, score);
    }

}

autoAddDeps(Y8lib, '$ExternalAPI');
autoAddDeps(Y8lib, '$y8_global');
mergeInto(LibraryManager.library, Y8lib);

