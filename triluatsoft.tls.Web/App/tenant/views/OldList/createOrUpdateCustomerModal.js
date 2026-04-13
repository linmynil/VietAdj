(function () {
    appModule.controller('tenant.views.OldList.createOrUpdateCustomerModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.customer', 'item', 'mode', 'recordType',
        function ($scope, $uibModalInstance, customerService, item, mode, recordType) {
            var vm = this;

            vm.saving = false;
            vm.item = item;
            vm.editing = mode === 'edit' ? true : false;
            vm.viewing = mode === 'view' ? true : false;
            //vm.item.recordType = recordType == '' ? 'C' : recordType;


            vm.save = function () {
                console.log('create', vm.item);
                vm.saving = true;
                //if (vm.item) {
                //    vm.item.recordType = recordType;
                //}
                customerService.createOrUpdate(vm.item).then(function () {
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
                if (!vm.item) {
                    vm.item = {};
                    vm.item.recordType = 'C';
                }
            }

            init();
        }
    ]);
})();