'use strict';

angular.module('PatchManager').config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/releases/:releaseVersion', {
                    templateUrl: 'app/partials/release.html',
                    controller: 'releaseController'
                });
        }]);

angular
    .module('PatchManager')
    .controller('releaseController', releaseController);

releaseController.$inject = ['$scope', '$routeParams', 'Releases', 'Patches', '$mdDialog'];

function releaseController($scope, $routeParams, releases, patches, $mdDialog) {

    console.log("getting all the gerrits related to release " + $routeParams.releaseVersion);

    var originatorEv;

    $scope.release = releases.get({ releaseVersion: $routeParams.releaseVersion });

    $scope.patches = patches.query({ releaseVersion: $routeParams.releaseVersion });

    $scope.refreshGerrit = function (gerrit) {
        console.log("refreshing gerrit " + gerrit.gerrit.id);

        patches.get({ gerritId: gerrit.id, releaseVersion: $routeParams.releaseVersion }).$promise.then(function (result) {

            console.log("Refreshing statuses for gerrit " + result.gerrit.id);
            gerrit.status = result.status;
        });
    };

    $scope.refreshAllGerrits = function () {
        console.log("Refreshing all gerrits");

        $scope.patches.forEach(function (patch) {
            $scope.refreshGerrit(patch);
        });
    };

    $scope.saveGerrit = function () {
        console.log("Saving new gerrit " + $scope.newGerrit.gerrit.id);

        patches.save({ patchId: '', releaseVersion: $routeParams.releaseVersion }, $scope.newGerrit);

        $scope.patches.push($scope.newGerrit);
    }

    $scope.applyActionToGerrit = function (gerrit, actionToApply) {
        console.log("Applying action " + actionToApply + " to gerrit " + gerrit.gerrit.id);

        gerrit.$action({ releaseVersion: $routeParams.releaseVersion, action: actionToApply });
    }

    $scope.openMenu = function ($mdOpenMenu, ev) {
        console.log("opening menu");

        originatorEv = ev;
        $mdOpenMenu(ev);
    };

    $scope.showAdvanced = function (ev) {
        $mdDialog.show({
            controller: newPatchController,
            templateUrl: 'app/partials/add-patch.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: false,
            fullscreen: true
        })
        .then(function (answer) {
            console.log("New gerrit received from the popup, saving it");
            $scope.newGerrit = answer;
            $scope.saveGerrit();
        }, function () {
            console.log("New gerrit creation cancelled");
        });
    };
}
