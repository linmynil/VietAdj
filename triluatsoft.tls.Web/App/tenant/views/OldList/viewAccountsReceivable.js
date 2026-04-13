(function () {
    appModule.controller('tenant.views.OldList.viewAccountsReceivable', [
        '$scope',
        '$uibModal',
        'abp.services.app.aR',
        'abp.services.app.customer',
        function (
            $scope,
            $uibModal,
            arService,
            customerService
            ) {
            var vm = this;
            vm.list = [];
            vm.totalCount = 0;
            vm.page = 0;
            vm.searchdata = {};


            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {
            };

            vm.getAll = function () {
                vm.loading = true;
                let page = Math.max(vm.page - 1, 0);//because of server index page from zero
                let postData = $.extend({}, vm.searchdata, { page: page });

                if (vm.dateRangeModel) {
                    postData.fromDate = vm.dateRangeModel.startDate;
                    postData.toDate = vm.dateRangeModel.endDate;
                }

                console.log('request params', postData);
                arService.search(postData)
                    .then(function (result) {
                        console.log('getall', result.data);
                        vm.list = result.data.items;
                        vm.totalCount = result.data.totalCount;
                        vm.sumValue = result.data.sumValue;
                        vm.subTotal = result.data.subTotal;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });
            };

            vm.pageChanged = function () {
                vm.getAll();
            };

            vm.searchLogs = function (event) {
                try {
                    vm.getAll();
                } catch (e) {
                    console.log(e)
                } finally {
                    event.preventDefault();
                }
            }

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 0;
            }

            vm.getCustomers = function () {
                customerService.getByType('C')
                    .then(function (result) {
                        vm.listInsureres = result.data;
                    });
                customerService.getByType('B')
                    .then(function (result) {
                        vm.listBrokers = result.data;
                    });
            };

            //Modal DetailInvoiceNo
            vm.openModalDetailInvoiceNo = function (invoiceID) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalInvoiceDetail.cshtml',
                    controller: 'tenant.views.OldList.modalInvoiceDetail as vm',
                    backdrop: 'static',
                    size: 'lg',
                    scope: $scope,
                    resolve: {
                        invoiceID: function () {
                            return invoiceID;
                        }
                    }
                });
                modalInstance.result.then(function (result) {

                });
            }
            
            //Modal Create Payment
            vm.openModalCreatePayment = function (item) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalCreatePayment.cshtml',
                    controller: 'tenant.views.OldList.modalCreatePayment as vm',
                    backdrop: 'static',
                    scope: $scope,
                    resolve: {
                        item: function () {
                            return item;
                        }
                    }
                });
                modalInstance.result.then(function (result) {
                    
                });
            }
            
            //Modal View List Payment
            vm.openModalViewListPayment = function (claimID) {
                console.log('openModalViewListPayment');
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalViewListlPayment.cshtml',
                    controller: 'tenant.views.OldList.modalViewListlPayment as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        claimID: function () {
                            return claimID;
                        }
                    }
                });
                modalInstance.result.then(function (result) {
                    
                });
            }


            vm.init = function () {
                vm.getCustomers();
                vm.getAll();
            }

            vm.init();
        }
    ]);
})();