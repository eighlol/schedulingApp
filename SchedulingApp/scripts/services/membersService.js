(function () {
	'use strict';
	angular.module('conferenceApp').service("membersService", membersService);

	membersService.$inject = ["$http", "$q"];

	function membersService($http, $q) {

		var deferred = $q.defer();
		$http.get('api/members').then(function (data) {
			deferred.resolve(data);
		});

		this.getAllMembers = function () {
			return deferred.promise;
		}
	};

})();