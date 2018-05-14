(function () {
    'use strict';
    angular.module('conferenceApp').service("eventsService", eventsService);

    eventsService.$inject = ["$http", "$q"];

    function eventsService ($http, $q) {

        var deferred = $q.defer();
        $http.get('api/events').then(function (data) {
            deferred.resolve(data);
        });

        this.getEvents = function ()
        {
            return deferred.promise;
        }
    };

})();