(function () {
    appModule.controller('tenant.views.OldList.modalAddTimeSheet', [
        '$scope', '$uibModalInstance', 'abp.services.app.claim',
        'abp.services.app.timeSheet', '$uibModal',
        function ($scope, $uibModalInstance, claimService, timesheetService, $uibModal) {
            var vm = this;

            vm.saving = false;
            vm.list = [];
            vm.item = {};
            
            vm.save = function () {
                console.log('save', vm.item);
                vm.saving = true;
                timesheetService.createTimesheet(vm.item)
                    .then(function (result) {
                        abp.message.success(result.data);
                        $uibModalInstance.close();
                    })
                    .finally(function () {
                        vm.saving = false;
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            vm.selectClaim = function (claimId) {
                console.log('select claimId', claimId);
                
                vm.loading = true;
                timesheetService.getNewTSSeq(claimId)
                    .then(function (result) {
                        vm.item.tSSeqNo = result.data;
                        vm.item.timeSheetName = claimId + ".TS" + result.data;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });
            }

            vm.getAll = function () {
                console.log('modal create timesheet getall');
                vm.loading = true;
                claimService.getOpenClaimToCreateTimesheet()
                    .then(function (result) {
                        vm.list = result.data;
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