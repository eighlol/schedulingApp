(function () {
    'use strict';

    angular.module('schedulingApp').controller('mainController', mainController);

    mainController.$inject = ['$scope', '$mdSidenav', 'eventsService', 'locationService', '$mdToast', '$mdDialog', '$mdMedia', 'membersService', 'filterFilter'];

    function mainController($scope, $mdSidenav, eventsService, locationService, $mdToast, $mdDialog, $mdMedia, membersService, filterFilter) {
        var vm = this;

        eventsService.getEvents()
            .then(function (events) {
                $scope.$apply(function() {                    
                    vm.events = events;
                    vm.selectedEvent = vm.events[vm.events.length - 1];
                });
            });

        membersService.getAllMembers()
            .then(function(members) {
                vm.allMembers = members;
            });

        vm.searchMemberText = "";
        vm.pageHeader = "Pasākumu apraksts";
           
        vm.allMembers = [];
        vm.selectedMember = "";
        vm.searchText = "";
        vm.selectedEvent = null;
        vm.tabId = 0;

        vm.addNewEvent = function () {

        };

        vm.selectEvent = function (event) {
            vm.selectedEvent = event;
            var sidenav =  $mdSidenav('left');
            if (sidenav.isOpen()) {
                sidenav.close();
            }
            vm.tabId = 0;
        };

        vm.addEvent = function ($event) {
            var useFullscreen = ($mdMedia('sm') || $mdMedia('xs'));
            console.log("44");
            $mdDialog.show({
                templateUrl: '../views/partials/addEvent.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                clickOutsideToClose: true,
                fullscreen: useFullscreen
            }).then(function (response) {
                vm.events.push(response);
                vm.selectEvent(response);
                vm.openToast("Pasākums pievienots");
            });
        };

        vm.formScope = {};

        vm.setFormScope = function (scope) {
            vm.formScope = scope;
        };

        vm.addMemberToEvent = function () {
            if (vm.selectedMember) {
                membersService.addMemberToEvent(vm.selectedMember.id, vm.selectedEvent.id)
                    .then(function (response) {
                        vm.selectedEvent.members.push(vm.selectedMember);
                        vm.formScope.memberForm.$setUntouched();
                        vm.formScope.memberForm.$setPristine();
                        vm.selectedMember = null;
                        vm.searchMemberText = "";
                        vm.openToast("Dalībnieks tika pievienots.");
                    }, function (response) {                        
                        vm.openToast("Kļūda: ", response.data);
                    });
            } else {
                vm.openToast("Dalībnieks netika atrasts.");
            }
           
        }

        vm.removeMember = function (member) {
            var eventIndex = vm.selectedEvent.members.indexOf(member);
            membersService.deleteMemberFromEvent(member.id, vm.selectedEvent.id)
                .then(function (response) {
                    vm.selectedEvent.members.splice(eventIndex, 1);
                    vm.openToast("Dalībnieks tika noņemts");
                }, function (error) {
                    vm.openToast("Failed to delete member");
                });                
        };

        vm.removeAllMembers = function ($event) {
          var confirm =  $mdDialog.confirm()
                .title('Jūs tiešam gribāt nodzēst visus dalībniekus?')
                .textContent('Visi dalībnieki būs noņemti.')
                .targetEvent($event)
                .ok('Nodzēst')
                .cancel('Nē');

          $mdDialog.show(confirm)
            .then(function () {
                membersService.deleteAllMembersFromEvent(vm.selectedEvent.id)
                    .then(function (response) {
                        vm.selectedEvent.members = [];
                        vm.openToast("Visi dalībnieki tika nodzēsti");
                    }, function (error) {
                        vm.openToast("Failed to delete all members");
                    });
          });

        };

        vm.newLocation = {};

        vm.formScope2 = {};

        vm.setFormScope2 = function (scope) {
            vm.formScope2 = scope;
        };

        vm.addLocationToEvent = function () {
            var location = $("#locationName").val();
            vm.newLocation.name = location;
            var request = {
                location: vm.newLocation
            };

            if (vm.newLocation.name) {
                locationService.addLocationToEvent(vm.selectedEvent.id, request)
                    .then(function() {
                        locationService.getEventLocations(vm.selectedEvent.id)
                            .then(function(response) {
                                vm.selectedEvent.locations = response.locations;
                            });
                        vm.formScope2.formLocation.$setUntouched();
                        vm.formScope2.formLocation.$setPristine();
                        vm.newLocation = null;
                        vm.openToast("Lokācija bija pievienota.");
                    }, function(response) {
                        vm.openToast(response.data);
                    });
            } else {
                vm.openToast("Lokācija nebija atrasta.");
            }
        };

        vm.removeLocation = function(location) {
            var locationIndex = vm.selectedEvent.locations.indexOf(location);
            locationService.removeLocationFromEvent(location.id, vm.selectedEvent.id)
                .then(function () {
                    vm.selectedEvent.locations.splice(locationIndex, 1);
                    vm.openToast("Lokācija tika noņemta");
                }, function (error) {
                    vm.openToast("Neizdevās nodzēst dalībnieku");
                });
        }

        vm.filterMembers = function (expr) {
            var fList = angular.copy(vm.allMembers);
            for (var i = fList.length - 1; i >=0; i--) {
                var element = fList[i];
                for (var j = 0, length1 = vm.selectedEvent.members.length; j < length1; j++) {
                    var notToAdd = vm.selectedEvent.members[j];
                    if (element && (element["id"] === notToAdd["id"])) {
                        fList.splice(i, 1);
                    }
                }
            }

            fList = filterFilter(fList, expr);                      
            return fList;
        };

        vm.toggleSideNav = function () {
            $mdSidenav('left').toggle();
        };

        vm.openToast = function (message) {
            $mdToast.show(
                $mdToast.simple()
                    .textContent(message)
                    .position('top right')
                    .hideDelay(3000)
                );
        };
    }
})();
