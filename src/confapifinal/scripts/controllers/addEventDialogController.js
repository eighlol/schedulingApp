angular.module('conferenceApp').controller('addEventDialogController', addEventDialogController);

addEventDialogController.$inject = ['$http', '$mdDialog', 'categoriesService', '$mdToast'];

function addEventDialogController($http, $mdDialog, categoriesService, $mdToast) {
    var vm = this;

    var promise = categoriesService.getCategories();
    promise.then(function (data) {
        vm.categories = data.data;
    });

    vm.$mdToast = $mdToast;

    vm.event = {
        name: "",
        description: "",
        categories: [],
        locations: []
    };

    vm.eventLocation = {};

    //if (angular.element(document).find('md-dialog').length > 0) {
    //    if (angular.element(document).find('mapsearch') != null && !locationInput) {

    //    }
    //}



    vm.$mdDialog = $mdDialog;
    
    vm.searchText = "";

    vm.selectedLocation = {
        name: ""
    };
    
 
    vm.cancel = function () {
        vm.$mdDialog.cancel();
    };

    vm.transformChip = function (chip) {
        return chip;
    };

    vm.saveEvent = function () {
        var location = $("#mapsearch").val();
        vm.selectedLocation.name = location;
        vm.event.locations = [];
        vm.event.locations.push(vm.selectedLocation);
        console.log(vm.event);
        $http.post("api/events", vm.event)
        .then(function (response) {
            vm.$mdDialog.hide(response);
        }, function () {
            vm.openToast("Error");
        })
        .finally(function () {

        });

        
    };

    vm.openToast = function (message) {
        vm.$mdToast.show(
            vm.$mdToast.simple()
                .textContent(message)
                .position('top right')
                .hideDelay(2000)
            );
    };

   
};