'use strict';

var patchesServices = angular.module('patchesServices', ['ngResource']);

patchesServices.factory('Patches', ['$resource',
  function ($resource) {
      return $resource('/api/patches/:patchVersion', { version: '@Version' }, {
          query: { method: 'GET', params: {}, isArray: true }
      });
  }]);