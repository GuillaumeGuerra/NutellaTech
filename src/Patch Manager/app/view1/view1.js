'use strict';

angular.module('myApp.view1', ['ngRoute', 'ui.grid', 'ui.grid.exporter', 'ui.grid.selection', 'winjs'])
    .config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/view1', {
            templateUrl: 'view1/view1.html',
            controller: 'View1Ctrl'
        });
    }])

    .controller('View1Ctrl', ['$scope', function ($scope) {
        $scope.jediMembers = [
            {
                name: 'Yoda',
                rank: 'Greatest of all times',
                level: 'Good',
                picture: 'http://img3.wikia.nocookie.net/__cb20100724122309/fr.starwars/images/thumb/4/45/Yoda.jpg/250px-Yoda.jpg'
            },
            {
                name: 'ObiWan',
                rank: 'not great',
                level: 'Medium',
                picture: 'http://img2.wikia.nocookie.net/__cb20140611153427/fr.starwars/images/3/39/250px-Obi-Wan.jpg'
            },
            {
                name: 'Mace',
                rank: 'not great',
                level: 'Medium',
                picture: 'http://img2.wikia.nocookie.net/__cb20110824103326/fr.starwars/images/thumb/7/70/250px-Mace_Windu.jpg/250px-250px-Mace_Windu.jpg'
            },
            {
                name: 'QuiGon',
                rank: 'poor',
                level: 'GrandPa',
                picture: 'http://img4.wikia.nocookie.net/__cb20110814182918/fr.starwars/images/f/fc/Qui-gonjinn.jpg'
            }
        ];
        $scope.selectedJediName = $scope.jediMembers[0].name;
        $scope.selectedJediRank = $scope.jediMembers[0].rank;

        $scope.selectJedi = function (jedi) {
            $scope.selectedJediName = jedi.name;
            $scope.selectedJediRank = jedi.rank;
        };

        $scope.removeJedi = function (data) {
            alert('Killing ! ' + data.name);

            for (var index in $scope.jediMembers) {
                if ($scope.jediMembers[index].name === data.name)
                    $scope.jediMembers.splice(index, 1);
            }
        };

        $scope.gridOptions = {
            enableFiltering: true,
            data: $scope.jediMembers,
            columnDefs: [
                {field: 'name', cellClass: 'jediName'},
                {field: 'rank'},
                {
                    field: 'level',
                    cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                        if (grid.getCellValue(row, col) === 'Good') {
                            return 'blue';
                        }
                        else if (grid.getCellValue(row, col) === 'Medium') {
                            return 'orange';
                        }
                        else {
                            return 'red';
                        }
                    }
                },
                {
                    field: 'edit',
                    cellTemplate: '<button type="button"  ng-click="grid.appScope.removeJedi(row.entity)" >Kill !</button> '
                }
            ],
            enableGridMenu: true,
            exporterMenuCsv: true
        };

        $scope.rating = 1;
        $scope.maxRating = 5;

        var Mode = {
            editMode: {
                text: "edit",
                selectionMode: "multi",
                tapBehavior: "toggleSelect"
            },
            readOnly: {
                text: "readonly",
                selectionMode: "none",
                tapBehavior: "none"
            }
        };

        $scope.toggleEditMode = function () {
            $scope.mode = ($scope.mode === Mode.readOnly) ? Mode.editMode : Mode.readOnly;
        };

        $scope.addItem = function () {
            $scope.jediMembers.splice(0, 0, {
                name: 'ObiWan',
                rank: 'not great',
                level: 'Medium',
                picture: 'http://img2.wikia.nocookie.net/__cb20140611153427/fr.starwars/images/3/39/250px-Obi-Wan.jpg'
            });
        };

        $scope.removeItem = function () {
            // Remove the items that are selected
            for (var i = $scope.selection.length - 1; i > -1; i--) {
                $scope.jediMembers.splice($scope.selection[i], 1);
            }
        };

        $scope.selection = [];

        $scope.mode = Mode.readOnly;
    }]);