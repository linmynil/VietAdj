(function () {
    appModule.controller('tenant.views.OldList.viewInvoiceManage', [
        '$scope',
        '$uibModal',
        'abp.services.app.invoice',
        'abp.services.app.customer',
        'abp.services.app.timeSheet',
        function (
            $scope,
            $uibModal,
            invoiceService,
            customerService,
            timesheetService
            ) {
            var vm = this;
            vm.list = [];
            vm.totalCount = 0;
            vm.page = 0;

            vm.grant_create;

            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {
            };

            vm.check_invoice_permission = function () {
                console.log('Check permission for create invoice');
                timesheetService.checkPermission_InvoiceTimeSheet()
                .then(function (result) {
                    vm.grant_create = result.data;
                    console.log('Create Permission', vm.grant_create);
                });
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
                invoiceService.search(postData)
                    .then(function (result) {
                        console.log('getall', result.data);
                        vm.list = result.data.items;
                        vm.totalCount = result.data.totalCount;
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

            vm.getCustomers = function () {
                customerService.getByType('C')
                    .then(function (result) {
                        vm.listInsureres = result.data;
                    });
            };

            vm.init = function () {
                vm.check_invoice_permission();
                vm.getCustomers();
                vm.getAll();

            }
            vm.init();

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 0;
            }
            
            
            //Modal Index ModalIndexInvoice
            vm.openModalCreateInvoice = function (input) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalIndexCreateInvoice.cshtml',
                    controller: 'tenant.views.OldList.modalIndexCreateInvoice as vm',
                    backdrop: 'static',
                    size: 'lg',
                    scope: $scope,
                    resolve: {
                        input: function () {
                            return input;
                        }
                    }
                });
                modalInstance.result.then(function (result) {
                    
                });
            }

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

        }
    ]);
})();