(function () {
    appModule.controller('tenant.views.OldList.createCauseModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.cause',
        function ($scope, $uibModalInstance, causeService) {
            var vm = this;

            vm.saving = false;
            vm.item = {};

            vm.save = function () {
                console.log('create', vm.item);
                vm.saving = true;
                causeService.create(vm.item.name).then(function () {
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