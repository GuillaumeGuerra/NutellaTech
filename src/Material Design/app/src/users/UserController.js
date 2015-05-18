(function () {

    angular
        .module('users')
        .controller('UserController', [
            'userService', '$scope', '$mdSidenav', '$mdBottomSheet', '$log', '$q',
            UserController
        ]);

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
            { name: 'None', img: 'https://material.angularjs.org/img/100-0.jpeg', newMessage: true },
            { name: 'Lock', img: 'https://material.angularjs.org/img/100-1.jpeg', newMessage: false },
            { name: 'Agreed Rate', img: 'https://material.angularjs.org/img/100-2.jpeg', newMessage: false },
            { name: 'Optional', img: 'https://material.angularjs.org/img/100-2.jpeg', newMessage: false }
        ];

        $scope.toppings = [
            {name: 'Pepperoni', wanted: true},
            {name: 'Sausage', wanted: false},
            {name: 'Black Olives', wanted: true},
            {name: 'Green Peppers', wanted: false}
        ];
        $scope.settings = [
            { name: 'Wi-Fi', extraScreen: 'Wi-fi menu', icon: 'device:network-wifi', enabled: true },
            { name: 'Bluetooth', extraScreen: 'Bluetooth menu', icon: 'device:bluetooth', enabled: false }
        ];
        $scope.messages = [
            {id: 1, title: "Message A", selected: false},
            {id: 2, title: "Message B", selected: true},
            {id: 3, title: "Message C", selected: true},
        ];
        $scope.people = [
            { name: 'Janet Perkins', img: 'https://material.angularjs.org/img/100-0.jpeg', newMessage: true },
            { name: 'Mary Johnson', img: 'https://material.angularjs.org/img/100-1.jpeg', newMessage: false },
            { name: 'Peter Carlsson', img: 'https://material.angularjs.org/img/100-2.jpeg', newMessage: false }
        ];

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
