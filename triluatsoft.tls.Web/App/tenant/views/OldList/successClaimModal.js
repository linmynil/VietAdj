(function () {
    appModule.controller('tenant.views.OldList.successClaimModal', [
        '$scope', '$state', '$uibModalInstance', 'abp.services.app.cause', 'item', 'mode',
        function ($scope, $state, $uibModalInstance, causeService, item, mode) {
            var vm = this;
            vm.item = item;
            vm.mode = mode;

            vm.saving = false;

            vm.redirect = function () {
                console.log('redirect', vm.item);
                if (vm.mode == 'create') {
                    console.log('Create mode ', vm.item);
                    $state.go("tenant.claim", { 'claimRefId': vm.item });
                } else {
                    console.log('Edit mode ', vm.item);
                    $state.go("tenant.cuClaim", { 'claimId': vm.item });
                }               
                
                //causeService.update(vm.item.id, vm.item.name).then(function () {
                //    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                //}).finally(function () {
                //    vm.saving = false;
                //});
            };
            
        }
    ]);
})();