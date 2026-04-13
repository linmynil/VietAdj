(function () {
    appModule.controller('tenant.views.OldList.createOrUpdateReportModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.report', 'item',
        function ($scope, $uibModalInstance, reportService, item) {
            var vm = this;

            vm.saving = false;
            vm.item = item;
            vm.editing = vm.item ? true : false;

            vm.save = function () {
                console.log('create', vm.item);
                vm.saving = true;
                reportService.createOrUpdate(vm.item).then(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {

            }

            init();
        }
    ]);
})();