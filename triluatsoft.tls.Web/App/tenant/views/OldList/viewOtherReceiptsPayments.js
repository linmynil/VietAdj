(function () {
    appModule.controller('tenant.views.OldList.viewOtherReceiptsPayments', [
        '$scope',
        '$uibModal',
        'abp.services.app.cash',
        function (
            $scope,
            $uibModal,
            cashService
            ) {
            var vm = this;
            vm.page = 0;

            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');
                $('#from').daterangepicker({
                    singleDatePicker: true,
                    autoUpdateInput: false,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                });
                $('#from').on('apply.daterangepicker', function (ev, picker) {
                    $('#from').val(picker.startDate.format('DD-MM-YYYY'));
                });


                $('#to').daterangepicker({
                    singleDatePicker: true,
                    autoUpdateInput: false,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                });
                $('#to').on('apply.daterangepicker', function (ev, picker) {
                    $('#to').val(picker.startDate.format('DD-MM-YYYY'));
                });
            });

            vm.getAll = function () {
                vm.loading = true;
                let page = Math.max(vm.page - 1, 0);//because of server index page from zero
                let postData = $.extend({}, vm.searchdata, { page: page });


                let from = $('#from').val() !== '' ? moment($('#from').val(), "DD-MM-YYYY").format() : undefined;
                let to = $('#to').val() !== '' ? moment($('#to').val(), "DD-MM-YYYY").format() : undefined;
                postData.fromDate = from;
                postData.toDate = to;
                console.log('request params', postData);
                cashService.search(postData)
                    .then(function (result) {
                        console.log('getall', result.data);
                        vm.list = result.data.items;
                        vm.totalCount = result.data.totalCount;
                        vm.sumValue = result.data.sumValue;
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

            vm.init = function () {
                vm.getAll();
            }

            vm.init();

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 0;
            }

           
            //modal history
            vm.openModalHistoryPayment = function (id) {
                var modalInstance = $uibModal.open
                    ({
                        templateUrl: '~/App/tenant/views/OldList/modalHistory.cshtml',
                        controller: 'tenant.views.OldList.modalHistory as vm',
                        scope: $scope,
                        backdrop: 'static',
                        size: 'lg',
                        resolve: {
                            id: function () {
                                return id;
                            }
                        }
                    });
            }
            //modal Create
            vm.openModal = function (id) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalOtherReceipts.cshtml',
                    controller: 'tenant.views.OldList.modalOtherReceipts as vm',
                    backdrop: 'static',
                    scope: $scope,
                    resolve: {
                        id: function () {
                            return id;
                        }
                    }
                });
                modalInstance.result.then(function () {
                });
            };

          //END

            //Message Delete
            vm.deletePayment = function (id) {
                abp.message.confirm('AreYouSureDeleteInformation?',
                    function (isConfirmed) {
                        if (isConfirmed) {
                            cashService.deleteCash(id)
                                .then(function (result) {
                                    if (result.data == 'ok') {
                                        abp.notify.info('Receipt/Payment is deleted successfully');
                                        vm.getAll();
                                    }
                                });
                        }
                    });
            }

        }
    ]);
})();