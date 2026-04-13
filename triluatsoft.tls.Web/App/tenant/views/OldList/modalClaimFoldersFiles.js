(function () {
    appModule.controller('tenant.views.OldList.modalClaimFoldersFiles', [
        '$scope',
        '$uibModalInstance',
        '$uibModal',
        'abp.services.app.claimFolderService',
        function (
            $scope,
            $uibModalInstance,
            $uibModal,
            claimFolderService
        ) {
            var vm = this;
            vm.saving = false;
            vm.list = [];
            vm.searchdata = {};
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
                console.log('request params', $.extend({}, vm.searchdata, { page: vm.page }));
                vm.loading = true;
                claimFolderService.getAll($.extend({}, vm.searchdata, { page: vm.page }))
                    .then(function (result) {
                        vm.list = result.data.items;
                        console.log('list', vm.list);
                        vm.totalCount = result.data.totalCount;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });

            };

            vm.all = function () {
                vm.searchdata.claimID = "";
                vm.getAll();
            }

            vm.pageChanged = function () {
                vm.getAll();
            };

            function init() {
                vm.getAll();
            }
            init();

            //modal claimFolder
            vm.openModalClaimFolder = function (item) {
                vm.item = item;                
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalClaimFoldersDetail.cshtml',
                    controller: 'tenant.views.OldList.modalClaimFoldersDetail as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        item: function () {
                            return vm.item;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    refeshDatatable();
                });
            }
        }
    ]);
})();