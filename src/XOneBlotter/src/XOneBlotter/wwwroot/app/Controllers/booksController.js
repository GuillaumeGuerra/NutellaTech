(function () {
    'use strict';

    angular
        .module('XOneBlotter')
        .controller('booksController', booksController);

    booksController.$inject = ['$scope', 'Books'];

    function booksController($scope, books) {
        $scope.allBooks = books.query();
    }
})();
