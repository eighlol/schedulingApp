//(function () {
//    'use strict';

//    angular.module('conferenceApp')
//        .controller('eventController', eventController)
//        .controller('eventAddController', eventAddController)
//        .controller('eventEditController', eventEditController)
//        .controller('eventDeleteController', eventEditController);

//    eventController.$inject = ['$scope', 'Events'];
//    function eventController($scope, Events) {
//        $scope.events = Events.query();
//    }

//    eventAddController.$inject = ['$scope', 'Events', '$location'];
//    function eventAddController($scope, Events, $location) {

//        $scope.event = new Events();

//        $scope.addEvent = function () {
//            $scope.event.$save(function () {
//                $location.path('/');
//            });
//        };
//    }

//    eventEditController.$inject = ['$scope', 'Events', '$location', '$routeParams'];
//    function eventEditController($scope, Events, $location, $routeParams) {

//        $scope.event = Events.get({ id: $routeParams.id });

//        $.editEvent = function () {
//            $scope.event.$save(function () {
//                $location.path('/');
//            });
//        };
//    }

//    eventDeleteController.$inject = ['$scope', 'Events', '$location', '$routeParams'];
//    function eventDeleteController($scope, Events, $location, $routeParams) {

//        $scope.event = Events.get({ id: $routeParams.id });

//        $.deleteEvent = function () {
//            $scope.event.$remove({id: $scope.event.id } ,function () {
//                $location.path('/');
//            });
//        };
//    }

//})();
  