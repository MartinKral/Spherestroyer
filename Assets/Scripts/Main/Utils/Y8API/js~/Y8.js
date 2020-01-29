var Y8lib = {
    $y8Private: {
        Context: null,
        ID: {},
        isLoggedIn: false,
        loginCallback: function (response) {
            if (response) {
                console.log(response);
                if (response.status == "ok") this.isLoggedIn = true;
            }
        },
        showLeaderboard: function(){
            ID.GameAPI.Leaderboards.list({table: "Leaderboard"});
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
            y8Private.ID = ID;
            y8Private.Context = this;
            ID.Event.subscribe('id.init', function() { // SDK initialized
                ID.getLoginStatus(function(data) { // Try Autologin
                    console.log(data);
                    if (data.status == 'not_linked') ID.login(y8Private.LoginCallback);
                    if (data.status == 'ok'){                        
                        y8Private.isLoggedIn = true;

                    }    
                });
            });

            ID.init({
                appId : appId
              });
        }
    },

    Login: function() {
        ID.login(y8Private.LoginCallback);
    },

    IsLoggedIn: function() {
        return y8Private.isLoggedIn;
    },

    ShowHighscore: function(tableId_p) {
        var tableId = UTF8ToString(tableId_p);
        
        console.log(" test function ");
        y8Private.showLeaderboard();       
    },

    SaveHighscore: function(tableId_p, score) {
        var tableId = UTF8ToString(tableId_p);

        ID.GameAPI.Leaderboards.save(
            {
                table: tableId,
                points: score
            },
            (response)=> {console.log(response)});
    }


}

autoAddDeps(Y8lib, '$y8Private');
mergeInto(LibraryManager.library, Y8lib);

