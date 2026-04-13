(function () {
    appModule.controller('tenant.views.OldList.modalHistory', [
        '$scope', '$uibModalInstance', 'abp.services.app.cash', 'id',
        function ($scope, $uibModalInstance, cashService, id) {
            var vm = this;
            vm.list = [];

            vm.getAll = function () {
                console.log('getAll', id);
                vm.loading = true;
                cashService.getHistories(id)
                    .then(function (result) {
                        vm.list = result.data;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {
                vm.getAll();
            }

            init();
        }
    ]);
})();