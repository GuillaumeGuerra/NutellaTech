(function () {
    'use strict';

    angular
        .module('PatchManager')
        .service('PatchManagerContext', function () {
            var context =
                {
                    settings:
                        {
                            viewTypeIcon: "view_module",
                            viewType: "Card",
                            showCards: false,
                            mode: "Patches Gathering",
                            showRejected: false,
                            hideNonReadyGerrits: false,
                            hideTestedGerrits: false,
                            hideNonMergedGerrits: true
                        }
                };
            return context;
        });

})();