'use strict';

var patchesServices = angular.module('patchesServices', ['ngResource']);

patchesServices.factory('Patches', ['$resource',
  function ($resource) {
      return $resource(
          '/api/releases/:releaseVersion/patches/:patchId/:isAction/:action',
          { releaseVersion: '@version', patchId: '@gerrit.id', isAction: '', action: '' },
          {
              query: { method: 'GET', params: {}, isArray: true },
              action: { method: 'POST', params: { isAction: 'action'} },
          });
  }]);