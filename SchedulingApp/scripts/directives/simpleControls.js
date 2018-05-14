(function () {
    "use strict";
    angular.module("conferenceApp")
        .directive("waitLoader", waitLoader)
        .directive("googleAddresses", googleAddresses)
        .directive("googleMaps", googleMaps);

    function waitLoader() {
        return {
            scope: {
                displayWhen: "=displayWhen"
            },
            restrict: "E",
            templateUrl: "views/partials/waitLoader.html"
        };
    }

    function googleAddresses() {
        return {            
            restrict: 'A',
            link: function (scope, element, attrs) {
               new google.maps.places.SearchBox(element[0]);
            }
        }
    };

    function googleMaps() {
        return {
            restrict: 'A',
            transclude: true,
            scope: { location: '=googleMaps' },
            link: function (scope, element, attrs) {
                var latlng = {
                    lat: scope.location.latitude,
                    lng: scope.location.longitude
                };
                var map = new google.maps.Map(element[0], {
                    zoom: 15,
                    center: latlng
                });
                var marker = new google.maps.Marker({
                    position: latlng,
                    map: map
                });
            }
        }
    };
    //<wait-loader></wait-loader>
})();