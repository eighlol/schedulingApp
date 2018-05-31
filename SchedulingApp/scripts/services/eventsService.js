(function () {
    'use strict';
    angular.module('schedulingApp').service("eventsService", eventsService);

    eventsService.$inject = ["$http"];

    function eventsService ($http) {
        this.getEvents = function () {
            var request = new Promise(function(resolve, reject) {
                $http.get('api/events')
                    .then(function(response) {
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

        this.addEvent = function (event) {
            var initialEvent = Object.assign({}, event);
            transformCategories(event);
			var request = new Promise(function(resolve, reject) {
				$http.post('api/events', event)
					.then(function (response) {
                        if (response.status === 200) {
                            console.log(initialEvent);
                            resolve(initialEvent);
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
        
        this.deleteEvent = function (eventId) {
			var request = new Promise(function(resolve, reject) {
				$http.delete('api/events/' + eventId)
					.then(function (response) {
						if (response.status === 204) {
							resolve();
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

        function transformCategories(event) {
            event.categories = event.categories.map(function (item) {
                return { categoryId: item.id };
            });
        }
    };
})();