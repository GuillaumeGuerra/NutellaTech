'use strict';

angular.module('PatchManager').config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/patches/:releaseVersion', {
                    templateUrl: 'app/partials/release.html',
                    controller: 'releaseController'
                });
        }]);

angular
    .module('PatchManager')
    .controller('releaseController', releaseController);

releaseController.$inject = ['$scope', '$routeParams', 'Patches', 'Gerrits', '$mdDialog'];

function releaseController($scope, $routeParams, patches, gerrits, $mdDialog) {

    console.log("getting all the gerrits related to release " + $routeParams.releaseVersion);

    var originatorEv;

    $scope.release = patches.get({ patchVersion: $routeParams.releaseVersion });

    $scope.gerrits = gerrits.query({ patchVersion: $routeParams.releaseVersion });

    $scope.refreshGerrit = function (gerrit) {
        console.log("refreshing gerrit " + gerrit.id);

        gerrits.get({ gerritId: gerrit.id, patchVersion: $routeParams.releaseVersion }).$promise.then(function (result) {

            console.log("Refreshing statuses for gerrit " + result.id);
            gerrit.status = result.status;
        });
    };

    $scope.refreshAllGerrits = function () {
        console.log("Refreshing all gerrits");

        $scope.gerrits.forEach(function (gerrit) {
            $scope.refreshGerrit(gerrit);
        });
    };

    $scope.saveGerrit = function () {
        console.log("Saving new gerrit " + $scope.newGerrit.id);

        gerrits.save({ gerritId: '', patchVersion: $routeParams.releaseVersion }, $scope.newGerrit);

        $scope.gerrits.push($scope.newGerrit);
    }

    $scope.applyActionToGerrit = function (gerrit, actionToApply) {
        console.log("Applying action " + actionToApply + " to gerrit " + gerrit.id);

        gerrit.$action({ patchVersion: $routeParams.releaseVersion, action: actionToApply });
    }

    $scope.openMenu = function ($mdOpenMenu, ev) {
        console.log("opening menu");

        originatorEv = ev;
        $mdOpenMenu(ev);
    };

    $scope.showAdvanced = function (ev) {
        $mdDialog.show({
            controller: newGerritController,
            templateUrl: 'app/partials/add-gerrit.html',
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
