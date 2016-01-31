'use strict';

var booksServices = angular.module('booksServices', ['ngResource']);

booksServices.factory('Books', ['$resource',
  function ($resource) {
      return $resource('/api/books/:bookTitle', { bookTile: '@Title' }, {
          query: { method: 'GET', params: {}, isArray: true }
      });
  }]);