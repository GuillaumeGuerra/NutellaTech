
'use strict';

var app = angular.module('PatchManager', [
    // Angular modules 
    'ngRoute',
    'ngMaterial',

    // Custom modules 
    'releasesServices',
    'patchesServices'

    // 3rd Party Modules

]);

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/home', {
            templateUrl: 'app/partials/home.html'
        }).
        otherwise({
            redirectTo: '/home'
        });
  }]);
