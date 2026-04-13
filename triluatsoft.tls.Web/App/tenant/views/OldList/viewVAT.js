(function () {
    appModule.controller('tenant.views.OldList.viewVAT', [
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
            vm.page = 0;


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
                        vm.totalTax = result.data.totalTax;
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