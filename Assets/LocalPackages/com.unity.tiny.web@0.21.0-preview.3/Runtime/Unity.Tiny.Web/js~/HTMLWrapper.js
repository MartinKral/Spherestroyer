mergeInto(LibraryManager.library, {
  // helper function for strings 
  $utf16_to_js_string: function(ptr) {
    var str = '';
    ptr >>= 1;
    while (1) {
      var codeUnit = HEAP16[ptr++];
      if (!codeUnit) return str;
      str += String.fromCharCode(codeUnit);
    }
  },

  // Create a variable 'ut' in global scope so that Closure sees it.
  js_html_init__postset : 'var ut;',
  js_html_init__proxy : 'async',
  js_html_init : function() {
    ut = ut || {};
    ut._HTML = ut._HTML || {};

    var html = ut._HTML;
    html.visible = true;
    html.focused = true;
  },

  js_html_getDPIScale__proxy : 'sync',
  js_html_getDPIScale : function () {
    return window.devicePixelRatio;
  },

  js_html_getScreenSize__proxy : 'sync',
  js_html_getScreenSize : function (wPtr, hPtr) {
    HEAP32[wPtr>>2] = screen.width | 0;
    HEAP32[hPtr>>2] = screen.height | 0;
  },
  
  js_html_getFrameSize__proxy : 'sync',
  js_html_getFrameSize : function (wPtr, hPtr) {
    HEAP32[wPtr>>2] = window.innerWidth | 0;
    HEAP32[hPtr>>2] = window.innerHeight | 0;
  },  
  
  js_html_getCanvasSize__proxy : 'sync',
  js_html_getCanvasSize : function (wPtr, hPtr) {
    var html = ut._HTML;
    HEAP32[wPtr>>2] = html.canvasElement.width | 0;
    HEAP32[hPtr>>2] = html.canvasElement.height | 0;
  },

  js_html_setCanvasSize__proxy : 'sync',
  js_html_setCanvasSize : function(width, height) {
    if (!width>0 || !height>0)
        throw "Bad canvas size at init.";
    var canvas = ut._HTML.canvasElement;
    if (!canvas) {
      // take possible user element
      canvas = document.getElementById("UT_CANVAS");
      if (canvas)
        console.log('Using user UT_CANVAS element.');
    } 
    if (!canvas) {
      canvas = document.createElement("canvas");
      canvas.setAttribute("id", "UT_CANVAS");
      canvas.setAttribute("style", "touch-action: none;");
      canvas.setAttribute("tabindex", "1");
      if (document.body) {
        document.body.style.margin = "0px";
        document.body.style.border = "0";
        document.body.style.overflow = "hidden"; // disable scrollbars
        document.body.style.display = "block";   // no floating content on sides
        document.body.insertBefore(canvas, document.body.firstChild);
      } else {
        document.documentElement.appendChild(canvas);
      }
    }

    ut._HTML.canvasElement = canvas;

    canvas.width = width;
    canvas.height = height;

    ut._HTML.canvasMode = 'bgfx';

    canvas.addEventListener("webglcontextlost", function(event) { event.preventDefault(); }, false);
    window.addEventListener("focus", function(event) { ut._HTML.focus = true; } );
    window.addEventListener("blur", function(event) { ut._HTML.focus = false; } );

    canvas.focus();
    return true;
  },

  js_html_debugReadback__proxy : 'sync',
  js_html_debugReadback : function(w, h, pixels) {
    if (!ut._HTML.canvasContext || ut._HTML.canvasElement.width<w || ut._HTML.canvasElement.height<h)
      return;
    var gl = ut._HTML.canvasContext;
    var imd = new Uint8Array(w*h*4);
    gl.readPixels(0, 0, w, h, gl.RGBA, gl.UNSIGNED_BYTE, imd); 
    for (var i=0; i<w*h*4; i++)
      HEAPU8[pixels+i] = imd[i];
  },

  js_html_promptText__proxy : 'sync',
  js_html_promptText : function(message, defaultText) {
    var res =
        prompt(UTF8ToString(message), UTF8ToString(defaultText));
    var bufferSize = lengthBytesUTF8(res) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(res, buffer, bufferSize);
    return buffer;
  },
});
