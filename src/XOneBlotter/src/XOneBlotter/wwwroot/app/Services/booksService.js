//(function () {
//    'use strict';

//    angular
//        .module('app')
//        .factory('booksService', booksService);

//    booksService.$inject = ['$http'];

//    function booksService($http) {
//        var service = {
//            getData: getData
//        };

//        return service;

//        function getData() { }
//    }
//})();

(function () {
    'use strict';
    var booksServices = angular.module('booksServices', ['ngResource']);

    booksServices.factory('Books', ['$resource',
      function ($resource) {
          return $resource('/api/books/:bookTitle', { bookTile: '@Title' }, {
              query: { method: 'GET', params: {}, isArray: true }
          });
      }]);

})();