(function () {
    appModule.controller('tenant.views.OldList.viewOtherWorks', [
        '$scope',
        '$uibModal',
        'abp.services.app.employee',
        'abp.services.app.contributionAdjustment',
        function (
            $scope,
            $uibModal,
            employeeService,
            contributionAdjustmentService
            ) {
            var vm = this;
            vm.page = 1;            

            vm.searchdata = {};
            vm.searchdata.debitOrCredit = "";

            vm.getAll = function () {
                console.log('request params', $.extend({}, vm.searchdata, vm.dateRangeModel, { page: vm.page }));
                vm.loading = true;
                contributionAdjustmentService.getAll($.extend({}, vm.searchdata, vm.dateRangeModel, { page: vm.page }))
                    .then(function (result) {
                        vm.list = result.data.items;
                        vm.list.length = vm.list.length;
                        console.log('list', vm.list);
                        vm.totalCount = result.data.totalCount;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });

            };

            vm.pageChanged = function () {
                vm.getAll();
            };

            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = { };

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 1;
            }

            vm.init = function() {
                vm.getAll();
                vm.getEmployees();
            }

            vm.getEmployees = function () {
                console.log('getemployees');
                employeeService.getEmployeesForCRS({})
                    .then(function (result) {
                        vm.listEmployees = result.data;
                    });
            };

            vm.openModal = function (itemIn) {
                console.log('open modalCreateDeadline', itemIn);
                vm.item = itemIn;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalCreateOtherWorks.cshtml',
                    controller: 'tenant.views.OldList.modalCreateOtherWorks as vm',
                    backdrop: 'static',
                    scope: $scope,
                    resolve: {
                        item: function () {                            
                            return vm.item;
                        }
                    }
                });
                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };
            
            vm.deleteOtherWorks = function (owId) {
                abp.message.confirm('AreYouSureDeleteInformation?',
                    function (isConfirmed) {
                        if (isConfirmed) {

                            console.log('delete timesheetId', owId);
                            contributionAdjustmentService.deleteById(owId)
                                .then(function (result) {
                                    abp.notify.info(result.data);
                                })
                                .then(function () {
                                    vm.getAll();
                                });
                        }
                    });
            }

            vm.search = function (event) {
                try {
                    vm.getAll();
                } catch (e) {
                    console.log(e)
                } finally {
                    event.preventDefault();
                }
            }

            vm.init();
            //END
        }
    ]);
})();