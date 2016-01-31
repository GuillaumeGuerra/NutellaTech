'use strict';

var gerritsServices = angular.module('gerritsServices', ['ngResource']);

patchesServices.factory('Gerrits', ['$resource',
  function ($resource) {
      return $resource('/api/patches/:patchVersion/gerrits/:gerritId/:preview', {
          patchVersion: '@version',
          gerritId: '@id',
          preview:''
      }, {
          query: { method: 'GET', params: {}, isArray: true }
      });
  }]);