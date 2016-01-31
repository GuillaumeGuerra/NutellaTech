﻿'use strict';

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

patchController.$inject = ['$scope', '$routeParams', 'Patches', 'Gerrits'];

function patchController($scope, $routeParams, patches, gerrits) {

    console.log("getting all the gerrits related to patch " + $routeParams.patchVersion);

    $scope.patch = patches.get({ patchVersion: $routeParams.patchVersion });

    $scope.gerrits = gerrits.query({ patchVersion: $routeParams.patchVersion });

    $scope.refreshGerrit = function (gerrit) {
        console.log("refreshing gerrit " + gerrit.id);

        gerrits.get({ gerritId: gerrit.id, patchVersion: $routeParams.patchVersion }).$promise.then(function (result) {

            console.log("Refreshing statuses for gerrit " + result.id);
            gerrit.status = result.status;
        });
    };
}