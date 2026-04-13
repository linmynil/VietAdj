(function () {
    appModule.controller('tenant.views.OldList.createOrUpdateExpenseTypeModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.expenseType', 'item',
        function ($scope, $uibModalInstance, expenseTypeService, item) {
            var vm = this;

            vm.saving = false;
            vm.item = item;
            vm.editing = vm.item ? true : false;

            vm.save = function () {
                console.log('create', vm.item);
                vm.saving = true;
                expenseTypeService.createOrUpdate(vm.item).then(function () {
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