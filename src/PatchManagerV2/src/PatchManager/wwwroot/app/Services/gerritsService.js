'use strict';

var gerritsServices = angular.module('gerritsServices', ['ngResource']);

gerritsServices.factory('Gerrits', ['$resource',
  function ($resource) {
      return $resource(
          '/api/patches/:patchVersion/gerrits/:gerritId/:action/:actionType',
          { patchVersion: '@version', gerritId: '@id', action: '', actionType: '' },
          { query: { method: 'GET', params: {}, isArray: true } });
  }]);