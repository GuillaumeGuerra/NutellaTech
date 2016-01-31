'use strict';

angular
    .module('PatchManager')
    .config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/editBook/:bookTitle', {
                    templateUrl: 'app/partials/edit-book.html',
                    controller: 'editBookController'
                });
        }]);

angular
    .module('PatchManager')
    .controller('editBookController', editBookController);

editBookController.$inject = ['$scope', '$routeParams', 'Books'];

function editBookController($scope, $routeParams, books) {
    $scope.Title = 'MON Q !!!!';

    console.log("getting the book with id " + $routeParams.bookTitle);

    $scope.book = books.get({ bookTitle: $routeParams.bookTitle });
};
