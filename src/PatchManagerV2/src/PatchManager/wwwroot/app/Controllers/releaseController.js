(function () {
    'use strict';

    angular
        .module('PatchManager')
        .controller('releaseController', releaseController);

    angular
        .module('PatchManager').config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/releases/:releaseVersion', {
                    templateUrl: 'app/partials/release.html',
                    controller: 'releaseController'
                });
        }]);

    releaseController.$inject = ['$scope', '$routeParams', 'Releases', 'Patches', '$mdDialog', '$mdToast', '$mdBottomSheet', 'PatchManagerContext'];

    function releaseController($scope, $routeParams, releases, patches, $mdDialog, $mdToast, $mdBottomSheet, context) {
        /* jshint validthis:true */

        $scope.settings = context.settings;
        $scope.resolvedSettings = {};
        $scope.patchesLoading = true;

        var originatorEv;

        var columnDefs = [
            {
                headerName: 'Patch',
                children: [
                    { headerName: "Asset", field: "asset" },
                    { headerName: "Owner", field: "owner" },
                    { headerName: "Jira", field: "jira.id" },
                    { headerName: "Gerrit", field: "gerrit.id" },
                    { headerName: "Description", field: "gerrit.description" }
                ]
            },
            {
                headerName: 'Status',
                children: [
                    { headerName: "Registration Status", field: "status.registration" },
                    { headerName: "Jira Status", field: "status.jira" },
                    { headerName: "Gerrit Status", field: "status.gerrit" },
                    { headerName: "Test Status", field: "status.test" },
                ]
            }
        ];

        $scope.gridOptions = {
            columnDefs: columnDefs,
            enableColResize: true,
            enableSorting: true,
            enableFilter: true,
            rowData: []
        };

        $scope.showSpinner = function (patch, isLoading) {
            patch.isProgressBarVisible = isLoading;
            if (isLoading)
                patch.loadingMode = 'indeterminate';
            else
                patch.loadingMode = '';
        };

        $scope.shouldShowSpinner = function (patch) {
            if (patch.isProgressBarVisible == undefined)
                return false;
            else
                return patch.isProgressBarVisible;
        };

        $scope.refreshPatch = function (patch) {
            console.log("refreshing patch " + patch.gerrit.id);

            $scope.showSpinner(patch, true);
            patches.get({ patchId: patch.gerrit.id, releaseVersion: $routeParams.releaseVersion }).$promise.then(function (result) {

                console.log("Refreshing statuses for gerrit " + result.gerrit.id);
                patch.status = result.status;
                $scope.showSpinner(patch, false);
            });
        };

        $scope.refreshAllPatches = function () {
            console.log("Refreshing all gerrits");

            $scope.patches.forEach(function (patch) {
                $scope.refreshPatch(patch);
            });
        };

        $scope.saveGerrit = function () {
            console.log("Saving new gerrit " + $scope.newGerrit.gerrit.id);

            patches.save({ patchId: '', releaseVersion: $routeParams.releaseVersion }, $scope.newGerrit);

            $scope.patches.push($scope.newGerrit);
        }

        $scope.applyActionToGerrit = function (patch, actionToApply) {
            console.log("Applying action " + actionToApply + " to patch " + patch.gerrit.id);

            $scope.showSpinner(patch, true);
            patch.$action({ releaseVersion: $routeParams.releaseVersion, action: actionToApply }).then(function (result) {
                $scope.showSimpleToast('Action [' + actionToApply + '] has been properly applied to patch ' + patch.gerrit.id);
                $scope.showSpinner(patch, false);
            });
        }

        $scope.openMenu = function ($mdOpenMenu, ev) {
            console.log("opening menu");

            originatorEv = ev;
            $mdOpenMenu(ev);
        };

        $scope.ShowNewPatchModalShowAdvanced = function (ev) {
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

        $scope.showSimpleToast = function (message) {
            $mdToast.show(
              $mdToast.simple()
                .textContent(message)
                .position('bottom left')
                .hideDelay(3000)
            );
        };

        $scope.showQuickActions = function () {
            console.log('showing quick actions');
            $mdBottomSheet.show({
                templateUrl: 'app/partials/release-quick-actions.html',
                controller: 'releaseController',
                scope: $scope,
                preserveScope: true,
                clickOutsideToClose: true,
                escapeToClose: true
            });
        };

        $scope.hideQuickActions = function () {
            $mdBottomSheet.hide();
        };

        $scope.shouldShowPatch = function (patch) {
            if ($scope.settings.mode === "Patches Gathering")
                return true;
            else if ($scope.settings.mode === "Merges") {
                if (patch.status.gerrit !== 'ReadyForMerge' && $scope.settings.hideNonReadyGerrits)
                    return false;
                if (patch.status.registration === 'Refused' && !$scope.settings.showRejected)
                    return false;
            }
            else {

                if (patch.status.test === 'Tested' && $scope.settings.hideTestedGerrits)
                    return false;
                if (patch.status.merge !== 'Merged' && $scope.settings.hideNonMergedGerrits)
                    return false;
            }

            return true;
        };

        $scope.$watch(
            function () { return $scope.settings; },
            function (newValue, oldValue) {
                console.log("Settings updated, resolving new values");

                if ($scope.settings.mode === "Patches Gathering") {
                    $scope.resolvedSettings.showRegistrationStatus = true;
                    $scope.resolvedSettings.showMergeStatus = false;
                    $scope.resolvedSettings.showJiraStatus = false;
                    $scope.resolvedSettings.showTestStatus = false;
                }
                else if ($scope.settings.mode === "Merges") {
                    $scope.resolvedSettings.showRegistrationStatus = true;
                    $scope.resolvedSettings.showMergeStatus = true;
                    $scope.resolvedSettings.showJiraStatus = true;
                    $scope.resolvedSettings.showTestStatus = false;
                } else {
                    $scope.resolvedSettings.showRegistrationStatus = false;
                    $scope.resolvedSettings.showMergeStatus = false;
                    $scope.resolvedSettings.showJiraStatus = false;
                    $scope.resolvedSettings.showTestStatus = true;
                }
            }, true);

        function activate() {

            console.log("getting all the gerrits related to release " + $routeParams.releaseVersion);

            $scope.release = releases.get({ releaseVersion: $routeParams.releaseVersion });

            patches.query({ releaseVersion: $routeParams.releaseVersion })
                .$promise.then(function (data) {
                    $scope.patchesLoading = false;
                    $scope.patches = data;

                    $scope.gridOptions.api.setRowData(data);
                });
        }

        activate();
    }
})();
