'use strict';

var gerritsServices = angular.module('gerritsServices', ['ngResource']);

gerritsServices.factory('Gerrits', ['$resource',
  function ($resource) {
      return $resource(
          '/api/releases/:releaseVersion/gerrits/:gerritId/:isAction/:action',
          { releaseVersion: '@version', gerritId: '@id', isAction: '', action: '' },
          {
              query: { method: 'GET', params: {}, isArray: true },
              action: { method: 'POST', params: { isAction: 'action'} },
          });
  }]);