'use strict';

angular.module('PatchManager').config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/books', {
                    templateUrl: 'app/partials/book-list.html',
                    controller: 'bookListController'
                });
        }]);

angular
    .module('PatchManager')
    .controller('bookListController', booksController);

booksController.$inject = ['$scope', 'Books'];

function booksController($scope, books) {
    $scope.allBooks = books.query();
};

