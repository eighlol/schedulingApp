(function () {
	'use strict';
	angular.module('schedulingApp').service("membersService", membersService);

	membersService.$inject = ["$http"];

	function membersService($http) {	
		this.getAllMembers = function () {
			var request = new Promise(function(resolve, reject) {
				$http.get('api/members')
					.then(function (response) {
						if (response.status === 200) {
							resolve(response.data.members);
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

		this.getAllMembersForEvent = function (eventId) {
			var request = new Promise(function(resolve, reject) {
				$http.get('api/events/' + eventId)
					.then(function (response) {
						if (response.status === 200) {
							resolve(response.data.members);
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

		this.addMember = function (member) {
			var request = new Promise(function(resolve, reject) {
				$http.post('api/members', member)
					.then(function (response) {
						if (response.status === 200) {
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

		this.addMemberToEvent = function (memberId, eventId) {
			var request = new Promise(function(resolve, reject) {
				$http.put('api/events/' + eventId + '/members', { memberId: memberId })
					.then(function (response) {
						if (response.status === 200) {
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

		this.deleteMember = function (memberId) {
			var request = new Promise(function(resolve, reject) {
				$http.delete('api/members/' + memberId)
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

		this.deleteMemberFromEvent = function (memberId, eventId) {
			var request = new Promise(function(resolve, reject) {
				$http.delete('api/events/' + eventId + '/members/' + memberId)
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

		this.deleteAllMembersFromEvent = function (eventId) {
			var request = new Promise(function(resolve, reject) {
				$http.delete('api/events/' + eventId + '/members')
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
	};

})();