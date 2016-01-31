'use strict';

angular
    .module('PatchManager')
    .controller('navbarController', navbarController);

navbarController.$inject = ['$scope', 'Patches'];

function navbarController($scope, patches) {

    console.log("fetching all patches !!");

    $scope.allPatches = patches.query();
    $scope.allPatches.$promise.then(function (result) {

        console.log("Iterating on all patches");

        result.forEach(function (patch) {
            console.log("Current patch is " + patch.Version);
            if (patch.IsCurrent === true) {
                console.log("Found the current patch !!! " + patch.Version);
                $scope.currentPatch = patch;
            }
        });
    });
};

