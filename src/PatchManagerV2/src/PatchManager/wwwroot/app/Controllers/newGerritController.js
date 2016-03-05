'use strict';

angular
    .module('PatchManager')
    .controller('newGerritController', newGerritController);

newGerritController.$inject = ['$scope', '$routeParams', '$mdDialog', 'Gerrits'];

function newGerritController($scope, $routeParams, $mdDialog, gerrits) {
    $scope.hide = function () {
        $mdDialog.hide();
    };
    $scope.cancel = function () {
        $mdDialog.cancel();
    };
    $scope.answer = function (answer) {
        $mdDialog.hide(answer);
    };

    $scope.searchForGerrit = function () {
        console.log("Looking for gerrit information for gerrit " + $scope.newGerrit.id);
        $scope.newGerrit = gerrits.get({ gerritId: $scope.newGerrit.id, patchVersion: $routeParams.patchVersion, isAction: 'action', action: 'preview' });
    };
}
