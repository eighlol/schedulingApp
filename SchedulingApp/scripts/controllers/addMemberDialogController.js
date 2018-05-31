(function () {
    'use strict';

    angular.module('schedulingApp').controller('addMemberDialogController', addMemberDialogController);

    addMemberDialogController.$inject = ['$http', '$mdDialog', '$mdToast', 'membersService'];

    function addMemberDialogController($http, $mdDialog, $mdToast, membersService) {
        console.log("66");
        var vm = this;
        vm.member = {};
        vm.genders = [
            {
                value: "male",
                text: "Male",
            }, {
                value: "female",
                text: "Female",
            }
        ];

        vm.cancel = function () {
            $mdDialog.cancel();
        };

        vm.submit = function () {
            membersService.addMember(vm.member)
                .then(function (response) {
                    $mdDialog.hide(response);
                }, function () {
                    vm.openToast("Error while adding new member");
                });
        };

        vm.openToast = function (message) {
            $mdToast.show(
                $mdToast.simple()
                    .textContent(message)
                    .position('top right')
                    .hideDelay(2000)
                );
        };
    };

})();