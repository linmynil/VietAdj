(function () {
    appModule.controller('tenant.views.OldList.modalDetailTimeSheet', [
        '$scope',
        '$uibModalInstance',
        'abp.services.app.cause',
        '$uibModal',
        'timesheet',
        'abp.services.app.timeSheet',
        function (
            $scope,
            $uibModalInstance,
            causeService,
            $uibModal,
            timesheet,
            timesheetService
            ) {
            var vm = this;

            vm.saving = false;
            vm.timesheet = timesheet;

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

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            vm.setExpType = function (item) {
                console.log('setExpType', item);
                $('#ExpButton').html(item + ' <span class="caret" />');
                if (item === 'A') {
                    $('#ExpButton').html('Amount <span class="caret" />');
                }
                else {
                    $('#ExpButton').html('Percent <span class="caret" />');
                }
                vm.timesheet.discountType = item;
            };

            vm.getAll = function () {
                vm.loading = true;
                console.log('timesheetService.getTSInfo', vm.timesheet);
                
                timesheetService.getTSInfo(vm.timesheet.tid)
                    .then(function (result) {
                        vm.timesheet = result.data;
                        console.log('timesheetService.getTSInfo result', vm.timesheet);
                        vm.setExpType(vm.timesheet.discountType);
                    })
                    .finally(function () {
                        vm.loading = false;
                    });
            };

            function init() {
                vm.getAll();
            }
            init();


        }
    ]);
})();