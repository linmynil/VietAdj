(function () {
    appModule.controller('tenant.views.OldList.updateCauseModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.cause', 'item',
        function ($scope, $uibModalInstance, causeService, item) {
            var vm = this;
            vm.item = item;

            vm.saving = false;

            vm.update = function () {
                console.log('update', vm.item);
                vm.saving = true;
                causeService.update(vm.item.id, vm.item.name).then(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();