(function () {
    appModule.controller('tenant.views.OldList.expenseType', [
        '$scope', '$uibModal','abp.services.app.expenseType',
        function ($scope, $uibModal, expenseTypeService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.list = [];

            vm.getAll = function () {
                expenseTypeService.getAll({})
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
                            expenseTypeService.delete(item.id).then(function () {
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                                vm.getAll();
                            });
                        }
                    }
                );
            };
            
            vm.openModal = function (itemIn) {
                console.log('open createOrUpdateExpenseTypeModal', itemIn);
                vm.item = itemIn;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/createOrUpdateExpenseTypeModal.cshtml',
                    controller: 'tenant.views.OldList.createOrUpdateExpenseTypeModal as vm',
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