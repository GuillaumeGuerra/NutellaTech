'use strict';

angular.module('myApp.view1', ['ngRoute', 'ui.grid', 'ui.grid.exporter', 'ui.grid.selection'])
//angular.module('myApp.view1', ['ngRoute'])
    .config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/view1', {
            templateUrl: 'view1/view1.html',
            controller: 'View1Ctrl'
        });
    }])

    .controller('View1Ctrl', ['$scope', function ($scope) {
        $scope.jediMembers = [
            {name: 'Yoda', rank: 'Greatest of all times', level: 'Good'},
            {name: 'ObiWan', rank: 'not great', level: 'Medium'},
            {name: 'Mace', rank: 'not great', level: 'Medium'},
            {name: 'QuiGon', rank: 'poor', level: 'GrandPa'}
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
                if($scope.jediMembers[index].name===data.name)
                    $scope.jediMembers.splice(index,1);
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
    }]);