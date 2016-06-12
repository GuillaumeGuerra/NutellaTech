'use strict';

angular
    .module('PatchManager')
    .controller('sidenavController', sidenavController);

sidenavController.$inject = ['$scope', "$location", 'Releases', 'PatchManagerContext'];

function sidenavController($scope, $location, releases, context) {

    console.log("fetching all releases !!");

    $scope.allReleases = releases.query();
    $scope.allReleases.$promise.then(function (result) {
        result.forEach(function (release) {
            if (release.isCurrent === true) {
                $scope.currentRelease = release;
            }
        });
    });

    $scope.selectRelease = function (releaseVersion) {
        console.log("Selecting release " + releaseVersion);

        $location.url('/releases/' + releaseVersion);
    };

    $scope.settings = context.settings;

    $scope.$watch(
        function () { return $scope.settings.showGrid; },
        function (newValue, oldValue) {
            if (newValue !== oldValue)
                $scope.settings.switchView(newValue);
        });
};

