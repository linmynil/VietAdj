(function () {
    appModule.controller('tenant.views.OldList.modalClaimFoldersFileSub', [
        '$scope',
        '$uibModalInstance',
        '$uibModal',
        'abp.services.app.claimFolderService',
        'temp',
        function (
            $scope,
            $uibModalInstance,
            $uibModal,
            claimFolderService,
            temp
        ) {
            var vm = this;
            vm.saving = false;
            vm.list = [];
            vm.searchdata = {};
            vm.item = temp;
            vm.temp = {};
            vm.save = function () {
                $uibModalInstance.dismiss();
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            vm.totalCount = 0;
            vm.page = 1;

            vm.getAll = function () {
                debugger
                claimFolderService.getFilesList(vm.item)
                    .then(function (result) {
                        vm.list = result.data;
                        console.log('list', vm.list);
                        vm.totalCount = result.data.totalCount;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });

            };

            //vm.all = function () {
            //    vm.searchdata.claimID = "";
            //    vm.getAll();
            //}

            //vm.pageChanged = function () {
            //    vm.getAll();
            //};

            function init() {
                vm.getAll();
            }
            init();

            //modal claimFolder
            vm.openModalClaimFolder = function (temp) {
                debugger
                vm.temp = vm.item + temp;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalClaimFoldersFiles.cshtml',
                    controller: 'tenant.views.OldList.modalClaimFoldersFiles as vm',
                    backdrop: 'static',
                });

                modalInstance.result.then(function (result) {
                    refeshDatatable();
                });
            }
        }
    ]);
})();