'use strict';

angular.module('PatchManager').config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/patches/:patchVersion', {
                    templateUrl: 'app/partials/patch.html',
                    controller: 'patchController'
                });
        }]);

angular
    .module('PatchManager')
    .controller('patchController', patchController);

patchController.$inject = ['$scope', '$routeParams', 'Patches', 'Gerrits', '$mdDialog', '$mdMedia'];

function patchController($scope, $routeParams, patches, gerrits, $mdDialog, $mdMedia) {

    console.log("getting all the gerrits related to patch " + $routeParams.patchVersion);

    var originatorEv;

    $scope.patch = patches.get({ patchVersion: $routeParams.patchVersion });

    $scope.gerrits = gerrits.query({ patchVersion: $routeParams.patchVersion });

    $scope.refreshGerrit = function (gerrit) {
        console.log("refreshing gerrit " + gerrit.id);

        gerrits.get({ gerritId: gerrit.id, patchVersion: $routeParams.patchVersion }).$promise.then(function (result) {

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

        gerrits.save({ gerritId: '', patchVersion: $routeParams.patchVersion }, $scope.newGerrit);

        $scope.gerrits.push($scope.newGerrit);
    }

    $scope.applyActionToGerrit = function (gerrit, actionToApply) {
        console.log("Applying action " + actionToApply + " to gerrit " + gerrit.id);

        gerrit.$action({ patchVersion: $routeParams.patchVersion, action: actionToApply });
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
