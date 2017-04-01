document.write("<script type='text/javascript' src='/Scripts/JQuery3-1-1.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/bootstrap-3.3.7-dist/js/bootstrap.min.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/AugularJs1.4.6.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/Cookie.js'></script>");

var Angular = function (func, appName, factoryName, controllerName) {
    var app = angular.module(appName ? appName : 'app', []);
    app.factory(factoryName ? factoryName : 'factory', function ($http, $sce, $q) {
        var service = {};

        service.Query = function (apiUrl) {  //网络请求
            var cookie = getCookie(apiUrl, true);
            var defer = $q.defer();
            if (cookie == null) {
                $http.get(apiUrl).success(function (response) {
                    setCookie(apiUrl, response, true);
                    defer.resolve(response);
                });
            } else {
                defer.resolve(cookie);
            }
            return defer.promise;
        };

        service.Html = function (text) {
            return $sce.trustAsHtml(text);
        }

        service.IsNull = function (text) {
            return text == null || text == undefined || text == '';
        }

        service.Enter = function (event, id) {  //Enter绑定
            if (event.keyCode == 13) {
                document.getElementById(id).click();
            }
        }
        return service;
    });
    app.controller(controllerName ? controllerName : 'controller', func);
    return app;
}