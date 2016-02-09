
'use strict';

var app = angular.module('PatchManager', [
    // Angular modules 
    'ngRoute',
    'ngMaterial',

    // Custom modules 
    'booksServices',
    'patchesServices',
    'gerritsServices'

    // 3rd Party Modules

]);

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/home', {
            templateUrl: 'app/partials/home.html'
        }).
        when('/addBook', {
            templateUrl: 'app/partials/add-book.html',
            controller: 'addBookController'
        }).
        otherwise({
            redirectTo: '/home'
        });
  }]);
