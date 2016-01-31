(function () {
    'use strict';

    angular
        .module('XOneBlotter')
        .controller('editBookController', editBookController);

    editBookController.$inject = ['$scope', '$routeParams', 'Books'];

    function editBookController($scope, $routeParams, books) {
        $scope.Title = 'MON Q !!!!';

        console.log("getting the book with id " + $routeParams.bookTitle);

        $scope.book = books.get({ bookTitle: $routeParams.bookTitle });
    }
})();
