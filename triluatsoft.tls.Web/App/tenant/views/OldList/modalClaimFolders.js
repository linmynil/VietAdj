(function () {
    appModule.controller('tenant.views.OldList.modalClaimFolders', [
        '$scope',
        '$uibModalInstance',
        '$uibModal',
        'claims',
        function (
            $scope,
            $uibModalInstance,
            $uibModal,
            claims
        ) {
            var vm = this;
            vm.saving = false;
            vm.item = claims;
            vm.save = function () {};

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {
                console.log('Data: ', claims);
                vm.item.data = vm.item.id;
            }
            init();

            //modal claimFolderFile
            vm.openModalClaimFolderFile = function () {
                debugger
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalClaimFoldersFiles.cshtml',
                    controller: 'tenant.views.OldList.modalClaimFoldersFiles as vm',
                    backdrop: 'static',
                    size: 'lg'
                });

                modalInstance.result.then(function (result) {
                    refeshDatatable();
                });
            }
            
        }
    ]);
})();