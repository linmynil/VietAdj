(function () {
    appModule.controller('tenant.views.OldList.customer', [
        '$scope', '$uibModal','abp.services.app.customer',
        function ($scope, $uibModal, customerService) {
            var vm = this;
            vm.recordType = "";

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.list = [];

            vm.selectedType = function () {
                vm.getAll();
            };

            vm.searchLog = function (event) {
                try {
                    vm.getAll();
                } catch (e) {
                    console.log(e)
                } finally {
                    event.preventDefault();
                }
            }

            vm.getAll = function () {
                try {                    
                    vm.loading = true;
                        if (!vm.searchdata) {
                            customerService.getAll()
                                .then(function (result) {
                                    vm.list = result.data;
                                    console.log('list', vm.searchdata, vm.list);
                                    vm.loading = false;
                                });

                        } else {
                            console.log(vm.searchdata);
                            customerService.search(vm.searchdata)
                                .then(function (result) {
                                    vm.list = result.data;
                                    console.log('list', vm.searchdata, vm.list);
                                    vm.loading = false;
                                });
                        }
                } catch (e) {
                    console.log('customer search error', e);
                } finally {
                    vm.loading = false;
                }
            };
            vm.getAll();
            
            vm.openModal = function (itemIn, mode) {
                console.log('open createOrUpdateCustomerModal', itemIn, mode);
                vm.item = itemIn;
                vm.mode = mode;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/createOrUpdateCustomerModal.cshtml',
                    controller: 'tenant.views.OldList.createOrUpdateCustomerModal as vm',
                    backdrop: 'static',
                    resolve: {
                        item: function () {
                            return vm.item;
                        },
                        mode: function () {
                            return vm.mode;
                        },
                        recordType: function () {
                            return vm.recordType;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };

            vm.reset = function () {
                vm.searchdata = {};
            }

        }
    ]);
})();