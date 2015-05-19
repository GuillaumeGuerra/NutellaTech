(function () {

    angular
        .module('users', ['n3-charts.linechart','highcharts-ng'])
        .config(function ($mdIconProvider) {
            $mdIconProvider
                .icon('share-arrow', 'img/icons/share-arrow.svg', 24)
                .icon('upload', 'img/icons/upload.svg', 24)
                .icon('copy', 'img/icons/copy.svg', 24)
                .icon('print', 'img/icons/print.svg', 24)
                .icon('hangout', 'img/icons/hangout.svg', 24)
                .icon('mail', 'img/icons/mail.svg', 24)
                .icon('message', 'img/icons/message.svg', 24)
                .icon('copy2', 'img/icons/copy2.svg', 24)
                .icon('facebook', 'img/icons/facebook.svg', 24)
                .icon('twitter', 'img/icons/twitter.svg', 24);
        })
        .controller('UserController', [
            'userService', '$scope', '$mdSidenav', '$mdBottomSheet', '$log', '$q',
            UserController
        ])
        .controller('ListBottomSheetCtrl', function ($scope, $mdBottomSheet) {
            $scope.items = [
                {name: 'Share', icon: 'share-arrow'},
                {name: 'Upload', icon: 'upload'},
                {name: 'Copy', icon: 'copy'},
                {name: 'Print this page', icon: 'print'},
            ];
            $scope.listItemClick = function ($index) {
                var clickedItem = $scope.items[$index];
                $mdBottomSheet.hide(clickedItem);
            };
        })
        .controller('GridBottomSheetCtrl', function ($scope, $mdBottomSheet) {
            $scope.items = [
                {name: 'Hangout', icon: 'hangout'},
                {name: 'Mail', icon: 'mail'},
                {name: 'Message', icon: 'message'},
                {name: 'Copy', icon: 'copy2'},
                {name: 'Facebook', icon: 'facebook'},
                {name: 'Twitter', icon: 'twitter'},
            ];
            $scope.listItemClick = function ($index) {
                var clickedItem = $scope.items[$index];
                $mdBottomSheet.hide(clickedItem);
            };
        });

    /**
     * Main Controller for the Angular Material Starter App
     * @param $scope
     * @param $mdSidenav
     * @param avatarsService
     * @constructor
     */
    function UserController(userService, $scope, $mdSidenav, $mdBottomSheet, $log, $q) {
        var self = this;

        self.selected = null;
        self.users = [];
        self.selectUser = selectUser;
        self.toggleList = toggleUsersList;
        self.showContactOptions = showContactOptions;
        $scope.profiles = [
            {name: 'You Are An Exporter', selected: true},
            {name: 'You Are An Importer', selected: false}
        ];
        $scope.currencies = [
            {header: 'Your Expenses', currency: 'EUR'},
            {header: 'Your Incomes', currency: 'USD'},
        ];

        $scope.strategies = [
            {name: 'None', img: 'https://material.angularjs.org/img/100-0.jpeg', newMessage: true},
            {name: 'Lock', img: 'https://material.angularjs.org/img/100-1.jpeg', newMessage: false},
            {name: 'Agreed Rate', img: 'https://material.angularjs.org/img/100-2.jpeg', newMessage: false},
            {name: 'Optional', img: 'https://material.angularjs.org/img/100-2.jpeg', newMessage: false}
        ];

        $scope.data = [
            {x: 0, y: 0, other_y: 0, val_2: 0, val_3: 0},
            {x: 1, y: 0.993, other_y: 3.894, val_2: 8.47, val_3: 14.347},
            {x: 2, y: 1.947, other_y: 7.174, val_2: 13.981, val_3: 19.991},
            {x: 3, y: 2.823, other_y: 9.32, val_2: 14.608, val_3: 13.509},
            {x: 4, y: 3.587, other_y: 9.996, val_2: 10.132, val_3: -1.167},
            {x: 5, y: 4.207, other_y: 9.093, val_2: 2.117, val_3: -15.136},
            {x: 6, y: 4.66, other_y: 6.755, val_2: -6.638, val_3: -19.923},
            {x: 7, y: 4.927, other_y: 3.35, val_2: -13.074, val_3: -12.625},
            {x: 8, y: 4.998, other_y: -0.584, val_2: -14.942, val_3: 2.331},
            {x: 9, y: 4.869, other_y: -4.425, val_2: -11.591, val_3: 15.873},
            {x: 10, y: 4.546, other_y: -7.568, val_2: -4.191, val_3: 19.787},
            {x: 11, y: 4.042, other_y: -9.516, val_2: 4.673, val_3: 11.698},
            {x: 12, y: 3.377, other_y: -9.962, val_2: 11.905, val_3: -3.487},
            {x: 13, y: 2.578, other_y: -8.835, val_2: 14.978, val_3: -16.557}
        ];

        $scope.options = {
            axes: {y2: {min: -15, max: 15}}, series: [
                {y: 'val_2', label: 'One', type: 'area', striped: true},
                {y: 'y', type: 'area', striped: true, label: 'Two'},
                {y: 'other_y', type: 'area', label: 'Three', striped: true, axis: 'y2'}
            ],
            lineMode: 'cardinal',
            tooltip: {mode: 'scrubber'},
            margin: {
                bottom: 100
            }
        };

        $scope.chartConfig = {

            options: {
                //This is the Main Highcharts chart config. Any Highchart options are valid here.
                //will be overriden by values specified below.
                chart: {
                    type: 'bar'
                },
                tooltip: {
                    style: {
                        padding: 10,
                        fontWeight: 'bold'
                    }
                }
            },
            //The below properties are watched separately for changes.

            //Series object (optional) - a list of series using normal highcharts series options.
            series: [{
                data: [10, 15, 12, 8, 7]
            }],
            //Title configuration (optional)
            title: {
                text: 'Hello'
            },
            //Boolean to control showng loading status on chart (optional)
            //Could be a string if you want to show specific loading text.
            loading: false,
            //Configuration for the xAxis (optional). Currently only one x axis can be dynamically controlled.
            //properties currentMin and currentMax provied 2-way binding to the chart's maximimum and minimum
            xAxis: {
                currentMin: 0,
                currentMax: 20,
                title: {text: 'values'}
            },
            //Whether to use HighStocks instead of HighCharts (optional). Defaults to false.
            useHighStocks: false,
            //size (optional) if left out the chart will default to size of the div or something sensible.
            size: {
                width: 400,
                height: 300
            },
            //function (optional)
            func: function (chart) {
                //setup some logic for the chart
            }
        };

        $scope.showListBottomSheet = function ($event) {
            $scope.alert = '';
            $mdBottomSheet.show({
                templateUrl: 'src/users/bottom-sheet-list-template.html',
                controller: 'ListBottomSheetCtrl',
                targetEvent: $event
            }).then(function (clickedItem) {
                $scope.alert = clickedItem.name + ' clicked!';
            });
        };
        $scope.showGridBottomSheet = function ($event) {
            $scope.alert = '';
            $mdBottomSheet.show({
                templateUrl: 'src/users/bottom-sheet-grid-template.html',
                controller: 'GridBottomSheetCtrl',
                targetEvent: $event
            }).then(function (clickedItem) {
                $scope.alert = clickedItem.name + ' clicked!';
            });
        };

        // Load all registered users

        userService
            .loadAllUsers()
            .then(function (users) {
                self.users = [].concat(users);
                self.selected = users[0];
            });

        // *********************************
        // Internal methods
        // *********************************

        /**
         * First hide the bottomsheet IF visible, then
         * hide or Show the 'left' sideNav area
         */
        function toggleUsersList() {
            var pending = $mdBottomSheet.hide() || $q.when(true);

            pending.then(function () {
                $mdSidenav('left').toggle();
            });
        }

        /**
         * Select the current avatars
         * @param menuId
         */
        function selectUser(user) {
            self.selected = angular.isNumber(user) ? $scope.users[user] : user;
            self.toggleList();
        }

        /**
         * Show the bottom sheet
         */
        function showContactOptions($event) {
            var user = self.selected;

            $mdBottomSheet.show({
                parent: angular.element(document.getElementById('content')),
                templateUrl: 'src/users/view/contactSheet.html',
                controller: ['$mdBottomSheet', ContactPanelController],
                controllerAs: "cp",
                bindToController: true,
                targetEvent: $event
            }).then(function (clickedItem) {
                clickedItem && $log.debug(clickedItem.name + ' clicked!');
            });

            /**
             * Bottom Sheet controller for the Avatar Actions
             */
            function ContactPanelController($mdBottomSheet) {
                this.user = user;
                this.actions = [
                    {name: 'Phone', icon: 'phone', icon_url: 'assets/svg/phone.svg'},
                    {name: 'Twitter', icon: 'twitter', icon_url: 'assets/svg/twitter.svg'},
                    {name: 'Google+', icon: 'google_plus', icon_url: 'assets/svg/google_plus.svg'},
                    {name: 'Hangout', icon: 'hangouts', icon_url: 'assets/svg/hangouts.svg'}
                ];
                this.contactUser = function (action) {
                    $mdBottomSheet.hide(action);
                };
            }
        }

    }

})();
