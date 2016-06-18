(function () {
    'use strict';

    angular
        .module('PatchManager')
        .service('PatchManagerContext', function () {
            var context =
                {
                    settings:
                        {
                            viewTypeIcon: "view_headline",
                            viewType: "Cards",
                            showGrid: false,
                            cardsTheme: "default",
                            mode: "Patches Gathering",
                            showRejected: false,
                            hideNonReadyGerrits: false,
                            hideTestedGerrits: false,
                            hideNonMergedGerrits: true,

                            switchView: function (showGrid) {
                                this.showGrid = showGrid;
                                this.viewType = showGrid ? "Grid" : "Cards";
                                this.viewTypeIcon = showGrid ? "view_headline" : "view_module";
                            }
                        }
                };
            return context;
        });

})();