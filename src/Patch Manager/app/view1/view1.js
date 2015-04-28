'use strict';

angular.module('myApp.view1', ['ngRoute', 'ui.grid'])
//angular.module('myApp.view1', ['ngRoute'])
    .config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/view1', {
            templateUrl: 'view1/view1.html',
            controller: 'View1Ctrl'
        });
    }])

    .controller('View1Ctrl', ['$scope', function ($scope) {
        $scope.jediMembers = [
            {name: 'Yoda', rank: 'Greatest of all times'},
            {name: 'ObiWan', rank: 'not great'},
            {name: 'QuiGon', rank: 'poor'}
        ];
        $scope.selectedJediName = $scope.jediMembers[0].name;
        $scope.selectedJediRank = $scope.jediMembers[0].rank;

        $scope.selectJedi = function (jedi) {
            $scope.selectedJediName = jedi.name;
            $scope.selectedJediRank = jedi.rank;
        };

        $scope.gridOptions = {
            enableFiltering: true,
            data:$scope.jediMembers
        };
    }]);