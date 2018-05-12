(function () {
    'use strict';

    angular.module('conferenceApp').controller('mainController', mainController);

    mainController.$inject = ['$mdSidenav', '$http', 'eventsService', '$mdToast', '$mdDialog', '$mdMedia', 'membersService', 'filterFilter'];

    function mainController($mdSidenav, $http, eventsService, $mdToast, $mdDialog, $mdMedia, membersService, filterFilter, $setPristine, $setUntouched) {
        var vm = this;
        var promiseEvents = eventsService.getEvents();
        promiseEvents.then(function (data) {
            vm.events = data.data;
            vm.selectedEvent = vm.events[vm.events.length - 1];
        });

        var promiseMembers = membersService.getAllMembers();
        promiseMembers.then(function (data) {
            vm.allMembers = data.data;
        });
        vm.$mdSidenav = $mdSidenav;
        vm.$mdToast = $mdToast;
        vm.$mdDialog = $mdDialog;
        vm.$mdMedia = $mdMedia;
        vm.searchMemberText = "";
           
        vm.allMembers = [];
        vm.selectedMember = "";
        vm.searchText = "";
        vm.selectedEvent = null;
        vm.tabId = 0;
        vm.errorMessage = ""; //add show message tool

        vm.isBusy = true; //add loader

        vm.addNewEvent = function () {

        };

        vm.selectEvent = function (event) {
            vm.selectedEvent = event;
            var sidenav =  vm.$mdSidenav('left');
            if (sidenav.isOpen()) {
                sidenav.close();
            }
            vm.tabId = 0;
        };

        vm.addEvent = function ($event) {
            var useFullscreen = (vm.$mdMedia('sm') || vm.$mdMedia('xs'));

            vm.$mdDialog.show({
                templateUrl: '/views/partials/addEvent.html',
                parent: angular.element(document.body),
                targetEvent: $event,
                controller: addEventDialogController,
                controllerAs: 'addCtrl',
                clickOutsideToClose: true,
                fullscreen: useFullscreen
            }).then(function (response) {
                vm.events.push(response.data);
                vm.selectEvent(response.data);
                vm.openToast("Pasākums pievienots");
            }, function () {
                //vm.openToast("Error");
            })
            .finally(function(){

            });
            
        };

        vm.formScope = {};

        vm.setFormScope = function (scope) {
            vm.formScope = scope;
        };

        vm.addMemberToEvent = function () {
            if (vm.selectedMember) {
                var eventId = vm.selectedEvent.id;
                $http.post("/api/events/" + eventId + "/members", vm.selectedMember)
                    .then(function (response) {
                        vm.selectedEvent.members.push(response.data);
                        //reset form
                        console.log(vm.formScope);
                        vm.formScope.memberForm.$setUntouched();
                        vm.formScope.memberForm.$setPristine();
                        vm.selectedMember = null;
                        vm.searchMemberText = "";
                        vm.openToast("Dalībnieks tika pievienots.");
                        console.log(2);
                    }, function (response) {                        
                        vm.openToast("Kļūda: ", response.data);
                    });
            } else {
                vm.openToast("Dalībnieks netika atrasts.");
            }
           
        }

        vm.removeMember = function (member) {
            vm.isBusy = true;
            var foundIndex = vm.selectedEvent.members.indexOf(member);
            var eventId = vm.selectedEvent.id;
            $http.delete("/api/events/" + eventId + "/members/" + member.id)
            .then(function (response) {
                vm.selectedEvent.members.splice(foundIndex, 1);
                vm.openToast("Dalībnieks tika noņemts");
            }, function (error) {
                vm.openToast("Failed to delete member");
            }).finally(function () {
                vm.isBusy = false;
            });                
        };

        vm.removeAllMembers = function ($event) {
          vm.isBusy = true;
          var eventId = vm.selectedEvent.id;
          var confirm =  vm.$mdDialog.confirm()
                .title('Jūs tiešam gribāt nodzēst visus dalībniekus?')
                .textContent('Visi dalībnieki būs noņemti.')
                .targetEvent($event)
                .ok('Nodzēst')
                .cancel('Nē');

          vm.$mdDialog.show(confirm).then(function () {
              $http.delete("/api/events/" + eventId + "/members")
                .then(function (response) {
                    vm.selectedEvent.members = [];
                    vm.openToast("Visi dalībnieki tika nodzēsti");
                }, function (error) {
                    vm.openToast("Failed to delete all members");
                }).finally(function () {
                    vm.isBusy = false;
                });
          });

        };

        vm.newLocation = {};

        vm.formScope2 = {};

        vm.setFormScope2 = function (scope) {
            vm.formScope2 = scope;
        };


        vm.addLocationToEvent = function () {
            if (vm.newLocation) {
                var location = $("#locationName").val();
                vm.newLocation.name = location;
                var eventId = vm.selectedEvent.id;
                console.log(vm.newLocation);
                $http.post("/api/events/" + eventId + "/locations", vm.newLocation)
                    .then(function (response) {
                        vm.selectedEvent.locations.push(response.data);
                        vm.formScope2.formLocation.$setUntouched();
                        vm.formScope2.formLocation.$setPristine();
                        vm.newLocation = null;
                        vm.openToast("Lokācija bija pievienota.");
                    }, function (response) {
                        console.log(response.data);
                        vm.openToast(response.data);
                    });
            } else {
                vm.openToast("Lokācija nebija atrasta.");
            }
        };

        vm.addNewMember = function (item) {
            var data = {
                name: item
            };
            $http.post("/api/members", data)
                .then(function (response) {
                    vm.allMembers.push(response.data);
                    vm.openToast("Ir izveidots jauns dalībnieks.");
                }, function (response) {
                    vm.openToast(response.data);
                });
        };


        vm.filterMembers = function (expr) {
            var fList = angular.copy(vm.allMembers);
            //for (var i = 0, length = vm.allMembers.length; i < length; i++) {
            //    var element = vm.allMembers[i];
            //    for (var j = 0, length1 = vm.selectedEvent.members.length; j < length1; j++) {
            //        var notToAdd = vm.selectedEvent.members[j];
            //        if (element["id"] !== notToAdd["id"]) {
            //            if(fList.indexOf(element) === -1) {
            //                fList.push(element);   
            //            }
                                             
            //        }
            //    }
            //}
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

        /* MATERIAL */

        vm.toggleSideNav = function () {
            vm.$mdSidenav('left').toggle();
        };

        vm.openToast = function (message) {
            vm.$mdToast.show(
                vm.$mdToast.simple()
                    .textContent(message)
                    .position('top right')
                    .hideDelay(3000)
                );
        };
        /* MATERIAL END*/
    }




})();