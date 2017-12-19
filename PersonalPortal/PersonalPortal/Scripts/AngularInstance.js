document.write("<script type='text/javascript' src='/Scripts/JQuery3-1-1.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/bootstrap-3.3.7-dist/js/bootstrap.min.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/AngularJs1.4.6.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/Cookie.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/Linq.js'></script>");

var Angular = function (func, appName, factoryName, controllerName) {
    $("[data-toggle='tooltip']").tooltip({ trigger: 'hover' });  //激活提示

    var app = angular.module(appName ? appName : 'app', []);
    app.factory(factoryName ? factoryName : 'factory', function ($http, $sce, $q) {
        var service = {};

        service.Query = function (apiUrl, refresh, save) {  //网络请求
            var cookie = getCookie(apiUrl);
            var defer = $q.defer();
            if (cookie == null || refresh) {
                $http.get(apiUrl).success(function (response) {
                    if (save != false) { setCookie(apiUrl, response); }
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
        return service;
    });
    app.controller(controllerName ? controllerName : 'controller', func);
    return app;
}