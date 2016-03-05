'use strict';

var releasesServices = angular.module('releasesServices', ['ngResource']);

releasesServices.factory('Releases', ['$resource',
  function ($resource) {
      return $resource('/api/patches/:releaseVersion', { releaseVersion: '@version' }, {
          query: { method: 'GET', params: {}, isArray: true }
      });
  }]);