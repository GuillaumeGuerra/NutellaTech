'use strict';

var patchesServices = angular.module('patchesServices', ['ngResource']);

patchesServices.factory('Patches', ['$resource',
  function ($resource) {
      return $resource('/api/patches/:patchVersion', { patchVersion: '@version' }, {
          query: { method: 'GET', params: {}, isArray: true }
      });
  }]);