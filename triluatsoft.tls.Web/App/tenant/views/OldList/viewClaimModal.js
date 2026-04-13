(function () {
    appModule.controller('tenant.views.OldList.viewClaimModal', [
        '$scope', '$uibModalInstance', 'itemId', 'abp.services.app.claim', 
        function ($scope, $uibModalInstance, itemId, claimService) {
            var vm = this;
            console.log(itemId);
            vm.itemId = itemId;
            vm.item = {};

            vm.getAll = function () {
                console.log('request params', $.extend({}, { claimRefId: vm.itemId }, { page: 1 }));
                vm.loading = true;
                claimService.getById(vm.itemId)
                    .then(function (result) {
                        console.log('result data', result.data);
                        vm.item = result.data;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });

            };
            vm.getAll();


            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();