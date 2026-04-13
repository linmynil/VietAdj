(function () {
    appModule.controller('tenant.views.OldList.modalClaimFoldersDetail', [
        '$scope',
        '$uibModalInstance',
        '$uibModal',
        'abp.services.app.claimFolderService',
        'item',        
        function (
            $scope,
            $uibModalInstance,
            $uibModal,
            claimFolderService,
            item            
        ) {
            var vm = this;
            vm.saving = false;
            vm.list = [];
            vm.searchdata = {};
            vm.item = item;                        
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
                console.log('item = ', item);
                if (item != null || item != "") {
                    vm.loading = true;                    
                    claimFolderService.getFolderDetails(item)
                        .then(function (result) {
                            vm.list = result.data;
                            console.log('list', vm.list);
                            vm.totalCount = result.data.folderList.totalCount + result.data.fileList.totalCount;
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                }
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
                vm.temp = vm.item + "\\" + temp;
                console.log(vm.temp)
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalClaimFoldersDetail.cshtml',
                    controller: 'tenant.views.OldList.modalClaimFoldersDetail as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        item: function () {
                            return vm.temp;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    refeshDatatable();
                });
            };

            //Download file
            vm.download = function (fid, event) {
                event.preventDefault();
                App.startPageLoading({ animate: true });
                vm.temp = vm.item + "\\" + fid;
                console.log('Download: ', fid);
                claimFolderService.downloadClaimFile(fid, vm.temp)
                    .then(function (result) {
                        console.log('Download result', result.data);
                        app.downloadTempFile(result.data);
                    })
                    .finally(function () {
                        App.stopPageLoading({ animate: true });
                    });
            };
        }
    ]);
})();