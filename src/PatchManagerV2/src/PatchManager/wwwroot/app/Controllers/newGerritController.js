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

        // We have to save those inputs, as they would get overriden by the webapi result (the entire object is replaced with the promise result)
        $scope.userInput = {
            "asset": $scope.newGerrit.asset,
            "owner": $scope.newGerrit.owner
        }

        console.log("saved owner is " + $scope.userInput.owner);
        console.log("initial owner is " + $scope.newGerrit.owner);

        gerrits.get({ gerritId: $scope.newGerrit.id, releaseVersion: $routeParams.releaseVersion, isAction: 'action', action: 'preview' }).$promise.then(
            function (result) {
                console.log(result);
                console.log("restored owner is " + $scope.userInput.owner);

                result.asset = $scope.userInput.asset;
                result.owner = $scope.userInput.owner;

                $scope.newGerrit = result;
            });
    };
}
