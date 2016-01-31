(function () {
    'use strict';

    var app = angular.module('XOneBlotter', [
        // Angular modules 
        'ngRoute',

        // Custom modules 
        'booksServices'

        // 3rd Party Modules

    ]);

    app.config(['$routeProvider',
      function ($routeProvider) {
          $routeProvider.
            //when('/books', {
            //    templateUrl: 'app/partials/book-list.html',
            //    controller: 'booksController'
            //}).
            when('/home', {
                templateUrl: 'app/partials/home.html'
            }).
            when('/addBook', {
                templateUrl: 'app/partials/add-book.html',
                controller: 'addBookController'
            }).
            //when('/editBook/:bookTitle', {
            //    templateUrl: 'app/partials/edit-book.html',
            //    controller: 'editBookController'
            //}).
            otherwise({
                redirectTo: '/home'
            });
      }]);
})();