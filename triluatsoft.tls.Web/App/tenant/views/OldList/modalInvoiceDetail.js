(function () {
    appModule.controller('tenant.views.OldList.modalInvoiceDetail', [
        '$scope', '$uibModalInstance', 'abp.services.app.invoice', 'invoiceID',
        function ($scope, $uibModalInstance, invoiceService, invoiceID) {
            var vm = this;
            vm.invoiceID = invoiceID;
            console.log('invoiceCode', invoiceID);
            vm.item = {};

            vm.getAll = function () {
                vm.loading = true;
                console.log('getAll ', vm.invoiceID);
                invoiceService.getInfo(vm.invoiceID)
                    .then(function (result) {
                        console.log('getAll data', result.data);
                        vm.item = result.data;
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