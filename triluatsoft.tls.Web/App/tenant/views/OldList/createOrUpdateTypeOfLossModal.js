(function () {
    appModule.controller('tenant.views.OldList.createOrUpdateTypeOfLossModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.typeOfLoss', 'item',
        function ($scope, $uibModalInstance, typeOfLossService, item) {
            var vm = this;

            vm.saving = false;
            vm.item = item;

            vm.save = function () {
                console.log('create', vm.item);
                vm.saving = true;
                typeOfLossService.createOrUpdate(vm.item).then(function () {
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