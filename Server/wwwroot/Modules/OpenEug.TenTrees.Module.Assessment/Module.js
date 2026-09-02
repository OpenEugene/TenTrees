var OpenEug = OpenEug || {};
OpenEug.TenTrees = OpenEug.TenTrees || {};
OpenEug.TenTrees.Assessment = OpenEug.TenTrees.Assessment || {};
OpenEug.TenTrees.Assessment.openPhotoPicker = function (elementId) {
    "use strict";

    var input = document.getElementById(elementId);
    if (input && !input.disabled) {
        input.click();
    }
};
OpenEug.TenTrees.Assessment.PhotoDrafts = (function () {
    "use strict";

    var databaseName = "TenTreesAssessmentDrafts";
    var storeName = "photoDrafts";

    function openDatabase() {
        return new Promise(function (resolve, reject) {
            var request = indexedDB.open(databaseName, 1);

            request.onupgradeneeded = function () {
                var database = request.result;
                if (!database.objectStoreNames.contains(storeName)) {
                    database.createObjectStore(storeName, { keyPath: "key" });
                }
            };

            request.onsuccess = function () {
                resolve(request.result);
            };

            request.onerror = function () {
                reject(request.error);
            };
        });
    }

    async function save(key, photos) {
        var database = await openDatabase();
        try {
            await new Promise(function (resolve, reject) {
                var transaction = database.transaction(storeName, "readwrite");
                transaction.objectStore(storeName).put({ key: key, photos: photos || [] });
                transaction.oncomplete = resolve;
                transaction.onerror = function () { reject(transaction.error); };
                transaction.onabort = function () { reject(transaction.error); };
            });
        } finally {
            database.close();
        }
    }

    async function load(key) {
        var database = await openDatabase();
        try {
            return await new Promise(function (resolve, reject) {
                var transaction = database.transaction(storeName, "readonly");
                var request = transaction.objectStore(storeName).get(key);
                request.onsuccess = function () {
                    resolve(request.result ? request.result.photos : []);
                };
                request.onerror = function () { reject(request.error); };
            });
        } finally {
            database.close();
        }
    }

    async function clear(key) {
        var database = await openDatabase();
        try {
            await new Promise(function (resolve, reject) {
                var transaction = database.transaction(storeName, "readwrite");
                transaction.objectStore(storeName).delete(key);
                transaction.oncomplete = resolve;
                transaction.onerror = function () { reject(transaction.error); };
                transaction.onabort = function () { reject(transaction.error); };
            });
        } finally {
            database.close();
        }
    }

    return {
        save: save,
        load: load,
        clear: clear
    };
})();
