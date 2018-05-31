(function () {
    'use strict';
    angular.module('schedulingApp').service("categoriesService", categoriesService);

    categoriesService.$inject = ["$http"];

    function categoriesService($http) {
        this.getCategories = function () {
            console.log("321");
            var request = new Promise(function(resolve, reject) {
                $http.get("api/categories")
                    .then(function(response) {
                        if (response.status === 200) {

                            console.log("200" + response);
                            resolve(response.data.categories);
                        } else {
                            console.log("reject" + response);
                            reject(response.statusText);
                        }
                    })
                    .catch(function (error) {
                        console.log("error" + error);
                        reject(error);
                    }); 
            });
    
            return request;            
        };
    
    };    
})();