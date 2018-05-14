(function () {
    "use strict";

    angular.module("conferenceApp", ["ngRoute", "ngMaterial", "ngMdIcons", "ngMessages"]);

    angular.module("conferenceApp").config(["$mdIconProvider", "$mdThemingProvider", function ($mdIconProvider, $mdThemingProvider) {
        $mdIconProvider
            .iconSet('menu', '/icons/svg/menu.svg', 24);
        $mdThemingProvider.theme('default')
            .primaryPalette('blue')
            .accentPalette('red');
    }]);
    



    //angular.module("conferenceApp").config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {
    //    $routeProvider
    //        .when('/', {
    //            templateUrl: 'views/partials/events.html',
    //            controller: 'eventController'
    //        })
    //        .when('/events/add', {
    //            templateUrl: 'views/partials/addEvent.html',
    //            controller: 'eventAddController'
    //        })
    //        .when('/events/edit/:id', {
    //            templateUrl: 'views/partials/editEvent.html',
    //            controller: 'eventEditController'
    //        })
    //        .when('/events/delete/:id', {
    //            templateUrl: 'views/partials/deleteEvent.html',
    //            controller: 'eventDeleteController'
    //        });
    //    $locationProvider.html5Mode(true);       

    //}]);

})();