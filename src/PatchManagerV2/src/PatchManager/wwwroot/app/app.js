
'use strict';

// get ag-Grid to create an Angular module and register the ag-Grid directive
agGrid.initialiseAgGridWithAngular1(angular);

var app = angular.module('PatchManager', [
    // Angular modules 
    'ngRoute',
    'ngMaterial',

    // Custom modules 
    'releasesServices',
    'patchesServices',

    // 3rd Party Modules
    'agGrid'
]);


angular
    .module('PatchManager')
    .config(function ($mdThemingProvider) {
        $mdThemingProvider.theme('docs-dark', 'default').primaryPalette('grey').dark();
        $mdThemingProvider.theme('dark-grey').backgroundPalette('grey').dark();
        $mdThemingProvider.theme('dark-blue').backgroundPalette('blue').dark();
    });

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
