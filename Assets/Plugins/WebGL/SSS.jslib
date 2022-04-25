var SSS = {
    getActivePublicKey: function()
    {
        if(!window.SSS) {
            console.log("SSS is not instaled");
            return;
        }
        var returnStr = window.SSS.activePublicKey;
        var buffer = _malloc(lengthBytesUTF8(returnStr) + 1);
        stringToUTF8(returnStr, buffer, returnStr.length + 1);
        return buffer;
    },
    getActiveAddress: function()
    {
        if(!window.SSS) {
            console.log("SSS is not instaled");
            return;
        }
        var returnStr = window.SSS.activeAddress;
        var buffer = _malloc(lengthBytesUTF8(returnStr) + 1);
        stringToUTF8(returnStr, buffer, returnStr.length + 1);
        return buffer;
    },
    getActiveNetworkType: function()
    {
        if(!window.SSS) {
            console.log("SSS is not instaled");
            return;
        }
        return window.SSS.activeNetworkType;
    },
    announceTransaction: function(methodName, parameter) {
        methodName = UTF8ToString(methodName)
        parameter = UTF8ToString(parameter)

        var jsonObj = {}
        jsonObj.methodName = methodName
        jsonObj.parameter = parameter

        var argsmentString = JSON.stringify(jsonObj)
        var event = new CustomEvent('unityMessage', { detail: argsmentString })
        window.dispatchEvent(event)
    }
};

mergeInto(LibraryManager.library, SSS);