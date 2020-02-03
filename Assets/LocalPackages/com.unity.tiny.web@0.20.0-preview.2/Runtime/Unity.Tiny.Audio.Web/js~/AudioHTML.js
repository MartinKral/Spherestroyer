mergeInto(LibraryManager.library, {

    js_html_initAudio : function() {
        
        ut = ut || {};
        ut._HTML = ut._HTML || {};

        ut._HTML.unlock = function() {
        // call this method on touch start to create and play a buffer, then check
        // if the audio actually played to determine if audio has now been
        // unlocked on iOS, Android, etc.
            if (!self.audioContext)
                return;

            // fix Android can not play in suspend state
            self.audioContext.resume();

            // create an empty buffer
            var source = self.audioContext.createBufferSource();
            source.buffer = self.audioContext.createBuffer(1, 1, 22050);
            source.connect(self.audioContext.destination);

            // play the empty buffer
            if (typeof source.start === 'undefined') {
                source.noteOn(0);
            } else {
                source.start(0);
            }

            // calling resume() on a stack initiated by user gesture is what
            // actually unlocks the audio on Android Chrome >= 55
            self.audioContext.resume();

            // setup a timeout to check that we are unlocked on the next event
            // loop
            source.onended = function () {
                source.disconnect(0);

                // update the unlocked state and prevent this check from happening
                // again
                self.unlocked = true;
                //console.log("[Audio] unlocked");

                // remove the touch start listener
                document.removeEventListener('click', ut._HTML.unlock, true);
                document.removeEventListener('touchstart', ut._HTML.unlock, true);
                document.removeEventListener('touchend', ut._HTML.unlock, true);
                document.removeEventListener('keydown', ut._HTML.unlock, true);
                document.removeEventListener('keyup', ut._HTML.unlock, true);
            };
        };

        // audio initialization
        if (!window.AudioContext && !window.webkitAudioContext)
            return false;

        var audioContext =
            new (window.AudioContext || window.webkitAudioContext)();
        if (!audioContext)
            return false;

        this.audioContext = audioContext;
        this.audioBuffers = {};
        this.audioSources = {};

        // try to unlock audio
        this.unlocked = false;
        var navigator = (typeof window !== 'undefined' && window.navigator)
            ? window.navigator
            : null;
        var isMobile = /iPhone|iPad|iPod|Android|BlackBerry|BB10|Silk|Mobi/i.test(
            navigator && navigator.userAgent);
        var isTouch = !!(isMobile ||
            (navigator && navigator.maxTouchPoints > 0) ||
            (navigator && navigator.msMaxTouchPoints > 0));
        if (this.audioContext.state !== 'running' || isMobile || isTouch) {
            ut._HTML.unlock();
        } else {
            this.unlocked = true;
        }
        //console.log("[Audio] initialized " + (this.unlocked ? "unlocked" : "locked"));
        return true;
    },

    js_html_audioIsUnlocked : function() {
        return this.unlocked;
    },

    // unlock audio for browsers
    js_html_audioUnlock : function () {
        var self = this;
        if (self.unlocked || !self.audioContext ||
            typeof self.audioContext.resume !== 'function')
            return;

        // setup a touch start listener to attempt an unlock in
        document.addEventListener('click', ut._HTML.unlock, true);
        document.addEventListener('touchstart', ut._HTML.unlock, true);
        document.addEventListener('touchend', ut._HTML.unlock, true);
        document.addEventListener('keydown', ut._HTML.unlock, true);
        document.addEventListener('keyup', ut._HTML.unlock, true);
    },

    // pause audio context
    js_html_audioPause : function () {
        if (!this.audioContext)
            return;

        this.audioContext.suspend();
    },

    // resume audio context
    js_html_audioResume : function () {
        if (!this.audioContext || typeof this.audioContext.resume !== 'function')
            return;

        this.audioContext.resume();
    },

    // load audio clip
    js_html_audioStartLoadFile : function (audioClipName, audioClipIdx) 
    {
        if (!this.audioContext || audioClipIdx < 0)
            return -1;

        audioClipName = UTF8ToString(audioClipName);

        var url = audioClipName;
        if (url.substring(0, 9) === "ut-asset:")
            url = UT_ASSETS[url.substring(9)];

        var self = this;
        var request = new XMLHttpRequest();

        self.audioBuffers[audioClipIdx] = 'loading';
        request.open('GET', url, true);
        request.responseType = 'arraybuffer';
        request.onload =
            function () {
                self.audioContext.decodeAudioData(request.response, function (buffer) {
                    self.audioBuffers[audioClipIdx] = buffer;
                });
            };
        request.onerror =
            function () {
                self.audioBuffers[audioClipIdx] = 'error';
            };
        try {
            request.send();
            //Module._AudioService_AudioClip_OnLoading(entity,audioClipIdx);
        } catch (e) {
            // LG Nexus 5 + Android OS 4.4.0 + Google Chrome 30.0.1599.105 browser
            // odd behavior: If loading from base64-encoded data URI and the
            // format is unsupported, request.send() will immediately throw and
            // not raise the failure at .onerror() handler. Therefore catch
            // failures also eagerly from .send() above.
            self.audioBuffers[audioClipIdx] = 'error';
        }

        return audioClipIdx;
    },

    /*public enum LoadResult
    {
        stillWorking = 0,
        success = 1,
        failed = 2
    };
    */
    js_html_audioCheckLoad : function (audioClipIdx) {
        var WORKING_ON_IT = 0;
        var SUCCESS = 1;
        var FAILED = 2;

        if (!this.audioContext || audioClipIdx < 0)
            return FAILED;
        if (this.audioBuffers[audioClipIdx] == null)
            return FAILED;
        if (this.audioBuffers[audioClipIdx] === 'loading')
            return WORKING_ON_IT; 
        if (this.audioBuffers[audioClipIdx] === 'error')
            return FAILED;
        return SUCCESS;
    },

    js_html_audioFree : function (audioClipIdx) {
        var audioBuffer = this.audioBuffers[audioClipIdx];
        if (!audioBuffer)
            return;

        for (var i = 0; i < this.audioSources.length; ++i) {
            var sourceNode = this.audioSources[i];
            if (sourceNode && sourceNode.buffer === audioBuffer)
                sourceNode.stop();
        }

        this.audioBuffers[audioClipIdx] = null;
    },

    // create audio source node
    js_html_audioPlay : function (audioClipIdx, audioSourceIdx, volume, loop) 
    {
        if (!this.audioContext || audioClipIdx < 0 || audioSourceIdx < 0)
            return false;

        if (this.audioContext.state !== 'running')
            return false;

        // require audio buffer to be loaded
        var srcBuffer = this.audioBuffers[audioClipIdx];
        if (!srcBuffer || typeof srcBuffer === 'string')
            return false;

        // create audio source node
        var sourceNode = this.audioContext.createBufferSource();
        sourceNode.buffer = srcBuffer;

        // create gain node if needed
        if (volume !== 1.0) {
            var gainNode = this.audioContext.createGain();
            gainNode.gain.setValueAtTime(volume, 0);
            sourceNode.connect(gainNode);
            gainNode.connect(this.audioContext.destination);
        } else {
            sourceNode.connect(this.audioContext.destination);
        }

        // loop value
        sourceNode.loop = loop;

        
       
        if (this.audioSources[audioSourceIdx] === undefined){
            // store audio source node
            this.audioSources[audioSourceIdx] = sourceNode;            
            
        } else {
            // stop audio source node if it is already playing
            this.audioSources[audioSourceIdx].stop();
            
        }        
        
        sourceNode.onended = function (event) {
            sourceNode.stop();
            sourceNode.isPlaying = false;
        };

        // play audio source
        sourceNode.start();
        sourceNode.isPlaying = true;
        //console.log("[Audio] playing " + audioSourceIdx);
        return true;
    },

    // remove audio source node, optionally stop it 
    js_html_audioStop : function (audioSourceIdx, dostop) {
        if (!this.audioContext || audioSourceIdx < 0)
            return;

        // retrieve audio source node
        var sourceNode = this.audioSources[audioSourceIdx];
        if (!sourceNode)
            return;

        // forget audio source node
        sourceNode.onended = null;
        this.audioSources[audioSourceIdx] = null;

        // stop audio source
        if (dostop) {
            sourceNode.stop();
            sourceNode.isPlaying = false;
            //console.log("[Audio] stopping " + audioSourceIdx);
        }
    },

    js_html_audioIsPlaying : function (audioSourceIdx) {
        if (!this.audioContext || audioSourceIdx < 0)
            return false;

        if (this.audioSources[audioSourceIdx] == null)
            return false;

        return this.audioSources[audioSourceIdx].isPlaying;
    }
});
