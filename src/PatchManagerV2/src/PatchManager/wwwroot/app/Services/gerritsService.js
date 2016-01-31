'use strict';

var gerritsServices = angular.module('gerritsServices', ['ngResource']);

patchesServices.factory('Gerrits', ['$resource',
  function ($resource) {
      return $resource('/api/patches/:patchVersion/gerrits/:gerritId', {
          patchVersion: '@version',
          gerritId: '@id'
      }, {
          query: { method: 'GET', params: {}, isArray: true }
      });
  }]);