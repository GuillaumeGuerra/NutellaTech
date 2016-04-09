'use strict';

angular
    .module('PatchManager')
    .controller('newPatchController', newPatchController).
    config(function ($mdThemingProvider) {
        // Configure a dark theme with primary foreground yellow
        $mdThemingProvider.theme('docs-dark', 'default')
          .primaryPalette('grey')
          .dark();
    });;

newPatchController.$inject = ['$scope', '$routeParams', '$mdDialog', 'Patches'];

function newPatchController($scope, $routeParams, $mdDialog, patches) {
    $scope.gerritId = undefined;
    $scope.newGerrit = {};
    $scope.newGerrit.gerrit = {};
    $scope.loadingMode = '';

    $scope.showSpinner = function(isLoading) {
        if (isLoading)
            $scope.loadingMode = 'indeterminate';
        else
            $scope.loadingMode = '';

    };

    $scope.hide = function () {
        $mdDialog.hide();
    };
    $scope.cancel = function () {
        $mdDialog.cancel();
    };
    $scope.answer = function (answer) {
        $mdDialog.hide(answer);
    };

    $scope.$watch('gerritId', function (newValue, oldValue) {
        if ($scope.gerritId != undefined) {
            console.log('Gerrit id modified, calling the WebApi');
            $scope.searchForGerrit();
        }
    });

    $scope.searchForGerrit = function () {
        console.log("Looking for gerrit information for gerrit " + $scope.gerritId);

        $scope.newGerrit.gerrit.id = $scope.gerritId;
        // We have to save those inputs, as they would get overriden by the webapi result (the entire object is replaced with the promise result)
        $scope.userInput = {
            "asset": $scope.newGerrit.asset,
            "owner": $scope.newGerrit.owner
        }

        $scope.showSpinner(true);
        patches.get({ patchId: $scope.newGerrit.gerrit.id, releaseVersion: $routeParams.releaseVersion, isAction: 'action', action: 'preview' }).$promise.then(
            function (result) {
                console.log(result);

                result.asset = $scope.userInput.asset;
                result.owner = $scope.userInput.owner;

                $scope.newGerrit = result;
                $scope.showSpinner(false);
            });
    };
}
