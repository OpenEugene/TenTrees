/* Grower module browser integrations. */
var OpenEug = OpenEug || {};

OpenEug.TenTrees = OpenEug.TenTrees || {};

OpenEug.TenTrees.Grower = {
    captureLocation: function () {
        return new Promise(function (resolve) {
            if (!navigator.geolocation) {
                resolve({ success: false, errorCode: "unsupported" });
                return;
            }

            navigator.geolocation.getCurrentPosition(
                function (position) {
                    resolve({
                        success: true,
                        latitude: position.coords.latitude,
                        longitude: position.coords.longitude,
                        accuracy: position.coords.accuracy
                    });
                },
                function (error) {
                    var errorCode = "unknown";
                    if (error.code === error.PERMISSION_DENIED) errorCode = "permission-denied";
                    if (error.code === error.POSITION_UNAVAILABLE) errorCode = "unavailable";
                    if (error.code === error.TIMEOUT) errorCode = "timeout";
                    resolve({ success: false, errorCode: errorCode });
                },
                {
                    enableHighAccuracy: true,
                    timeout: 20000,
                    maximumAge: 0
                });
        });
    }
};
