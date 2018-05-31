(function () {
    'use strict';

    angular.module('schedulingApp')
        .controller('eventController', eventController);

        eventController.$inject = ['$scope', 'eventsService'];

    function eventController($scope, eventsService) {
        
    };
        
})();