'use strict';

angular
    .module('PatchManager')
    .controller('sidenavController', sidenavController);

sidenavController.$inject = ['$scope', "$location", 'Releases'];

function sidenavController($scope, $location, releases) {

    console.log("fetching all releases !!");

    $scope.allReleases = releases.query();
    $scope.allReleases.$promise.then(function (result) {

        console.log("Iterating on all releases");

        result.forEach(function (release) {
            console.log("Current release is " + release.version);
            if (release.isCurrent === true) {
                console.log("Found the current release !!! " + release.version);
                $scope.currentRelease = release;
            }
        });
    });

    $scope.selectRelease = function (releaseVersion) {
        console.log("Selecting release " + releaseVersion);

        $location.url('/releases/' + releaseVersion);
    }
};

