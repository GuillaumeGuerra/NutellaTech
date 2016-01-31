'use strict';

var gerritsServices = angular.module('gerritsServices', ['ngResource']);

patchesServices.factory('Gerrits', ['$resource',
  function ($resource) {
      return $resource('/api/patches/:patchVersion/gerrits/:gerrit', {
          patchVersion: '@version',
          gerrit: '@gerrit'
      }, {
          query: { method: 'GET', params: {}, isArray: true }
      });
  }]);