(function () {
    appModule.controller('tenant.views.OldList.createOrUpdateFollowUpModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.followUp', 'item',
        function ($scope, $uibModalInstance, followUpService, item) {
            var vm = this;

            vm.saving = false;
            vm.item = item;
            vm.editing = vm.item ? true : false;

            vm.save = function () {
                console.log('create', vm.item);
                vm.saving = true;
                followUpService.createOrUpdate(vm.item).then(function () {
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