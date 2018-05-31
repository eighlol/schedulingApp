(function () {
    "use strict";

    angular.module("schedulingApp", ["ngRoute", "ngMaterial", "ngMdIcons", "ngMessages"])
        .config(["$mdIconProvider", "$mdThemingProvider", "$routeProvider", "$locationProvider", function ($mdIconProvider, $mdThemingProvider, $routeProvider, $locationProvider) {
            $mdIconProvider
                .iconSet('menu', '/icons/svg/menu.svg', 24);
            $mdThemingProvider.theme('default')
                .primaryPalette('blue')
                .accentPalette('red');
            $routeProvider
                .when('/', {
                    templateUrl: 'views/pages/events.html',
                    controller: 'mainController'
                })
                .when('/members', {
                    templateUrl: 'views/pages/members.html',
                })
            $locationProvider.html5Mode(true);
        }]);
})();