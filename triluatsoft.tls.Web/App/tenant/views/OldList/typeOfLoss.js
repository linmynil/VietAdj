(function () {
    appModule.controller('tenant.views.OldList.typeOfLoss', [
        '$scope', '$uibModal','abp.services.app.typeOfLoss',
        function ($scope, $uibModal, typeOfLossService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.list = [];

            vm.getAll = function () {
                typeOfLossService.getAll({})
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
                            typeOfLossService.delete(item.id).then(function () {
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                                vm.getAll();
                            });
                        }
                    }
                );
            };
            
            vm.openModal = function (itemIn) {
                console.log('open createOrUpdateTypeOfLossModal', itemIn);
                vm.item = itemIn;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/createOrUpdateTypeOfLossModal.cshtml',
                    controller: 'tenant.views.OldList.createOrUpdateTypeOfLossModal as vm',
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