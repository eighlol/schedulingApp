(function () {
    'use strict';
    angular.module('conferenceApp').service("categoriesService", categoriesService);

    categoriesService.$inject = ["$http", "$q"];

    function categoriesService($http, $q) {

        var deferred = $q.defer();
        $http.get('api/categories').then(function (data) {
            deferred.resolve(data);
        });

        this.getCategories = function () {
            return deferred.promise;
        };
    };

})();