(function () {
    'use strict';

    angular.module('app').controller('OrderController', OrderController);

    function OrderController($http) {
        //alert('controller initializing'); 
        var vm = this;
        var dataService = $http;

        ////vm.test = "Test";
        //vm.removeTask = removeTask;
        //vm.tasks = [{ id: 1, 'name': 'test1' }, { id: 2, 'name': 'test2' }, { id: 3, 'name': 'test3' }];

        // Hook up public events
        vm.resetSearch = resetSearch;
        vm.search = search;
        vm.orderGetDetails = orderGetDetails;
        vm.backToCustomer = backToCustomer;

        vm.customer = {};
        vm.customers = [];
        vm.searchCustomers = [];
        vm.order = {};
        vm.orders = [];
        vm.searchOrders = [];
        vm.orderdetail = {};
        vm.orderdetails = [];

        vm.searchInput = {
            customerName: '',
            orderDateFrom: '',
            orderDateTo: '',
            shipDateFrom: '',
            shipDateTo: '',
            dueDateFrom: '',
            dueDateTo: ''
        };
        //customerName: 'John Doe',
        //orderDateFrom: '1900-01-01',
        //orderDateTo: '2099-12-31',
        //shipDateFrom: '1900-01-01',
        //shipDateTo: '2099-12-31',
        //dueDateFrom: '1900-01-01',
        //dueDateTo: '2099-12-31'


        const pageMode = {
            LIST: 'List',
            DETAIL: 'Detail'
        };

        vm.uiState = {
            mode: pageMode.LIST,
            isListAreaVisible: true,
            isSearchAreaVisible: true,
            isDetailAreaVisible: false,
            isValid: true,
            messages: []
        };

        function setUIState(state) {
            vm.uiState.mode = state;

            vm.uiState.isListAreaVisible = (state == pageMode.LIST);
            vm.uiState.isSearchAreaVisible = (state == pageMode.LIST);

            vm.uiState.isDetailAreaVisible = (state == pageMode.DETAIL);

        }

        function addValidationMessage(prop, msg) {
            vm.uiState.messages.push({
                property: prop,
                message: msg
            });
        }

        function validateData() {
            // Fix up date (IE 11 bug workaround)
            vm.searchInput.orderDateFrom = vm.searchInput.orderDateFrom.replace(/\u200E/g, '');
            vm.searchInput.orderDateTo = vm.searchInput.orderDateTo.replace(/\u200E/g, '');
            vm.searchInput.dueDateFrom = vm.searchInput.dueDateFrom.replace(/\u200E/g, '');
            vm.searchInput.dueDateTo = vm.searchInput.dueDateTo.replace(/\u200E/g, '');
            vm.searchInput.shipDateFrom = vm.searchInput.shipDateFrom.replace(/\u200E/g, '');
            vm.searchInput.shipDateTo = vm.searchInput.shipDateTo.replace(/\u200E/g, '');

            vm.uiState.messages = [];

            if (vm.searchInput.orderDateFrom != null) {
                if (isNaN(Date.parse(vm.searchInput.orderDateFrom))) {
                    addValidationMessage('Order Date From', 'Invalid Order Date From');
                }
            }
            if (vm.searchInput.orderDateTo != null) {
                if (isNaN(Date.parse(vm.searchInput.orderDateTo))) {
                    addValidationMessage('Order Date To', 'Invalid Order Date To');
                }
            }
            if (vm.searchInput.dueDateFrom != null) {
                if (isNaN(Date.parse(vm.searchInput.dueDateFrom))) {
                    addValidationMessage('Due Date From', 'Invalid Due Date From');
                }
            }
            if (vm.searchInput.dueDateTo != null) {
                if (isNaN(Date.parse(vm.searchInput.dueDateTo))) {
                    addValidationMessage('Due Date To', 'Invalid Due Date To');
                }
            }
            if (vm.searchInput.shipDateFrom != null) {
                if (isNaN(Date.parse(vm.searchInput.shipDateFrom))) {
                    addValidationMessage('Ship Date From', 'Invalid Ship Date From');
                }
            }
            if (vm.searchInput.shipDateTo != null) {
                if (isNaN(Date.parse(vm.searchInput.shipDateTo))) {
                    addValidationMessage('Ship Date To', 'Invalid Ship Date To');
                }
            }

            vm.uiState.isValid = (vm.uiState.messages.length == 0);

            return vm.uiState.isValid;
        }

        function search() {
            // Create object literal for search values
            var searchEntity = {
                CustomerName: vm.searchInput.customerName,
                OrderDateFrom: vm.searchInput.orderDateFrom,
                OrderDateTo: vm.searchInput.orderDateTo,
                ShipDateFrom: vm.searchInput.shipDateFrom,
                ShipDateTo: vm.searchInput.shipDateTo,
                DueDateFrom: vm.searchInput.dueDateFrom,
                DueDateTo: vm.searchInput.dueDateTo
            };

            // Call Web API to get a list of Orders
            dataService.post("/api/Order/Search",
                    searchEntity)
              .then(function (result) {
                  vm.orders = result.data;

                  setUIState(pageMode.LIST);
              }, function (error) {
                  handleException(error);
              });
        }


        function orderGetDetails(salesOrderNumber) {
            var searchEntity = {
                SalesOrderNumber: salesOrderNumber
            };
            // Call Web API to get order details
            dataService.post("/api/Order/OrderDetails",
                    searchEntity)
              .then(function (result) {
                  vm.orderdetails = result.data;

                  setUIState(pageMode.DETAIL);
              }, function (error) {
                  handleException(error);
              });
        }

        function backToCustomer() {

            setUIState(pageMode.LIST);
        }

        //function removeTask(taskId) {
        //    alert("Task Id is " + taskId);
        //}

        function resetSearch() {
            vm.searchInput = {
                customerName: '',
                orderDateFrom: '',
                orderDateTo: '',
                shipDateFrom: '',
                shipDateTo: '',
                dueDateFrom: '',
                dueDateTo: ''
            };

            //setUIState(pageMode.LIST);    // remove the list
            //customerList();
        }

        function searchCustomersList() {
            dataService.get("/api/Order/GetSearchCustomers")
            .then(function (result) {
                vm.searchCustomers = result.data;
            },
            function (error) {
                handleException(error);
            });
        }


        function handleException(error) {
            vm.uiState.isValid = false;
            var msg = {
                property: 'Error',
                message: ''
            };

            vm.uiState.messages = [];

            switch (error.status) {
                case 400:   // 'Bad Request'
                    // Model state errors
                    var errors = error.data.ModelState;
                    debugger;

                    // Loop through and get all 
                    // validation errors
                    for (var key in errors) {
                        for (var i = 0; i < errors[key].length;
                                i++) {
                            addValidationMessage(key,
                                        errors[key][i]);
                        }
                    }

                    break;

                case 404:  // 'Not Found'
                    msg.message = 'The product you were ' +
                                  'requesting could not be found';
                    vm.uiState.messages.push(msg);

                    break;

                case 500:  // 'Internal Error'
                    msg.message =
                      error.data.ExceptionMessage;
                    vm.uiState.messages.push(msg);

                    break;

                default:
                    msg.message = 'Status: ' +
                                error.status +
                                ' - Error Message: ' +
                                error.statusText;
                    vm.uiState.messages.push(msg);

                    break;
            }
        }
    }
})();