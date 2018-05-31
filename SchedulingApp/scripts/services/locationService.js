(function () {
    'use strict';
    angular.module('schedulingApp').service("locationService", locationService);

    locationService.$inject = ["$http"];

    function locationService ($http) {
        this.addLocationToEvent = function (eventId, location) {
            var request = new Promise(function(resolve, reject) {
                $http.post("/api/events/" + eventId + "/locations", location)
                    .then(function(response) {
                        console.log(response);
                        if (response.status === 200) {
                            resolve(response.data.events);
                        } else {
                            reject(response.statusText);
                        }
                    })
                    .catch(function(error) {
                        reject(error);
                    }); 
            });

            return request;            
        };

        this.getEventLocations = function (eventId) {
            var request = new Promise(function(resolve, reject) {
                $http.get("/api/events/" + eventId + "/locations")
                    .then(function(response) {
                        if (response.status === 200) {
                            resolve(response.data);
                        } else {
                            reject(response.statusText);
                        }
                    })
                    .catch(function(error) {
                        reject(error);
                    }); 
            });

            return request;            
        };

        this.removeLocationFromEvent = function (locationId, eventId) {
            var request = new Promise(function (resolve, reject) {
                $http.delete("/api/events/" + eventId + "/locations/" + locationId)
                    .then(function (response) {
                        if (response.status === 204) {
                            resolve();
                        } else {
                            reject(response.statusText);
                        }
                    })
                    .catch(function (error) {
                        reject(error);
                    });
            });

            return request;
        };
    };

})();