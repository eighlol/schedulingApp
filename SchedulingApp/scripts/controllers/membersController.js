(function () {
    'use strict';

    angular.module('schedulingApp')
        .controller('membersController', membersController);

    membersController.$inject = ['$scope', '$mdDialog', '$mdMedia', '$mdToast', 'membersService'];

    function membersController($scope, $mdDialog, $mdMedia, $mdToast, membersService) {
        var vm = this;
        vm.pageHeader = "Dalībnieki";
        vm.customFullscreen = $mdMedia('xs') || $mdMedia('sm');

        vm.getAllMembers = function() {
            membersService.getAllMembers()
                .then(function(members) {
                    $scope.$apply(function(){
                        vm.members = members;
                    });
                });
        };

        vm.getAllMembers();

        vm.openToast = function (message) {
            $mdToast.show(
                $mdToast.simple()
                    .textContent(message)
                    .position('top right')
                    .hideDelay(3000)
                );
        };

        vm.addMember = function (ev) {
            var useFullscreen = ($mdMedia('sm') || $mdMedia('xs'));

            $mdDialog.show({
                templateUrl: '../views/partials/addMember.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: false,
                fullscreen: useFullscreen
            }).then(function (response) {
                vm.getAllMembers();              
                vm.openToast("New member successfully added");
            });
        };

        vm.deleteMember = function(ev, member) {
            var confirm = $mdDialog.confirm()
                .title('Do you want to delete member?')
                .textContent('Please confirm to delete ' + member.name)
                .ariaLabel('Delete member')
                .targetEvent(ev)
                .ok('Delete')
                .cancel('Cancel');
            $mdDialog.show(confirm)
                .then(function() {
                    membersService.deleteMember(member.id)
                        .then(function(response) {
                            vm.getAllMembers();              
                            vm.openToast("Member removed");
                        });;
                });
        };
    };        
})();