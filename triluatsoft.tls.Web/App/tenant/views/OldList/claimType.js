(function () {
    appModule.controller('tenant.views.OldList.claimType', [
        '$scope', '$uibModal','abp.services.app.claimType',
        function ($scope, $uibModal, claimTypeService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.list = [];

            vm.getAll = function () {
                claimTypeService.getAll({})
                    .then(function (result) {
                        vm.list = result.data;
                        console.log('list', vm.list);
                    });
            };
            vm.getAll();

            vm.delete = function (item) {
                abp.message.confirm(
                    '',
                    function (isConfirmed) {
                        if (isConfirmed) {
                            claimTypeService.delete(item.id).then(function () {
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                                vm.getAll();
                            });
                        }
                    }
                );
            };
            
            vm.openModal = function (itemIn) {
                console.log('open createOrUpdateClaimTypeModal', itemIn);
                vm.item = itemIn;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/createOrUpdateClaimTypeModal.cshtml',
                    controller: 'tenant.views.OldList.createOrUpdateClaimTypeModal as vm',
                    backdrop: 'static',
                    resolve: {
                        item: function () {
                            return vm.item;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };

        }
    ]);
})();