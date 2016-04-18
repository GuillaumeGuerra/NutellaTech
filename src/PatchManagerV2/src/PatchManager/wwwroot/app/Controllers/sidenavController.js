'use strict';

angular
    .module('PatchManager')
    .controller('sidenavController', sidenavController);

sidenavController.$inject = ['$scope', "$location", 'Releases', 'PatchManagerContext'];

function sidenavController($scope, $location, releases, context) {

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
    };

    $scope.settings = context.settings;

    $scope.$watch(
        function () { return $scope.settings.showCards; },
        function (newValue, oldValue) {
            if (newValue === true) {
                $scope.settings.viewType = 'Grid';
                $scope.settings.viewTypeIcon = 'view_headline';
            } else {
                $scope.settings.viewType = 'Cards';
                $scope.settings.viewTypeIcon = 'view_module';
            }
        });
};

