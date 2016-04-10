(function () {
    'use strict';

    angular
        .module('PatchManager')
        .controller('homeController', homeController);

    homeController.$inject = ['$scope', '$location', '$mdSidenav'];

    function homeController($scope, $location, $mdSidenav) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'homeController';

        activate();

        function activate() { }

        $scope.toggleSidenav = function () {
            $mdSidenav('left').toggle();
        }
    }
})();
