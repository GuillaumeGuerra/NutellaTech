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

newPatchController.$inject = ['$scope', '$routeParams', '$mdDialog', '$timeout', 'Patches'];

function newPatchController($scope, $routeParams, $mdDialog, $timeout, patches) {
    $scope.gerritId = undefined;
    $scope.newGerrit = {};
    $scope.newGerrit.gerrit = {};
    $scope.isProgressBarVisible = false;
    var searchForGerritPromise;
    var gerritId;

    $scope.showSpinner = function (isLoading) {
        $scope.isProgressBarVisible = isLoading;
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

            // Cancelling previous timeout promise, in case it was already ongoing
            if (searchForGerritPromise)
                $timeout.cancel(searchForGerritPromise);

            gerritId = newValue;
            searchForGerritPromise = $timeout(function () {
                console.log('Gerrit id modified, calling the WebApi');
                $scope.searchForGerrit(gerritId);
            }, 250); // delay 250 ms
        }
    });

    $scope.searchForGerrit = function (gerritId) {
        console.log("Looking for gerrit information for gerrit " + gerritId);

        $scope.newGerrit.gerrit.id = gerritId;

        // We have to save those inputs, as they would get overriden by the webapi result (the entire object is replaced with the promise result)
        $scope.userInput = {
            "asset": $scope.newGerrit.asset,
            "owner": $scope.newGerrit.owner
        }

        $scope.showSpinner(true);
        patches.get({ patchId: gerritId, releaseVersion: $routeParams.releaseVersion, isAction: 'action', action: 'preview' }).$promise.then(
            function (result) {
                console.log(result);

                result.asset = $scope.userInput.asset;
                result.owner = $scope.userInput.owner;

                $scope.newGerrit = result;
                $scope.showSpinner(false);
            });
    };
}
