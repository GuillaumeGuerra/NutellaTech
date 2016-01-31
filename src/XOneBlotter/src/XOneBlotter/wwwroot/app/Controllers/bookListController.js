'use strict';

angular.module('XOneBlotter').config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/books', {
                    templateUrl: 'app/partials/book-list.html',
                    controller: 'bookListController'
                });
        }]);

angular
    .module('XOneBlotter')
    .controller('bookListController', booksController);

booksController.$inject = ['$scope', 'Books'];

function booksController($scope, books) {
    $scope.allBooks = books.query();
};

